using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace SpecFlowTest.Utilities
{
    public class TestHelper
    {
        public const string DeleteKey = "DELETE"; // this marks which objects need to be cleaned up as part of after scenario step

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
