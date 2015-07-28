namespace SimpleInjectorSO.Core.BL
{
    using System.Collections.Generic;
    using Interfaces;

    public class Transaction
    {
        public Transaction(string identifier)
        {
            this.Identifier = identifier;
            this.TransactionSteps = new List<ITransactionalCommandHandler>();
            this.NeedToRollback = false;
        }

        public string Identifier { get; set; }

        public List<ITransactionalCommandHandler> TransactionSteps { get; set; }

        public bool NeedToRollback { get; set; }
    }
}
