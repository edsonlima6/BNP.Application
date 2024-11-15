using Bnp.Application.Common;
using Bnp.Application.Entity;
using FluentValidation;

namespace Bnp.Application.Validator
{
    public class SecurityValidator : AbstractValidator<Security>
    {
        public SecurityValidator()
        {
            RuleFor(x => x.Isin).Length(ConstantProvider.securityIsinLenghPatter.Length);
        }
    }
}
