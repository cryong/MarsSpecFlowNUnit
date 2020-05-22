using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MarsFramework.Model
{
    public class TestHierarchyModel<T>
    {
        private readonly List<TestHierarchyModel<T>> _children = new List<TestHierarchyModel<T>>();

        public TestHierarchyModel(T value)
        {
            Value = value;
        }

        public TestHierarchyModel<T> this[int i]
        {
            get { return _children[i]; }
        }

        public TestHierarchyModel<T> Parent { get; private set; }

        public T Value { get; private set; }

        public ReadOnlyCollection<TestHierarchyModel<T>> Children
        {
            get { return _children.AsReadOnly(); }
        }

        public TestHierarchyModel<T> AddChild(T value)
        {
            var node = new TestHierarchyModel<T>(value) { Parent = this };
            _children.Add(node);
            return node;
        }

        public TestHierarchyModel<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TestHierarchyModel<T> node)
        {
            return _children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
            {
                child.Traverse(action);
            }
        }

        public T Search(Func<T, string> keyFunction, string searchKey)
        {
            if (keyFunction(Value) == searchKey)
            {
                return Value;
            }

            foreach (var child in _children)
            {
                T found = child.Search(keyFunction, searchKey);
                if (found != null)
                {
                    return found;
                }
            }

            return default;
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
        }
    }
}
