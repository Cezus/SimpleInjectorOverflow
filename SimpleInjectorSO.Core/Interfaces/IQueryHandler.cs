namespace SimpleInjectorSO.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}
