using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Pipelines.Blocks
{
    [PipelineDisplayName("PopulateWarrantyNotesActionsBlock")]
    class PopulateWarrantyNotesActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument cannot be null.");

            /* STUDENT: Complete the Run method as specified in the requirements */
            if (arg.Name.Equals("WarrantyNotes", System.StringComparison.InvariantCultureIgnoreCase))
            {
                EntityActionView entityActionView = new EntityActionView();
                entityActionView.Name = "WarrantyNotes-Edit";
                entityActionView.DisplayName = "Edit Warranty Notes";
                entityActionView.Description = "Edit Warranty Notes";
                entityActionView.IsEnabled = true;
                entityActionView.EntityView = "WarrantyNotes";
                entityActionView.Icon = "edit";
                arg.GetPolicy<ActionsPolicy>().Actions.Add(entityActionView);
            }

            return Task.FromResult(arg);
        }
    }
}
