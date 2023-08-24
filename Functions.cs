using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace TestFunctionApp
{
    public class Functions
    {
        private readonly IConfiguration _configuration;

        public Functions(IConfiguration config)
        {
            _configuration = config;
        }

        [Function("Test")]
        public HttpResponseData RunTest([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString(this.RunEnvTest("Test__Underscore"));
            response.WriteString(this.RunEnvTest("Test:Underscore"));

            response.WriteString(this.RunConfigTest("Test__Understore"));
            response.WriteString(this.RunConfigTest("Test:Underscore"));

            return response;
        }

        private string RunEnvTest(string variable)
        {
            string value1 = Environment.GetEnvironmentVariable(variable) ?? string.Empty;
            return $"Env {variable} Returns \"{value1}\"\n";
        }

        private string RunConfigTest(string variable)
        {
            string value1 = this._configuration[variable] ?? string.Empty;
            return $"Config {variable} Returns \"{value1}\"\n";
        }
    }
}
