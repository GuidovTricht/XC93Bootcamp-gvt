using Sitecore.Commerce.Core;

namespace Plugin.Bootcamp.Exercises.Catalog.WarrantyInformation.Components
{
    public class WarrantyNotesComponent : Component
    {
        /* STUDENT: Add properties as specified in the requirements */
        public string WarrantyType { get; set; } //Gold or Silver
        public int WarrantyTerm { get; set; } //Years; 1, 2 or 3
    }
}
