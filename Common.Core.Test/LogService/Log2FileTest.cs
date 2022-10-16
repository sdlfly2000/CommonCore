using Common.Core.LogService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Common.Core.Test.LogService
{
    [TestClass]
    public class Log2FileTest
    {
        private Mock<IConfiguration> _configurationMock;
        private Func<string, Exception?, string> _formatter;

        private ILog2File _log2File;

        [TestInitialize]
        public void TestInit()
        {
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock
                .Setup(configuration => configuration["LogFilePath"])
                .Returns("test.log");

            _formatter = (content, ex) =>
            {
                return content;
            };

            _log2File = new Log2File(_configurationMock.Object);
        }

        [TestMethod, TestCategory("Integration")]
        public void Given_LogContent_When_Log_Then_contentInFile()
        {
            // Arrange
            var logContent = "Test is a test";

            // Action
            _log2File.Log(LogLevel.Information, (EventId)101, logContent, null, _formatter);

            _log2File.Dispose();
            // Assert

        }

        [TestMethod, TestCategory("Integration")]
        public void Given_LogContent_When_LogInformation_Then_contentInFile()
        {
            // Arrange
            var logContent = "LogInformation Test";

            // Action
            _log2File.LogInformation(logContent);

            _log2File.Dispose();
            // Assert
        }
    }
}