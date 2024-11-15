

namespace Bnp.Application.Common
{
    public static class ConstantProvider
    {
        public static string securityIsinLenghPatter = "AS1234567891";
        public static string GetSecurityUrl(string isinCode) => $"https://securities.dataprovider.com/securityprice/{isinCode}";

        public static bool IsValidIsinLenght(string isinCode) => securityIsinLenghPatter.Length == isinCode.Length;
    }
}
