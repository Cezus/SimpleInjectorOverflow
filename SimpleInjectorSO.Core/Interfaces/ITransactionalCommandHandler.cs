namespace SimpleInjectorSO.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface ITransactionalCommandHandler
    {
        Task Rollback();
    }
}
