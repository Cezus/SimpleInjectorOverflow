namespace SimpleInjectorSO.Core.BL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Commands;
    using Interfaces;
    using Entities;

    public class UpdateEdgesTransactionCommandHandler : ICommandHandler<UpdateEdgesTransactionCommand>
    {
        private readonly ITransactionalCommandHandlerFactory transactionalCommandHandlerFactory;
        private readonly ITransactionEngine transactionEngine;
        private readonly string transactionId = "UpdateWithTransaction";

        public UpdateEdgesTransactionCommandHandler(
            ITransactionalCommandHandlerFactory transactionalCommandHandlerFactory,
            ITransactionEngine transactionEngine
            )
        {
            this.transactionalCommandHandlerFactory = transactionalCommandHandlerFactory;
            this.transactionEngine = transactionEngine;
        }

        public async Task Handle(UpdateEdgesTransactionCommand command)
        {
            var needToRollback = false;
            await transactionEngine.StartTransaction(this.transactionId);

            foreach (var edge in command.Edges)
            {
                try
                {
                    var updateFeaturesCommand = new UpdateFeaturesCommand<Edge, EdgeAttributes>()
                    {
                        Features = command.Edges,
                        OriginalFeatures = null
                    };

                    var updateFeaturesCommandHandler = this.transactionalCommandHandlerFactory.CreateInstance(updateFeaturesCommand);

                    if (string.IsNullOrEmpty(updateFeaturesCommand.Features.First().Attributes.Name))
                    {
                        throw new Exception("Naam is niet ingevuld!");
                    }

                    await updateFeaturesCommandHandler.Handle(updateFeaturesCommand);
                    await this.transactionEngine.AddTransactionStep(this.transactionId, (ITransactionalCommandHandler)updateFeaturesCommandHandler);
                }
                catch (Exception e)
                {
                    // TODO: Or fatal?
                    needToRollback = true;
                    break;
                }

                if (needToRollback)
                {
                    await this.transactionEngine.MarkForRollback(this.transactionId);
                }

                await this.transactionEngine.FinishTransaction(this.transactionId);
            }
        }
    }
}
