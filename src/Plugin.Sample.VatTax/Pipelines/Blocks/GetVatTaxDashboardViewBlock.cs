using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Plugin.Bootcamp.Exercises.VatTax.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Linq;

namespace Plugin.Bootcamp.Exercises.VatTax.EntityViews
{
    [PipelineDisplayName("GetVatTaxDashboardViewBlock")]
    public class GetVatTaxDashboardViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;
        
        public GetVatTaxDashboardViewBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }
        
        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Contract.Requires(entityView != null);
            Contract.Requires(context != null);
            Condition.Requires(entityView).IsNotNull($"{this.Name}: The argument cannot be null");

            /* STUDENT: Complete the body of the Run method. You should handle the 
             * entity view for both a new and existing entity. */
            EntityViewArgument entityViewArgument = context.CommerceContext.GetObjects<EntityViewArgument>().FirstOrDefault<EntityViewArgument>();
            if (string.IsNullOrEmpty(entityViewArgument?.ViewName))
                return entityView;

            if (entityViewArgument.ViewName.Equals("VatTaxDashboard", StringComparison.OrdinalIgnoreCase))
            {
                //Add list
                var dashboardListView = new EntityView();
                dashboardListView.Name = "VatTaxList";
                entityView.ChildViews.Add(dashboardListView);
                dashboardListView.UiHint = "Table";

                var allEntities = context.CommerceContext.GetEntities<VatTaxEntity>();
                foreach(var entity in allEntities)
                {
                    var ev = new EntityView();
                    ev.EntityId = entity.Id;
                    ev.ItemId = entity.Id;
                    ev.DisplayName = entity.DisplayName;
                    ev.Name = "Summary";

                    ev.Properties.Add(new ViewProperty
                    {
                        Name = "ItemId",
                        RawValue = entity.Id,
                        IsHidden = true,
                        IsReadOnly = true
                    });
                    ev.Properties.Add(new ViewProperty
                    {
                        Name = "Tag",
                        RawValue = entity.TaxTag,
                        IsReadOnly = true,
                        UiType = "EntityLink"
                    });
                    ev.Properties.Add(new ViewProperty
                    {
                        Name = "Country code",
                        RawValue = entity.CountryCode,
                        IsReadOnly = true,
                        UiType = "EntityLink"
                    });
                    ev.Properties.Add(new ViewProperty
                    {
                        Name = "Pct",
                        RawValue = entity.TaxPct,
                        IsReadOnly = true,
                        UiType = "EntityLink"
                    });

                    dashboardListView.ChildViews.Add(ev);
                }
            }



            
            return entityView;
        }
    }
}
