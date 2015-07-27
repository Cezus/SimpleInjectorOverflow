namespace ProRail.Naiade.Infrastructure.ArcGis.TransactionalCommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using SimpleInjectorSO.Core.Commands;
    using SimpleInjectorSO.Core.Entities;
    using SimpleInjectorSO.Core.Interfaces;

    public class UpdateFeaturesCommandHandler<TFeature, TAttributes> :
        ITransactionalCommandHandler,
        ICommandHandler<UpdateFeaturesCommand<TFeature, TAttributes>>
        where TFeature : Feature<TAttributes>
        where TAttributes : FeatureAttributes
    {
        private readonly ICommandHandler<UpdateFeaturesCommand<TFeature, TAttributes>> updateFeaturesCommandHandler;

        private IEnumerable<TFeature> featuresForRollback;
        private IEnumerable<TFeature> featuresToUpdate;

        public UpdateFeaturesCommandHandler(ICommandHandler<UpdateFeaturesCommand<TFeature, TAttributes>> updateFeaturesCommandHandler)
        {
            this.updateFeaturesCommandHandler = updateFeaturesCommandHandler;
        }

        public async Task Handle(UpdateFeaturesCommand<TFeature, TAttributes> command)
        {
            // When original features are filled, we asume the action can be rolled back. So when the action can be rolled back the
            // the original features and the list with features to update must be atleast the same length.
            if (command.OriginalFeatures != null && this.featuresToUpdate.Count() != this.featuresForRollback.Count())
            {
                // TODO: refactor naar Naiade custom exceptions.
                throw new Exception("Het aantal te updaten features komen niet overeen met het aantal originele features.");
            }

            this.featuresForRollback = command.OriginalFeatures;
            this.featuresToUpdate = command.Features;
        }

        /// <summary>
        /// Rollbacks the features that were updated.
        /// this.updateFeaturesCommandHandler, and not this.Handle(), is used because this instance holds the rollback data.
        /// </summary>
        /// <returns></returns>
        public async Task Rollback()
        {
            // TODO: Debuglogging
            if (this.featuresForRollback == null || this.featuresForRollback.Count() == 0)
            {
                // TODO: refactor naar Naiade custom exceptions.
                throw new Exception("Er zijn geen features om te herstellen!");
            }

            var updateFeaturesCommandHandler = new UpdateFeaturesCommand<TFeature, TAttributes>()
            {
                Features = this.featuresForRollback
            };

            await this.updateFeaturesCommandHandler.Handle(updateFeaturesCommandHandler);
        }
    }
}
