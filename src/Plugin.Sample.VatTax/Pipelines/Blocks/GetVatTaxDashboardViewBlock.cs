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
        private readonly IFindEntitiesInListPipeline _findEntitiesInListPipeline;

        public GetVatTaxDashboardViewBlock(CommerceCommander commerceCommander, IFindEntitiesInListPipeline findEntitiesInListPipeline)
        {
            this._commerceCommander = commerceCommander;
            this._findEntitiesInListPipeline = findEntitiesInListPipeline;
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

            var isAddAction = entityViewArgument.ViewName.Equals("VatTax-Add", StringComparison.OrdinalIgnoreCase);
            if (isAddAction)
            {
                entityView.Properties.Add(new ViewProperty
                {
                    Name = "TaxTag",
                    DisplayName = "TaxTag",
                    RawValue = string.Empty,
                    IsReadOnly = false,
                    IsRequired = true,
                    IsHidden = false
                });
                entityView.Properties.Add(new ViewProperty
                {
                    Name = "CountryCode",
                    DisplayName = "CountryCode",
                    RawValue = string.Empty,
                    IsReadOnly = false,
                    IsRequired = true,
                    IsHidden = false
                });
                entityView.Properties.Add(new ViewProperty
                {
                    Name = "TaxPct",
                    DisplayName = "TaxPct",
                    RawValue = string.Empty,
                    IsReadOnly = false,
                    IsRequired = true,
                    IsHidden = false
                });

                return entityView;
            }

            var isEditAction = entityViewArgument.ViewName.Equals("VatTax-Edit", StringComparison.OrdinalIgnoreCase);
            if (isEditAction && entityViewArgument?.Entity is VatTaxEntity viewEntity)
            {
                entityView.Properties.Add(new ViewProperty
                {
                    Name = "TaxTag",
                    RawValue = viewEntity.TaxTag,
                    IsReadOnly = true,
                    IsRequired = false,
                    IsHidden = false
                });
                entityView.Properties.Add(new ViewProperty
                {
                    Name = "CountryCode",
                    RawValue = viewEntity.CountryCode,
                    IsReadOnly = false,
                    IsRequired = true,
                    IsHidden = false
                });
                entityView.Properties.Add(new ViewProperty
                {
                    Name = "TaxPct",
                    RawValue = viewEntity.TaxPct,
                    IsReadOnly = false,
                    IsRequired = true,
                    IsHidden = false
                });

                return entityView;
            }

            if (entityViewArgument.ViewName.Equals("VatTaxDashboard", StringComparison.OrdinalIgnoreCase))
            {
                //Add list
                var dashboardListView = entityView;
                dashboardListView.UiHint = "Table";

                var listEntities = await _findEntitiesInListPipeline.Run(new FindEntitiesInListArgument(typeof(Entities.VatTaxEntity), CommerceEntity.ListName<Entities.VatTaxEntity>(), 0, 10), context).ConfigureAwait(false);
                var allEntities = listEntities?.List?.Items?.Where(i => i is VatTaxEntity)?.Select(i => i as VatTaxEntity)?.ToList();
                if (allEntities == null || !allEntities.Any())
                    return entityView;

                foreach (var entity in allEntities)
                {
                    var ev = new EntityView()
                    {
                        EntityId = entity.Id,
                        ItemId = entity.Id,
                        DisplayName = entity.DisplayName,
                        Name = "Summary"
                    };
                    var viewProperties = ev.Properties;

                    viewProperties.Add(new ViewProperty
                    {
                        Name = "ItemId",
                        RawValue = entity.Id,
                        IsHidden = true,
                        IsReadOnly = true
                    });
                    viewProperties.Add(new ViewProperty
                    {
                        Name = "Tag",
                        DisplayName = "Tag",
                        RawValue = entity.TaxTag,
                        Value = entity.TaxTag,
                        IsReadOnly = true
                    });
                    viewProperties.Add(new ViewProperty
                    {
                        Name = "Country code",
                        DisplayName = "Country code",
                        RawValue = entity.CountryCode,
                        Value = entity.CountryCode,
                        IsReadOnly = true
                    });
                    viewProperties.Add(new ViewProperty
                    {
                        Name = "Pct",
                        DisplayName = "Pct",
                        RawValue = entity.TaxPct,
                        Value = entity.TaxPct.ToString(),
                        IsReadOnly = true
                    });

                    dashboardListView.ChildViews.Add(ev);
                }
            }

            return entityView;
        }
    }
}
