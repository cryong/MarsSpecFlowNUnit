using System.Collections;
using System.Collections.Generic;
using MarsWebService.Model;

namespace NUnitTest.Context
{
    public class DataSetUpContext
    {
        // just a class to hold list of objects that have been set up using WS 
        // which needs to be removed at the end of the test run in tear down step

        private readonly Queue<IWSObject> _objectsToBeRemoved;
        public DataSetUpContext()
        {
            _objectsToBeRemoved = new Queue<IWSObject>();
        }

        public void Add(IWSObject objectTobeRemoved)
        {
            _objectsToBeRemoved.Enqueue(objectTobeRemoved);
        }

        public IWSObject Pop()
        {
            return _objectsToBeRemoved.Dequeue();
        }

        public IEnumerable GetObejcts()
        {
            while (_objectsToBeRemoved.Count > 0)
            {
                yield return Pop();
            }
        }
    }
}
