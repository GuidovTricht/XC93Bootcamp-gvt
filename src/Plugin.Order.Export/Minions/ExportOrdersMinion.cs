using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines;
using Plugin.Bootcamp.Exercises.Order.Export.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using System;
using System.Threading.Tasks;
using System.Linq;
using XC = Sitecore.Commerce.Plugin.Orders;

namespace Plugin.Bootcamp.Exercises.Order.Export.Minions
{
    public class ExportOrdersMinion : Minion
    {
        protected IExportOrderMinionPipeline ExportOrderPipeline { get; set; }

        public override void Initialize(IServiceProvider serviceProvider,
            MinionPolicy policy,
            CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, policy, globalContext);
            ExportOrderPipeline = serviceProvider.GetService<IExportOrderMinionPipeline>();
        }

        protected override async Task<MinionRunResultsModel> Execute()
        {
            MinionRunResultsModel runResults = new MinionRunResultsModel();
            /* STUDENT: Complete the body of this method. You need to pull from an appropriate list
             * and then execute an appropriate pipeline. */

            int itemsProcessed = 0;
            foreach(var listName in this.Policy.ListsToWatch)
            {
                CommerceList<CommerceEntity> commerceList = await this.GetItems<XC.Order>(listName, this.Policy.ItemsPerBatch, 0).ConfigureAwait(false);
                long totalItemCount = commerceList.TotalItemCount;
                if(totalItemCount > 0)
                {
                    foreach (var order in commerceList.Items.OfType<XC.Order>())
                    {
                        CommercePipelineExecutionContextOptions executionContextOptions = new CommercePipelineExecutionContextOptions(new CommerceContext(Logger, MinionContext.TelemetryClient, null));
                        var processedOrder = await ExportOrderPipeline.Run(new ExportOrderArgument(order.Id), executionContextOptions).ConfigureAwait(false);
                        itemsProcessed++;
                    }
                }
            }

            runResults.DidRun = true;
            runResults.ItemsProcessed = itemsProcessed;
            return runResults;
        }
    }
}
