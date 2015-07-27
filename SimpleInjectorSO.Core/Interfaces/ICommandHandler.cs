namespace SimpleInjectorSO.Core.Interfaces
{
    using System.Threading.Tasks;

    public interface ICommandHandler<TCommand>
    {
        Task Handle(TCommand command);
    }
}
