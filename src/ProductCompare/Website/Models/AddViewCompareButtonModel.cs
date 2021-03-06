﻿using Sitecore.Commerce.XA.Foundation.Common.Models;
using Sitecore.Data.Items;

namespace Feature.Website.Models
{
    public class AddViewCompareButtonModel : BaseCommerceRenderingModel
    {
        public string AddToCompareButtonText { get; set; }
        public string ViewCompareButtonText { get; set; }
        public string AddingToCompareWaitText { get; set; }
        public string ComparePageLink { get; set; }
        public bool IsProductInCompareList { get; set; }
        public string CatalogName { get; set; }
        public string ProductId { get; set; }
        public string VariantId { get; set; }
        public bool IsValid { get; set; }

        public void Initialize(Item productItem)
        {
            ProductId = productItem.Name;
        }
    }
}