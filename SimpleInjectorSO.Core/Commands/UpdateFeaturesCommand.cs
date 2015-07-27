namespace SimpleInjectorSO.Core.Commands
{
    using System.Collections.Generic;
    using Entities;

    public class UpdateFeaturesCommand<TFeature, TAttributes>
        where TFeature : Feature<TAttributes>
        where TAttributes : FeatureAttributes
    {
        public IEnumerable<TFeature> OriginalFeatures { get; set; }
        public IEnumerable<TFeature> Features { get; set; }
    }
}
