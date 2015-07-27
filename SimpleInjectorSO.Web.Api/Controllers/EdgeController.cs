using System.Collections.Generic;
using System.Web.Http;
using SimpleInjectorSO.Core.Commands;
using SimpleInjectorSO.Core.Entities;
using SimpleInjectorSO.Core.Interfaces;

namespace SimpleInjectorSO.Web.Api.Controllers
{
    public class EdgeController : ApiController
    {
        private ICommandHandler<UpdateEdgesTransactionCommand> updateEdgesTransactionCommand;

        public EdgeController(ICommandHandler<UpdateEdgesTransactionCommand> updateEdgesTransactionCommand)
        {
            this.updateEdgesTransactionCommand = updateEdgesTransactionCommand;
        }

        public void Get()
        {
            var command = new UpdateEdgesTransactionCommand()
            {
                Edges = new List<Edge>() {
                    new Edge()
                }
            };

            this.updateEdgesTransactionCommand.Handle(command);
        }
    }
}
