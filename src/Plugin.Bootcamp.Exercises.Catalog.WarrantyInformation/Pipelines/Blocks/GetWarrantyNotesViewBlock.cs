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
            var entityViewArgument = _viewCommander.CurrentEntityViewArgument(context.CommerceContext);
            var sellableItem = entityViewArgument?.Entity as SellableItem;

            if(sellableItem != null || (!isVariantView && !isMasterView) || !isWarrantyNotesView)
            {
                var variationId = string.Empty;
                if (!string.IsNullOrEmpty(arg.ItemId))
                {
                    variationId = arg.ItemId;
                }

                var isEditView = arg.Action.Equals("WarrantyNotes-Edit");
                if (!isEditView)
                {
                    arg.ChildViews.Add(new EntityView()
                    {

                    })
                }
            }

            return Task.FromResult(arg);
        }
    }
}
