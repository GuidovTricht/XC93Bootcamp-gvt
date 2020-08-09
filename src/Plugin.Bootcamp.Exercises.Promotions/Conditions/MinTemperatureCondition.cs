using System;

using Sitecore.Commerce.Core;
using Sitecore.Framework.Rules;
using Sitecore.Commerce.Plugin.Carts;

namespace Plugin.Bootcamp.Exercises.Promotions
{
    [EntityIdentifier("MinTemperatureCondition")]
    public class MinTemperatureCondition : ICartsCondition
    {
        /* STUDENT: Add IRuleValue properties for the city, country, and
         * minimum temperature.
         */

        public bool Evaluate(IRuleExecutionContext context)
        {
            /* STUDENT: Complete the Evaluate method to:
             * Retrieve the current temperature
             * Compare it to the temperature stored in the Policy
             * Return the result of that comparison
             */

            
            // STUDENT: Replace this line. It is only provided so the stub 
            // project will build cleanly
            return false;
        }

        public decimal GetCurrentTemperature(string city, string country, string applicationId)
        {
            WeatherService weatherService = new WeatherService(applicationId);
            var temperature = weatherService.GetCurrentTemperature(city, country).Result;

            return (decimal) temperature.Max;
        }
    }
}
