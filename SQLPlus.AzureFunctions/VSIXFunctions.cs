using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using SqlPlusBase;
using SQLPlus.AzureFunctions.Default;
using SQLPlus.AzureFunctions.Utilities;

namespace SQLPlus.AzureFunctions
{
    public class Customer
    {
        public string CustomerToken { set; get; }
        public string Email { set; get; }
        public DateTime Expires { set; get; }
        public int SubscriptionTypeId { set; get; }
    }

    public class Login : ValidInput
    {
        [Required]
        [EmailAddress]
        [MaxLength(64)]
        public string Email { set; get; }

        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string Password { set; get; }
    }

    public static class VSIXFunctions
    {
        [FunctionName("Login")]
        public static async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            
            string json = await new StreamReader(req.Body).ReadToEndAsync();
            if(json is null)
            {
                return new BadRequestResult();
            }

            Login login = JsonConvert.DeserializeObject<Login>(json);
            if(!login.IsValid())
            {
                return new BadRequestResult();
            }

            var dataservice = new Service(Environment.GetEnvironmentVariable("Connection"));
            var lookupOutput = dataservice.CustomerLookup(new(login.Email));
            if(lookupOutput.ReturnValue == Default.Models.CustomerLookupOutput.Returns.NotFound)
            {
                return new BadRequestResult();
            }

            var passwordOutput = dataservice.CustomerPasswordById(new(lookupOutput.CustomerId.Value));
            var password = passwordOutput.ResultData;

            var passwordHelper = new PasswordHelper();
            if(passwordHelper.IsMatch(login.Password, password.PasswordHash, password.PasswordSalt, password.PasswordIterations ))
            {
                var subscriptionOutput = dataservice.SubscriptionByCustomerId(new(lookupOutput.CustomerId.Value));
                if(subscriptionOutput.ReturnValue == Default.Models.SubscriptionByCustomerIdOutput.Returns.Individual)
                {
                    return new OkResult();
                }

                if(subscriptionOutput.ReturnValue == Default.Models.SubscriptionByCustomerIdOutput.Returns.TeamMember)
                {
                    return new OkResult();
                }

                if(subscriptionOutput.ReturnValue == Default.Models.SubscriptionByCustomerIdOutput.Returns.TeamOwner)
                {
                    return new OkResult();
                }

                if(subscriptionOutput.ReturnValue == Default.Models.SubscriptionByCustomerIdOutput.Returns.NotFound)
                {
                    return new OkResult();
                }
            }


            return new OkObjectResult("Nothing");
        }
    }
}
