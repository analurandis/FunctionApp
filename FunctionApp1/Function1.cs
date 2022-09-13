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
            var datas = "";
            var mensagem = "";


            if (Byte.MaxValue == date.DayOfYear -1 )
            {
                for (int i = 0; i <= Byte.MaxValue; i++)
                {
                    datas += $"\n Byte: {i} Dia: {i + 1} Data: {new DateTime(date.Year, 1, 1).AddDays(i).ToString("dd.MM.yyyy")} ";
                }
                mensagem = $"Programador, {name}. \n Hoje e seu dia sabe porque? Role a pagina ate o Fim. \n {datas} \n Dia {date.ToString("dd/MM/yyyy")} representa o valor maximo de 1 Byte que e {Byte.MaxValue} lembre-se de contar o 0";
            }
            else
            {
                if (Byte.MaxValue < date.DayOfYear -1)
                {
                    mensagem = $"Data atual: {date.ToString("dd/MM/yyyy")} \n Já se passaram {(date.DayOfYear -1)- Byte.MaxValue} dias do dia do programador ";
                }
                else
                {
                    mensagem = $"Data atual: {date.ToString("dd/MM/yyyy")} \n Faltam  {Byte.MaxValue - (date.DayOfYear - 1) } para o dia do programador";
                }
                
            }

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : mensagem;

            return new OkObjectResult(responseMessage);
        }
    }
}

