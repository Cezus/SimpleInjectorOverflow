namespace SimpleInjectorSO.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Compilation;
    using SimpleInjector;
    using Core.Interfaces;

    public class TransactionalCommandHandlerFactory : ITransactionalCommandHandlerFactory
    {
        private readonly Container container;

        public TransactionalCommandHandlerFactory(Container container)
        {
            this.container = container;
        }

        public ICommandHandler<TCommand> CreateInstance<TCommand>(TCommand commandType)
        {
            // ensure all references are loaded;
            BuildManager.GetReferencedAssemblies();
            // Get all types in current domain.
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes()).ToList();

            // Get all types with interface ITransactionalCommandHandler
            var filteredTypes = types.Where(t => !t.IsAbstract && !t.IsInterface);
            filteredTypes = filteredTypes.Where(t =>
                t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)) &&
                t.GetInterfaces().Any(i => i == typeof(ITransactionalCommandHandler))).ToList();

            // Get the CommandHandler that implements the given command type commandType
            var commandHandlerType = filteredTypes.FirstOrDefault(t =>
                t.GetInterfaces().Any(i => i.GenericTypeArguments.Any(gta => gta.GetGenericTypeDefinition() == commandType.GetType().GetGenericTypeDefinition())));

            // Create constructed type with the generics TFeature and TAttributes
            var constructed = commandHandlerType.MakeGenericType(commandType.GetType().GenericTypeArguments[0], commandType.GetType().GenericTypeArguments[1]);

            // Get constructor parameters.
            var constructor = constructed.GetConstructors()[0];
            List<object> dependencies = new List<object>();

            // Resolve dependencie for each constructor parameter.
            foreach (var item in constructor.GetParameters())
            {
                dependencies.Add(this.container.GetInstance(item.ParameterType));
            }

            // Create object with the correct constructor arguments
            var instance = Activator.CreateInstance(constructed, dependencies.ToArray());

            return (ICommandHandler<TCommand>)instance;
        }
    }
}