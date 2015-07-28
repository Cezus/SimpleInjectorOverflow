namespace SimpleInjectorSO.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface ITransactionEngine
    {
        Task StartTransaction(string identifier);

        Task AddTransactionStep(string identifier, ITransactionalCommandHandler commandHandler);

        Task MarkForRollback(string identifier);

        Task FinishTransaction(string identifier);
    }
}
