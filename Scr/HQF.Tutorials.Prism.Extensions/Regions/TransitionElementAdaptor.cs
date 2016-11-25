using System;
using System.Collections.Specialized;
using System.Windows;
using Prism.Regions;
using Transitionals.Controls;

namespace HQF.Tutorials.Prism.Extensions.Regions
{
    /// <summary>
    /// 又变换效果的
    /// https://sachabarbs.wordpress.com/2011/01/04/prism-4-custom-transitioning-region/
    /// 
    /// </summary>
    public class TransitionElementAdaptor : RegionAdapterBase<TransitionElement>
    {
        public TransitionElementAdaptor(IRegionBehaviorFactory behaviorFactory) :
            base(behaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, TransitionElement regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                //Add
                if (e.Action == NotifyCollectionChangedAction.Add)
                    foreach (FrameworkElement element in e.NewItems)
                        regionTarget.Content = element;

                //Removal
                if (e.Action == NotifyCollectionChangedAction.Remove)
                    foreach (FrameworkElement element in e.OldItems)
                    {
                        regionTarget.Content = null;
                        GC.Collect();
                    }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}