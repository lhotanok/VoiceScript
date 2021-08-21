using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    public abstract class Component
    {
        readonly List<string> validChildrenTypes;
        protected List<Component> children;
        public Component(string name, Component parent, List<string> validChildren)
        {
            Parent = parent;
            Name = name;
            children = new List<Component>();
            validChildrenTypes = validChildren;
        }
        public abstract string GetTypeName();

        public Component Parent { get; protected set; }

        public List<Component> Children { get => children; }

        public List<string> ValidChildrenTypes { get => validChildrenTypes; }

        public virtual string Name { get; set; }
        public abstract Component Clone();

        public virtual void AddChild(Component child) => children.Add(child);

        public virtual bool TryDeleteChild(string childType, string childName)
        {
            int childIndex = GetChildIndex(childType, childName);

            if (childIndex != -1)
            {
                children.RemoveAt(childIndex);
            }

            return childIndex != -1;
        }

        public virtual Component FindChild(string childType, string childName)
        {
            int childIndex = GetChildIndex(childType, childName);

            if (childIndex != -1)
            {
                return children[childIndex];
            }

            return null;
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

        protected T GetUniqueChild<T>() where T : Component
        {
            var filteredChildren = GetTypeFilteredChildren<T>();
            if (filteredChildren.Count != 0)
            {
                return filteredChildren[0];
            }
            else
            {
                return null;
            }
        }

        protected void CloneChildrenInto(Component parent)
        {
            foreach (var child in children)
            {
                parent.children.Add(child.Clone());
            }
        }

        int GetChildIndex(string childType, string childName)
        {
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                if (child.GetTypeName() == childType && child.Name == childName)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
