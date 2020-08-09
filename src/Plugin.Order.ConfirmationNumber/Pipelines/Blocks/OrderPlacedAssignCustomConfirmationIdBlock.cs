using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Policies;
using Sitecore.Commerce.Plugin.Orders;
using System.Diagnostics.Contracts;


namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Blocks
{
    [PipelineDisplayName("OrderConfirmation.OrderConfirmationIdBlock")]
    public class OrderPlacedAssignCustomConfirmationIdBlock : PipelineBlock<Sitecore.Commerce.Plugin.Orders.Order, Sitecore.Commerce.Plugin.Orders.Order, CommercePipelineExecutionContext>
    {
        public override Task<Sitecore.Commerce.Plugin.Orders.Order> Run(Sitecore.Commerce.Plugin.Orders.Order arg, CommercePipelineExecutionContext context)
        {

            Contract.Requires(arg != null);
            Contract.Requires(context != null);
            /* STUDENT: Complete this method to set the order number as specified in the requirements */
             

            return Task.FromResult<Sitecore.Commerce.Plugin.Orders.Order>(arg);
        }
    }

}
