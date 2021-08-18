using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    abstract class Component
    {
        protected readonly List<Component> children;
        readonly List<string> validChildrenTypes;
        public Component(string name, Component parent, List<string> validChildren)
        {
            Parent = parent;
            Name = name;
            children = new List<Component>();
            validChildrenTypes = validChildren;
        }
        public abstract string GetTypeName();
        public Component Parent { get; }
        public IEnumerable<Component> Children { get => children; }
        public List<string> ValidChildrenTypes { get => validChildrenTypes; }
        public virtual string Name { get; protected set; }
        public void AddChild(Component child) => children.Add(child);
        public bool TryDeleteChild(string childType, string childName)
        {
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                if (child.GetTypeName() == childType && child.Name == childName)
                {
                    children.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        protected List<T> GetTypeFilteredChildren<T>() where T: Component
        {
            var filteredChildren = new List<T>();

            foreach (var child in children)
            {
                if (child is T filteredChild)
                {
                    filteredChildren.Add(filteredChild);
                }
            }

            return filteredChildren;
        }
    }
}
