using Reqnroll;

namespace Tests.SwagLabs.ReqnrollTests.Hooks
{
    internal class TestHooks
    {
        [BeforeScenario]
        public static void BeforeScenario()
        {
            Console.WriteLine("Before Scenario");
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Console.WriteLine("After Scenario");
        }
    }
}
