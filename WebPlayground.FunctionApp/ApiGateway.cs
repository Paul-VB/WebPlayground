using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore;

namespace WebPlayground.FunctionApp
{
    public class ApiGateway
    {

        public ApiGateway()
        {
        }

        [Function("ApiGateway")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", "patch", Route = "{*segments}")] HttpRequest req)
        {
            return new OkObjectResult("this should hit from all urls");
        }
    }
}