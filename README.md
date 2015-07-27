# SimpleInjectorOverflow
Voor Steven

1. Set debug point vanaf EdgeController.js regel 27
2. Start/Debug het SimpleInjectorSO.Web.Api project.
3. Bezoek in de browser de volgende url: http://[root]:[port]/api/edge
4. Voer de debug tot aan onderstaand:

Het gaat mis op regel 49 in de TransactionalCommandHandlerFactory:
dependencies.Add(this.container.GetInstance(item.ParameterType));

Groeten,
