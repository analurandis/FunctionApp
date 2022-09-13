using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> log)
        {
            _logger = log;
        }

        [FunctionName("Function1")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain ; charset=utf-8", bodyType: typeof(string),Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var date = DateTime.Now;
            var array = "";
            for (int i = 0; i<= Byte.MaxValue; i++ )
            {
                array += $"\n Byte: {i} Dia: {i + 1} Data: {new DateTime(date.Year, 1, 1).AddDays(i).ToString("dd.MM.yyyy")} ";
            }
            var dia = date.DayOfYear;
            var diaByte = Byte.MaxValue;

            Console.WriteLine(date);
            Console.WriteLine(dia);
            Console.WriteLine(diaByte);

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Programador, {name}. \n Hoje e seu dia sabe porque? Role a pagina ate o Fim. \n {array} \n Dia {date} representa o valor maximo de 1 Byte que e {Byte.MaxValue} lembre-se de contar o 0";

            return new OkObjectResult(responseMessage);
        }
    }
}

