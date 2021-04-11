using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckYourStocks.Models;

namespace CheckYourStocks.Infrastructure
{
    public class EventModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context) 
        {
            if (context == null) 
                throw new ArgumentNullException(nameof(context));

            var parseDate = context.ValueProvider.GetValue("Date");
            var parseCost = context.ValueProvider.GetValue("Cost");
            var parseName = context.ValueProvider.GetValue("Name");
           
            DateTime.TryParse(parseDate.FirstValue, out var parsedDate);
            DateTime date = new DateTime(parsedDate.Year,parsedDate.Month,parsedDate.Day);

            double cost = Convert.ToDouble(parseCost.FirstValue.Replace('.',','));

            context.Result = ModelBindingResult.Success(new Stock{Date = date,
                Name = parseName.FirstValue,
                    Cost = cost});

            return Task.CompletedTask;
        }
    }
}
