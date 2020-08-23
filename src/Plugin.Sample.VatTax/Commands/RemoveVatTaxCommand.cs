using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.VatTax.Commands
{
    public class RemoveVatTaxCommand : CommerceCommand
    {
        private readonly IRemoveListEntitiesPipeline _removeListEntitiesPipeline;
        private readonly IDeleteEntityPipeline _deleteEntityPipeline;

        public RemoveVatTaxCommand(IRemoveListEntitiesPipeline removeListEntitiesPipeline, IDeleteEntityPipeline deleteEntityPipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._removeListEntitiesPipeline = removeListEntitiesPipeline;
            this._deleteEntityPipeline = deleteEntityPipeline;
        }
        
        public async Task<bool> Process(
            CommerceContext commerceContext,
            Entities.VatTaxEntity entity)
        {
            //This should run a pipeline which creates the entity, but I'm too lazy
            var removeList = new List<string>() { entity.Id };
            var removeFromListResult = await _removeListEntitiesPipeline.Run(new ListEntitiesArgument(removeList, CommerceEntity.ListName<Entities.VatTaxEntity>()), commerceContext.PipelineContextOptions).ConfigureAwait(false);
            if (!removeFromListResult.Success)
                return false;

            var deleteResult = await _deleteEntityPipeline.Run(new DeleteEntityArgument(entity), commerceContext.PipelineContextOptions).ConfigureAwait(false);
            return deleteResult.Success;
        }
    }
}
