


using Bnp.Application.Dto;
using System.Net;
using System.Text.Json;

namespace Bnp.Application.TDD
{
    public class HttpMessageHandlerMock(List<string> isinCodes, decimal price, HttpStatusCode httpStatusCode) : HttpMessageHandler
    {
        List<string> isinCodes = isinCodes;
        decimal price = price;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var isinCode = isinCodes.FirstOrDefault(x => request.RequestUri.LocalPath.Contains(x));

            SecurityReponseDto securityReponseDto = new SecurityReponseDto() { Isin = isinCode, Price = price};
            var reponse = JsonSerializer.Serialize(securityReponseDto);

            var httpMessage = new HttpResponseMessage()
            {
                Content = new StringContent(reponse),
                StatusCode = httpStatusCode
            };

            return Task.FromResult(httpMessage);
        }
    }
}
