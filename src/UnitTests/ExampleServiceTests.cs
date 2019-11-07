
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Xunit;

namespace Festispec.UnitTests
{
    public class ExampleServiceTests
    {
        private readonly IExampleService _exampleService;

        public ExampleServiceTests()
        {
            _exampleService = new ExampleService();
        }

        [Fact]
        public void ReturnTrueReturnsTrue()
        {
            Assert.True(_exampleService.ReturnTrue());
        }

        [Fact]
        public void ReturnFalseReturnsFalse()
        {
            Assert.False(_exampleService.ReturnFalse());
        }

        [Fact]
        public void ReturnStringReturnsString()
        {
            Assert.IsType<string>(_exampleService.ReturnString());
        }
    }
}
