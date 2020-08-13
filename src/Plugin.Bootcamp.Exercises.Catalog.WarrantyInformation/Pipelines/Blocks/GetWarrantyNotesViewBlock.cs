using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Pipelines.Blocks
{
    [PipelineDisplayName("GetWarrantyNotesViewBlock")]
    public class GetWarrantyNotesViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly ViewCommander _viewCommander;

        public GetWarrantyNotesViewBlock(ViewCommander viewCommander)
        {
            this._viewCommander = viewCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");
            var catalogViewsPolicy = context.GetPolicy<KnownCatalogViewsPolicy>();

            /* STUDENT: Complete the Run method as specified in the requirements */
            var isVariantView = arg.Name.Equals(catalogViewsPolicy.Variant);
            var isMasterView = arg.Name.Equals(catalogViewsPolicy.Master);
            var isWarrantyNotesView = arg.Name.Equals("WarrantyNotes");
            var isConnectView = arg.Name.Equals(catalogViewsPolicy.ConnectSellableItem, StringComparison.InvariantCultureIgnoreCase);
            var entityViewArgument = _viewCommander.CurrentEntityViewArgument(context.CommerceContext);
            var sellableItem = entityViewArgument?.Entity as SellableItem;
            if(sellableItem == null)
                return Task.FromResult(arg);

            if (sellableItem != null || isVariantView || isMasterView || isConnectView || !isWarrantyNotesView)
            {
                var variationId = string.Empty;
                if (!string.IsNullOrEmpty(arg.ItemId))
                {
                    variationId = arg.ItemId;
                }

                var componentView = arg;
                var isEditView = arg.Action.Equals("WarrantyNotes-Edit");
                if (!isEditView)
                {
                    componentView = new EntityView()
                    {
                        Name = "WarrantyNotes",
                        DisplayName = "Warranty Notes",
                        EntityId = arg.EntityId,
                        EntityVersion = arg.EntityVersion,
                        ItemId = variationId
                    };
                    arg.ChildViews.Add(componentView);
                }

                if (sellableItem != null && (sellableItem.HasComponent<WarrantyNotesComponent>(variationId) || isVariantView || isMasterView || isEditView || isConnectView))
                {
                    var component = sellableItem.GetComponent<WarrantyNotesComponent>(variationId);

                    componentView.Properties.Add(
                    new ViewProperty
                    {
                        Name = nameof(WarrantyNotesComponent.WarrantyTerm),
                        DisplayName = "Warranty Term",
                        RawValue = component.WarrantyTerm,
                        IsReadOnly = !isEditView,
                        IsRequired = false
                    });

                    componentView.Properties.Add(
                    new ViewProperty
                    {
                        Name = nameof(WarrantyNotesComponent.WarrantyType),
                        DisplayName = "Warranty Type",
                        RawValue = component.WarrantyType,
                        IsReadOnly = !isEditView,
                        IsRequired = false
                    });
                }
            }

            return Task.FromResult(arg);
        }
    }
}
