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
        public IRuleValue<String> City { get; set; }
        public IRuleValue<String> Country { get; set; }
        public IRuleValue<Decimal> MinimumTemperature { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            /* STUDENT: Complete the Evaluate method to:
             * Retrieve the current temperature
             * Compare it to the temperature stored in the Policy
             * Return the result of that comparison
             */
            var policy = context.Fact<CommerceContext>((string)null)?.GetPolicy<WeatherServiceClientPolicy>();
            if (string.IsNullOrEmpty(policy?.ApiKey))
                return false;

            var weatherService = new WeatherService(policy.ApiKey);
            if (weatherService == null)
                return false;

            var city = this.City.Yield(context);
            if (city == null)
                return false;
            var country = this.Country.Yield(context);
            if (country == null)
                return false;
            var minTemp = this.MinimumTemperature.Yield(context);

            var temp = weatherService.GetCurrentTemperature(city, country).ConfigureAwait(false).GetAwaiter().GetResult();
            if (temp?.Value == null)
                return false;

            return Convert.ToDecimal(temp.Value) > minTemp;
        }

        public decimal GetCurrentTemperature(string city, string country, string applicationId)
        {
            WeatherService weatherService = new WeatherService(applicationId);
            var temperature = weatherService.GetCurrentTemperature(city, country).Result;

            return (decimal) temperature.Max;
        }
    }
}
