using Plugin.Bootcamp.Exercises.VatTax.Commands;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Pipelines;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("DoActionAddDashboardEntity")]
    public class DoActionAddVatTaxBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IAddListEntitiesPipeline _addListEntitiesPipeline;

        public DoActionAddVatTaxBlock(CommerceCommander commerceCommander, IPersistEntityPipeline persistEntityPipeline, IAddListEntitiesPipeline addListEntitiesPipeline)
        {
            this._commerceCommander = commerceCommander;
            this._persistEntityPipeline = persistEntityPipeline;
            this._addListEntitiesPipeline = addListEntitiesPipeline;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);

            /* STUDENT: Complete this method by adding the code to save a new Vat Tax configuration item */
            if (string.IsNullOrEmpty(entityView?.Action) || !entityView.Action.Equals("VatTax-Add", StringComparison.OrdinalIgnoreCase))
                return entityView;

            var taxTagViewProperty = entityView.Properties.FirstOrDefault<ViewProperty>((Func<ViewProperty, bool>)(p => p.Name.Equals("TaxTag", StringComparison.OrdinalIgnoreCase)));
            if (string.IsNullOrEmpty(taxTagViewProperty?.Value))
            {
                string str1 = taxTagViewProperty == null ? "TaxTag" : taxTagViewProperty.DisplayName;
                string str2 = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    str1
                }, "Invalid or missing value for property 'TaxTag'.").ConfigureAwait(false);
                return entityView;
            }
            var countryCodeViewProperty = entityView.Properties.FirstOrDefault<ViewProperty>((Func<ViewProperty, bool>)(p => p.Name.Equals("CountryCode", StringComparison.OrdinalIgnoreCase)));
            if (string.IsNullOrEmpty(countryCodeViewProperty?.Value))
            {
                string str1 = countryCodeViewProperty == null ? "CountryCode" : countryCodeViewProperty.DisplayName;
                string str2 = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    str1
                }, "Invalid or missing value for property 'CountryCode'.").ConfigureAwait(false);
                return entityView;
            }
            var taxPctViewProperty = entityView.Properties.FirstOrDefault<ViewProperty>((Func<ViewProperty, bool>)(p => p.Name.Equals("TaxPct", StringComparison.OrdinalIgnoreCase)));
            if (string.IsNullOrEmpty(taxPctViewProperty?.Value))
            {
                string str1 = taxPctViewProperty == null ? "TaxPct" : taxPctViewProperty.DisplayName;
                string str2 = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    str1
                }, "Invalid or missing value for property 'TaxPct'.").ConfigureAwait(false);
                return entityView;
            }

            var newEntity = await _commerceCommander.Command<CreateVatTaxCommand>().Process(context.CommerceContext, taxTagViewProperty.Value, countryCodeViewProperty.Value, Convert.ToInt32(taxPctViewProperty.Value)).ConfigureAwait(false);
            var results = await _persistEntityPipeline.Run(new PersistEntityArgument(newEntity), context).ConfigureAwait(false);

            var listEntitiesArgument = new ListEntitiesArgument(new string[1]
            {
                newEntity.Id
            }, CommerceEntity.ListName<Entities.VatTaxEntity>());
            await _addListEntitiesPipeline.Run(listEntitiesArgument, context).ConfigureAwait(false);

            return entityView;
        }
    }
}
