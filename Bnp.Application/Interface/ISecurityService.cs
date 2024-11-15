using Bnp.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Application.Interface
{
    public interface ISecurityService
    {
        Task<List<SecurityNotificationDto>> PerformSecurityIsinAsync(SecurityDto securityDto, CancellationToken cancellationToken);
    }
}
