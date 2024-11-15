using Bnp.Application.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Application.Interface
{
    public interface ISecurityRepository
    {
        Task AddRangeAsync(List<Security> securities, CancellationToken cancellationToken);
    }
}
