using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.VatTax.Commands
{
    public class CreateVatTaxCommand : CommerceCommand
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;

        public CreateVatTaxCommand(IDoesEntityExistPipeline doesEntityExistPipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._doesEntityExistPipeline = doesEntityExistPipeline;
        }
        
        public async Task<Entities.VatTaxEntity> Process(
            CommerceContext commerceContext,
            string taxTag,
            string countryCode,
            int taxPct)
        {
            //This should run a pipeline which creates the entity, but I'm too lazy
            var id = taxTag.ToEntityId<Entities.VatTaxEntity>();
            if(await _doesEntityExistPipeline.Run(new FindEntityArgument(typeof(Entities.VatTaxEntity), id, false), commerceContext.PipelineContextOptions).ConfigureAwait(false))
            {
                //Should add error message
                return null;
            }

            var entity = new Entities.VatTaxEntity
            {
                Id = id,
                FriendlyId = taxTag,
                DisplayName = taxTag,
                TaxTag = taxTag,
                CountryCode = countryCode,
                TaxPct = taxPct
            };
            return entity;
        }
    }
}
