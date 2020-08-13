using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components;


namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Pipelines.Blocks
{
    [PipelineDisplayName("DoActionEditWarrantyNotesBlock")]
    class DoActionEditWarrantyNotesBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public DoActionEditWarrantyNotesBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            /* STUDENT: Complete the Run method as specified in the requirements */
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals("WarrantyNotes-Edit", StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrEmpty(arg.EntityId))
                return Task.FromResult(arg);

            var entity = context.CommerceContext.GetObject<SellableItem>(x => x.Id.Equals(arg.EntityId));
            if (entity == null)
                return Task.FromResult(arg);

            var warrantyNotesComponent = entity.GetComponent<WarrantyNotesComponent>(arg.ItemId);
            var termProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(WarrantyNotesComponent.WarrantyTerm), StringComparison.OrdinalIgnoreCase))?.Value;
            if(!string.IsNullOrEmpty(termProperty))
                warrantyNotesComponent.WarrantyTerm = Convert.ToInt32(termProperty);
            var typeProperty = arg.Properties.FirstOrDefault(x => x.Name.Equals(nameof(WarrantyNotesComponent.WarrantyType), StringComparison.OrdinalIgnoreCase))?.Value;
            if (!string.IsNullOrEmpty(typeProperty))
                warrantyNotesComponent.WarrantyType = typeProperty;
            
            // Persist changes
            this._commerceCommander.Pipeline<IPersistEntityPipeline>().Run(new PersistEntityArgument(entity), context);

            return Task.FromResult(arg);
        }
    }
}
