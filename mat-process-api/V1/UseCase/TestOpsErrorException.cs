using System;

namespace mat_process_api.UseCase.V1
{
    public class TestOpsErrorException : Exception
    {
        public TestOpsErrorException() : base("This is a test exception to test our integrations"){}
    }
}
