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

            
            return runResults;
        }


    }
}
