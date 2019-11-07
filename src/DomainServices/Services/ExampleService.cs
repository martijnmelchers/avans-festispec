using Festispec.DomainServices.Interfaces;
using System;

namespace Festispec.DomainServices.Services
{
    public class ExampleService : IExampleService
    {
        public bool ReturnTrue()
        {
            return true;
        }

        public bool ReturnFalse()
        {
            return false;
        }
        public string ReturnString()
        {
            return $"Test Command {new Random().Next(1,1000)}";
        }
    }
}
