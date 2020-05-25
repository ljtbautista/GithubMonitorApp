using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;

namespace GithubMonitorApp
{
    public static class GithubMonitor
    {
        [FunctionName("GithubMonitor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Our Github Monitor processed and action.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            //do something with data here
            log.LogInformation(requestBody);




            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("progress_dba@yahoo.com", "Dad"),
                Subject = "Github Triggered",
                PlainTextContent = "Hey it worked!",
                HtmlContent = "<strong>Hey it worked!</strong>"
            };
            msg.AddTo(new EmailAddress("ljtbautista@gmail.com", "Lauren"));
            var response = await client.SendEmailAsync(msg);




            return new OkResult();
        }
    }
}
