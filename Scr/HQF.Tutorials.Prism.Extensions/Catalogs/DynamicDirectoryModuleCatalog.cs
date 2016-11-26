﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Threading;
using Prism.Modularity;
using Microsoft.Practices.ServiceLocation;

namespace HQF.Tutorials.Prism.Extensions.Catalogs
{
    /// <summary>
    /// Dynamic Load Modules
    ///
    /// eg.
    ///  DynamicDirectoryModuleCatalog catalog = new DynamicDirectoryModuleCatalog(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Modules"));
    /// </summary>
    public class DynamicDirectoryModuleCatalog : ModuleCatalog
    {
        private readonly SynchronizationContext _context;

        public DynamicDirectoryModuleCatalog(string modulePath)
        {
            _context = SynchronizationContext.Current;

            ModulePath = modulePath;
            // we need to watch our folder for newly added modules
            if (Directory.Exists(ModulePath))
            {
            var fileWatcher = new FileSystemWatcher(ModulePath, "*.dll");
            fileWatcher.Created += FileWatcher_Created;
            fileWatcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        ///     Directory containing modules to search for.
        /// </summary>
        public string ModulePath { get; set; }

        /// <summary>
        ///     Rasied when a new file is added to the ModulePath directory
        /// </summary>
        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
                LoadModuleCatalog(e.FullPath, true);
        }

        /// <summary>
        ///     Drives the main logic of building the child domain and searching for the assemblies.
        /// </summary>
        protected override void InnerLoad()
        {
            LoadModuleCatalog(ModulePath);
        }

        private void LoadModuleCatalog(string path, bool isFile = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("Path cannot be null.");

            if (isFile)
            {
                if (!File.Exists(path))
                    throw new InvalidOperationException(string.Format("File {0} could not be found.", path));
            }
            else
            {
                if (!Directory.Exists(path))
                    throw new InvalidOperationException(string.Format("Directory {0} could not be found.", path));
            }

            var childDomain = BuildChildDomain(AppDomain.CurrentDomain);

            try
            {
                var loadedAssemblies = new List<string>();

                var assemblies = from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                                 where !(assembly is AssemblyBuilder)
                                       && (assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder")
                                       && !string.IsNullOrEmpty(assembly.Location)
                                 select assembly.Location;

                loadedAssemblies.AddRange(assemblies);

                var loaderType = typeof(InnerModuleInfoLoader);
                if (loaderType.Assembly != null)
                {
                    var loader =
                        (InnerModuleInfoLoader)
                        childDomain.CreateInstanceFrom(loaderType.Assembly.Location, loaderType.FullName).Unwrap();
                    loader.LoadAssemblies(loadedAssemblies);

                    //get all the ModuleInfos
                    var modules = loader.GetModuleInfos(path, isFile);

                    //add modules to catalog
                    Items.AddRange(modules);

                    //we are dealing with a file from our file watcher, so let's notify that it needs to be loaded
                    if (isFile)
                        LoadModules(modules);
                }
            }
            finally
            {
                AppDomain.Unload(childDomain);
            }
        }

        /// <summary>
        ///     Uses the IModuleManager to load the modules into memory
        /// </summary>
        /// <param name="modules"></param>
        private void LoadModules(ModuleInfo[] modules)
        {
            if (_context == null)
                return;

            IModuleManager manager = ServiceLocator.Current.GetInstance<IModuleManager>();

            _context.Send(delegate
            {
                foreach (var module in modules)
                    manager.LoadModule(module.ModuleName);
            }, null);
        }

        /// <summary>
        ///     Creates a new child domain and copies the evidence from a parent domain.
        /// </summary>
        /// <param name="parentDomain">The parent domain.</param>
        /// <returns>The new child domain.</returns>
        /// <remarks>
        ///     Grabs the <paramref name="parentDomain" /> evidence and uses it to construct the new
        ///     <see cref="AppDomain" /> because in a ClickOnce execution environment, creating an
        ///     <see cref="AppDomain" /> will by default pick up the partial trust environment of
        ///     the AppLaunch.exe, which was the root executable. The AppLaunch.exe does a
        ///     create domain and applies the evidence from the ClickOnce manifests to
        ///     create the domain that the application is actually executing in. This will
        ///     need to be Full Trust for Composite Application Library applications.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     An <see cref="ArgumentNullException" /> is thrown if
        ///     <paramref name="parentDomain" /> is null.
        /// </exception>
        protected virtual AppDomain BuildChildDomain(AppDomain parentDomain)
        {
            if (parentDomain == null) throw new ArgumentNullException("parentDomain");

            var evidence = new Evidence(parentDomain.Evidence);
            var setup = parentDomain.SetupInformation;
            return AppDomain.CreateDomain("DiscoveryRegion", evidence, setup);
        }

        private class InnerModuleInfoLoader : MarshalByRefObject
        {
            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            internal ModuleInfo[] GetModuleInfos(string path, bool isFile = false)
            {
                var moduleReflectionOnlyAssembly =
                    AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().First(
                        asm => asm.FullName == typeof(IModule).Assembly.FullName);

                var IModuleType = moduleReflectionOnlyAssembly.GetType(typeof(IModule).FullName);

                FileSystemInfo info = null;
                if (isFile)
                    info = new FileInfo(path);
                else
                    info = new DirectoryInfo(path);

                ResolveEventHandler resolveEventHandler =
                    delegate (object sender, ResolveEventArgs args) { return OnReflectionOnlyResolve(args, info); };
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolveEventHandler;
                var modules = GetNotAllreadyLoadedModuleInfos(info, IModuleType);
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolveEventHandler;

                return modules.ToArray();
            }

            private static IEnumerable<ModuleInfo> GetNotAllreadyLoadedModuleInfos(FileSystemInfo info, Type IModuleType)
            {
                var validAssemblies = new List<FileInfo>();
                var alreadyLoadedAssemblies = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies();

                var fileInfo = info as FileInfo;
                if (fileInfo != null)
                    if (
                        alreadyLoadedAssemblies.FirstOrDefault(
                            assembly =>
                                string.Compare(Path.GetFileName(assembly.Location), fileInfo.Name,
                                    StringComparison.OrdinalIgnoreCase) == 0) == null)
                    {
                        var moduleInfos = Assembly.ReflectionOnlyLoadFrom(fileInfo.FullName).GetExportedTypes()
                            .Where(IModuleType.IsAssignableFrom)
                            .Where(t => t != IModuleType)
                            .Where(t => !t.IsAbstract).Select(t => CreateModuleInfo(t));

                        return moduleInfos;
                    }

                var directory = info as DirectoryInfo;

                var files = directory.GetFiles("*.dll").Where(file => alreadyLoadedAssemblies.
                                                                          FirstOrDefault(
                                                                              assembly =>
                                                                                  string.Compare(
                                                                                      Path.GetFileName(assembly.Location),
                                                                                      file.Name,
                                                                                      StringComparison.OrdinalIgnoreCase) ==
                                                                                  0) == null);

                foreach (var file in files)
                    try
                    {
                        Assembly.ReflectionOnlyLoadFrom(file.FullName);
                        validAssemblies.Add(file);
                    }
                    catch (BadImageFormatException)
                    {
                        // skip non-.NET Dlls
                    }

                return validAssemblies.SelectMany(file => Assembly.ReflectionOnlyLoadFrom(file.FullName)
                    .GetExportedTypes()
                    .Where(IModuleType.IsAssignableFrom)
                    .Where(t => t != IModuleType)
                    .Where(t => !t.IsAbstract)
                    .Select(type => CreateModuleInfo(type)));
            }

            private static Assembly OnReflectionOnlyResolve(ResolveEventArgs args, FileSystemInfo info)
            {
                var loadedAssembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(
                    asm => string.Equals(asm.FullName, args.Name, StringComparison.OrdinalIgnoreCase));
                if (loadedAssembly != null)
                    return loadedAssembly;

                var directory = info as DirectoryInfo;
                if (directory != null)
                {
                    var assemblyName = new AssemblyName(args.Name);
                    var dependentAssemblyFilename = Path.Combine(directory.FullName, assemblyName.Name + ".dll");
                    if (File.Exists(dependentAssemblyFilename))
                        return Assembly.ReflectionOnlyLoadFrom(dependentAssemblyFilename);
                }

                return Assembly.ReflectionOnlyLoad(args.Name);
            }

            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            internal void LoadAssemblies(IEnumerable<string> assemblies)
            {
                foreach (var assemblyPath in assemblies)
                    try
                    {
                        Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                    }
                    catch (FileNotFoundException)
                    {
                        // Continue loading assemblies even if an assembly can not be loaded in the new AppDomain
                    }
            }

            private static ModuleInfo CreateModuleInfo(Type type)
            {
                var moduleName = type.Name;
                var dependsOn = new List<string>();
                var onDemand = false;
                var moduleAttribute =
                    CustomAttributeData.GetCustomAttributes(type)
                        .FirstOrDefault(
                            cad => cad.Constructor.DeclaringType.FullName == typeof(ModuleAttribute).FullName);

                if (moduleAttribute != null)
                    foreach (var argument in moduleAttribute.NamedArguments)
                    {
                        var argumentName = argument.MemberInfo.Name;
                        switch (argumentName)
                        {
                            case "ModuleName":
                                moduleName = (string)argument.TypedValue.Value;
                                break;

                            case "OnDemand":
                                onDemand = (bool)argument.TypedValue.Value;
                                break;

                            case "StartupLoaded":
                                onDemand = !(bool)argument.TypedValue.Value;
                                break;
                        }
                    }

                var moduleDependencyAttributes =
                    CustomAttributeData.GetCustomAttributes(type)
                        .Where(
                            cad => cad.Constructor.DeclaringType.FullName == typeof(ModuleDependencyAttribute).FullName);
                foreach (var cad in moduleDependencyAttributes)
                    dependsOn.Add((string)cad.ConstructorArguments[0].Value);

                var moduleInfo = new ModuleInfo(moduleName, type.AssemblyQualifiedName)
                {
                    InitializationMode =
                        onDemand
                            ? InitializationMode.OnDemand
                            : InitializationMode.WhenAvailable,
                    Ref = type.Assembly.CodeBase
                };
                moduleInfo.DependsOn.AddRange(dependsOn);
                return moduleInfo;
            }
        }
    }

    /// <summary>
    ///     Class that provides extension methods to Collection
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Add a range of items to a collection.
        /// </summary>
        /// <typeparam name="T">Type of objects within the collection.</typeparam>
        /// <param name="collection">The collection to add items to.</param>
        /// <param name="items">The items to add to the collection.</param>
        /// <returns>The collection.</returns>
        /// <exception cref="System.ArgumentNullException">
        ///     An <see cref="System.ArgumentNullException" /> is thrown if
        ///     <paramref name="collection" /> or <paramref name="items" /> is <see langword="null" />.
        /// </exception>
        public static Collection<T> AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (items == null) throw new ArgumentNullException("items");

            foreach (var each in items)
                collection.Add(each);

            return collection;
        }
    }
}