namespace SimpleInjectorSO.Web.Api
{
    using System;
    using System.Web.Compilation;
    using System.Web.Http;
    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.WebApi;
    using Core.Interfaces;
    using ProRail.Naiade.Infrastructure.ArcGis.TransactionalCommandHandlers;

    public static class SimpleInjectorConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new Container();
            var webapiLifestyle = new WebApiRequestLifestyle();

            BuildManager.GetReferencedAssemblies();
            AppDomain currentDomain = AppDomain.CurrentDomain;

            container.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), webapiLifestyle, currentDomain.GetAssemblies());
            container.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), webapiLifestyle, currentDomain.GetAssemblies());

            // Veroorzaakt een StackOverFlowException bij opvragen:
            container.RegisterOpenGeneric(typeof(ICommandHandler<>), typeof(UpdateFeaturesCommandHandler<,>), webapiLifestyle,
                c => !c.Handled);

            container.RegisterSingle<ITransactionalCommandHandlerFactory>(new TransactionalCommandHandlerFactory(container));

            container.RegisterWebApiControllers(config);
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}