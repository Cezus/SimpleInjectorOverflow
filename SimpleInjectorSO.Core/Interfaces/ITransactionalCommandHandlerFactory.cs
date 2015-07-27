namespace SimpleInjectorSO.Core.Interfaces
{
    public interface ITransactionalCommandHandlerFactory
    {
        ICommandHandler<TCommand> CreateInstance<TCommand>(TCommand commandToUseForHandler);
    }
}
