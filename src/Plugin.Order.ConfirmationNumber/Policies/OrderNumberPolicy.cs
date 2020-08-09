using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Order.ConfirmationNumber.Policies
{
    public class OrderNumberPolicy : Policy
    {
        public OrderNumberPolicy()
        {
            /* STUDENT: Complete the constructor to initialize the properties */
            OrderNumberPrefix = string.Empty;
            IncludeDate = false;
            OrderNumberSuffix = string.Empty;
        }
        /* STUDENT: Add read/write properties as specified in the requirements */
        public string OrderNumberPrefix { get; set; }
        public bool IncludeDate { get; set; }
        public string OrderNumberSuffix { get; set; }

    }
}
