namespace SimpleInjectorSO.Core.BL
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SimpleInjectorSO.Core.Interfaces;

    public class TransactionEngine : ITransactionEngine
    {
        //private readonly ILogger logger;

        private readonly Dictionary<string, Transaction> transactions;

        public TransactionEngine()
        {
            this.transactions = new Dictionary<string, Transaction>();
        }

        public async Task StartTransaction(string identifier)
        {
            if (!this.transactions.ContainsKey(identifier))
            {
                // TODO: log. Exception?
            }

            await Task.Run(() => this.transactions.Add(identifier, new Transaction(identifier)));
        }

        public async Task AddTransactionStep(string identifier, ITransactionalCommandHandler commandHandler)
        {
            if (!this.transactions.ContainsKey(identifier))
            {
                // TODO: log. Exception?
            }

            await Task.Run(() => this.transactions[identifier].TransactionSteps.Add(commandHandler));
        }

        public async Task MarkForRollback(string identifier)
        {
            if (!this.transactions.ContainsKey(identifier))
            {
                // TODO: log. Exception?
            }

            await Task.Run(() => transactions[identifier].NeedToRollback = true);
        }

        public async Task FinishTransaction(string identifier)
        {
            if (!this.transactions.ContainsKey(identifier))
            {
                // TODO: log. Exception?
            }

            if (this.transactions[identifier].NeedToRollback)
            {
                await this.Rollback(this.transactions[identifier]);
            }

            this.transactions.Remove(identifier);
        }

        private async Task Rollback(Transaction transaction)
        {
            if (transaction == null)
            {
                // TODO: log. Exception?
                return;
            }

            if (transaction.TransactionSteps.Count() == 0)
            {
                // TODO: log. Exception?
                return;
            }

            foreach (var transactionStep in transaction.TransactionSteps)
            {
                await transactionStep.Rollback();
            }
        }

    }
}
