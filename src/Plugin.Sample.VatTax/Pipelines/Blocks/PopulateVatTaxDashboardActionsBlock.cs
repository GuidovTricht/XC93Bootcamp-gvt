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
        public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);

            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            /* STUDENT: Add the necessary code to add the add and remove actions to the table on the Vat Tax dashboard */
            if (string.IsNullOrEmpty(entityView?.Name) || !entityView.Name.Equals("VatTaxList", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(entityView);

            var policy = entityView.GetPolicy<ActionsPolicy>();
            var allEntities = context.CommerceContext.GetEntities<VatTaxEntity>();

            entityView.GetPolicy<ActionsPolicy>().Actions.Add(new EntityActionView
            {
                Name = "VatTax-Add",
                DisplayName = "Add Vat Tax",
                Description = "Add Vat Tax",
                IsEnabled = true,
                EntityView = "Details",
                Icon = "add"
            });
            entityView.GetPolicy<ActionsPolicy>().Actions.Add(new EntityActionView
            {
                Name = "VatTax-Delete",
                DisplayName = "Delete Vat Tax",
                Description = "Delete Vat Tax",
                IsEnabled = allEntities != null && allEntities.Any(),
                EntityView = string.Empty,
                RequiresConfirmation = true,
                Icon = "delete"
            });


            return Task.FromResult(entityView);
        }
    }
}
