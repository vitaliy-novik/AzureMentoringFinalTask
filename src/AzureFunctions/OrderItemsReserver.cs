using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace OrderItemsReserver
{
	public static class OrderItemsReserver
    {
        [FunctionName("OrderItemsReserver")]
        [FixedDelayRetry(3, "00:01:00")]
        public static async Task<IActionResult> Run(
            //[ServiceBusTrigger("orderitems", Connection = "AzureServiceBusConnectionString")]string myQueueItem,
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Blob("eshoponwebblobs/{rand-guid}.json", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream blob,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {req.Body}");

            using (StreamReader sr = new StreamReader(req.Body))
            {
                await blob.WriteAsync(Convert.FromBase64String(sr.ReadToEnd()));
            }

            return new OkResult();
        }
    }
}
