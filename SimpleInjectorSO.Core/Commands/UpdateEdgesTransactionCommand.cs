namespace SimpleInjectorSO.Core.Commands
{
    using System.Collections.Generic;
    using Entities;

    public class UpdateEdgesTransactionCommand
    {
        public IEnumerable<Edge> Edges { get; set; }
    }
}
