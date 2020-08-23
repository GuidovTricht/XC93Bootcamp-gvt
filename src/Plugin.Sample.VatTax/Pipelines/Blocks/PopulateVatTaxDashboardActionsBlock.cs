using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("EnsureActions")]
    public class PopulateVatTaxDashboardActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly IFindEntitiesInListPipeline _findEntitiesInListPipeline;

        public PopulateVatTaxDashboardActionsBlock(IFindEntitiesInListPipeline findEntitiesInListPipeline)
        {
            this._findEntitiesInListPipeline = findEntitiesInListPipeline;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);

            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            /* STUDENT: Add the necessary code to add the add and remove actions to the table on the Vat Tax dashboard */
            if (string.IsNullOrEmpty(entityView?.Name) || !entityView.Name.Equals("VatTaxDashboard", StringComparison.OrdinalIgnoreCase))
                return entityView;

            var listEntities = await _findEntitiesInListPipeline.Run(new FindEntitiesInListArgument(typeof(Entities.VatTaxEntity), CommerceEntity.ListName<Entities.VatTaxEntity>(), 0, 10), context).ConfigureAwait(false);
            var hasEntities = listEntities?.List?.Items?.Where(i => i is VatTaxEntity)?.Any() ?? false;

            entityView.GetPolicy<ActionsPolicy>().Actions.Add(new EntityActionView
            {
                Name = "VatTax-Add",
                DisplayName = "Add Vat Tax",
                Description = "Add Vat Tax",
                IsEnabled = true,
                EntityView = "VatTax-Add",
                Icon = "add"
            });
            entityView.GetPolicy<ActionsPolicy>().Actions.Add(new EntityActionView
            {
                Name = "VatTax-Delete",
                DisplayName = "Delete Vat Tax",
                Description = "Delete Vat Tax",
                IsEnabled = hasEntities,
                EntityView = string.Empty,
                RequiresConfirmation = true,
                Icon = "delete"
            });


            return entityView;
        }
    }
}
