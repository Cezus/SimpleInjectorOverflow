namespace SimpleInjectorSO.Core.Entities
{
    public class Feature<T> where T : FeatureAttributes
    {
        public T Attributes { get; set; }
    }
}
