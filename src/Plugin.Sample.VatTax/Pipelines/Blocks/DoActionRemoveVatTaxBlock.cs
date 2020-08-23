using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plugin.Bootcamp.Exercises.VatTax.Commands;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("DoActionRemoveDashboardEntity")]
    public class DoActionRemoveVatTaxBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;
        private readonly IFindEntityPipeline _findEntityPipeline;

        public DoActionRemoveVatTaxBlock(CommerceCommander commerceCommander, IFindEntityPipeline findEntityPipeline)
        {
            this._commerceCommander = commerceCommander;
            this._findEntityPipeline = findEntityPipeline;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(context != null);

            /* STUDENT: Add the necessary code to remove the selected Vat Tax configuration */
            if (string.IsNullOrEmpty(entityView?.Action) || !entityView.Action.Equals("VatTax-Delete", StringComparison.OrdinalIgnoreCase))
                return entityView;

            if (string.IsNullOrEmpty(entityView.ItemId))
                return entityView;

            var entity = await _findEntityPipeline.Run(new FindEntityArgument(typeof(VatTaxEntity), entityView.ItemId), context).ConfigureAwait(false);
            await _commerceCommander.Command<RemoveVatTaxCommand>().Process(context.CommerceContext, entity as VatTaxEntity).ConfigureAwait(false);

            return entityView;
        }
    }
}
