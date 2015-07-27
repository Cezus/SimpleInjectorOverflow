namespace SimpleInjectorSO.Core.Entities
{

    /// <summary>
    /// TODO: Geometry fields toevoegen.
    /// </summary>
    public class EdgeAttributes : FeatureAttributes
    {
        public string GmlId { get; set; }

        public string Puic { get; set; }

        public string FromNodePuic { get; set; }

        public string ToNodePuic { get; set; }

        public int PlanId { get; set; }

        public int? DeletedPlanId { get; set; }

        public string Name { get; set; }
    }
}
