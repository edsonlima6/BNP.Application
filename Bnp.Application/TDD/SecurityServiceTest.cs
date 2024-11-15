
using Bnp.Application.Common;
using Bnp.Application.Dto;
using Bnp.Application.Entity;
using Bnp.Application.Interface;
using Bnp.Application.Service;
using Bnp.Application.Validator;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;

namespace Bnp.Application.TDD
{
    [TestFixture]
    internal class SecurityServiceTest
    {
        ISecurityService _securityService;
        ISecurityRepository _securityRepository;
        IHttpClientFactory _httpClientFactory;
        IValidator<Security> securityValidator;

        [SetUp]
        public void Setup()
        {
            _securityRepository = Substitute.For<ISecurityRepository>();
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            securityValidator = new SecurityValidator();
        }

        [TestCase("AZ1234567891", true)]
        [TestCase("AZ123456", false)]
        [TestCase("", false)]
        [TestCase("AZ1234567891", true)]
        public void GivenStringLenghtPatternThenValidateTheDefaultLengh(string isinCode, bool expectedResult)
        {
            // act
            var isinResult = ConstantProvider.IsValidIsinLenght(isinCode);

            // assert
            Assert.That(isinResult, Is.EqualTo(expectedResult));
        }

        [TestCase(new string[] { "AZ1234567891" }, HttpStatusCode.OK)]
        [TestCase(new string[] { "AZ1234567891", "AZ1234567891" }, HttpStatusCode.OK)]
        public async Task GivenSecurityServiceWithCorrectIsinListThenProcessSuccefully(string[] isinCode, HttpStatusCode expectedResult)
        {
            // Arrange
            var httpClient = new HttpClient(new HttpMessageHandlerMock(isinCode.ToList(), 10, System.Net.HttpStatusCode.OK));
            _httpClientFactory.CreateClient().Returns(httpClient);
            var securityDto = new SecurityDto() { IsinListCodes = isinCode.ToList() };
            _securityService = new SecurityService(_securityRepository, _httpClientFactory, securityValidator);

            // Act
            var securityNotification = await _securityService.PerformSecurityIsinAsync(securityDto, CancellationToken.None);


            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(securityNotification.Count, Is.EqualTo(isinCode.Count()));
                Assert.That(securityNotification.All(x => x.HttpStatusCode == expectedResult), Is.True);
                _securityRepository.Received().AddRangeAsync(Arg.Any<List<Security>>(), Arg.Any<CancellationToken>());
            });
        }
    }
}
