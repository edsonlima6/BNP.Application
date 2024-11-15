using Bnp.Application.Common;
using Bnp.Application.Dto;
using Bnp.Application.Entity;
using Bnp.Application.Interface;
using FluentValidation;
using System.Text.Json;

namespace Bnp.Application.Service
{
    public class SecurityService(ISecurityRepository securityRepository, IHttpClientFactory httpClientFactory,IValidator<Security> securityValidator) : ISecurityService
    {
        ISecurityRepository _securityRepository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
        IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        IValidator<Security> _securityValidator = securityValidator ?? throw new ArgumentNullException(nameof(securityValidator));

        public async Task<List<SecurityNotificationDto>> PerformSecurityIsinAsync(SecurityDto securityDto, CancellationToken cancellationToken)
        {
            List<SecurityNotificationDto> securityNotifications = [];
            List<Security> securitiesToStore = [];

            foreach (var item in securityDto.IsinListCodes)
            {
                var security = new Security() { Isin = item };
                var validatorResult = _securityValidator.Validate(security);
                if (validatorResult.IsValid)
                {
                    var httpClient = httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(ConstantProvider.GetSecurityUrl(security.Isin), cancellationToken).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                        var responseDto = JsonSerializer.Deserialize<SecurityReponseDto>(responseContent);
                        security.Price = (responseDto is not null) ? responseDto.Price : 0;

                        securityNotifications.Add(new SecurityNotificationDto() { IsinCode = security.Isin, HttpStatusCode = response.StatusCode });
                        securitiesToStore.Add(security);
                    }

                }
            }

            if (securitiesToStore.Count > 0)
                await securityRepository.AddRangeAsync(securitiesToStore, cancellationToken);

            return securityNotifications;
        }
    }
}
