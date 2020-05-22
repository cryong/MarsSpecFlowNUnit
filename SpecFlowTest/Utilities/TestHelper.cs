using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SpecFlowTest.Utilities
{
    public class TestHelper
    {
        public const string DeleteKey = "DELETE"; // this marks which objects need to be cleaned up as part of after scenario step

        public static void DoHandleException(Exception e, string stepTitle)
        {
            if (e is AssertionException) // just re-throw AssertionException
            {
                throw e;
            }
            Assert.Fail($"Error has ocucrred while executing step : {stepTitle}\nMessage : {e.Message}\nStackTrace: {e.StackTrace}");
        }

        public static List<object> GetListOfObjectsToBeRemoved(ScenarioContext context)
        {
            if (!context.TryGetValue<List<object>>(DeleteKey, out var objectsToBeDeleted))
            {
                objectsToBeDeleted = new List<object>();
                context.Set(objectsToBeDeleted, DeleteKey);
            }
            return objectsToBeDeleted;
        }

    }
}
