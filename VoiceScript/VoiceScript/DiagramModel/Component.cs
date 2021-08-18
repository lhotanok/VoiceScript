using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    abstract class Component
    {
        protected readonly List<Component> children;
        readonly List<string> validChildrenTypes;
        static readonly Dictionary<string, Func<string, Component, Component>> componentCtors = new()
        {
            {Class.TypeName, (childName, parent) => new Class(childName, parent) },
            {Field.TypeName, (childName, parent) => new Field(childName, parent) },
            {Method.TypeName, (childName, parent) => new Method(childName, parent) },
            {Parameter.TypeName, (childName, parent) => new Parameter(childName, parent) },
            {Required.TypeName, (childName, parent) => new Required(parent) },
            {VariableType.TypeName, (childName, parent) => new VariableType(childName, parent) },
            {Visibility.TypeName, (childName, parent) => new Visibility(childName, parent) }
        };
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
        public void AddChild(string childType, string childName)
        {
            if (ValidChildrenTypes.Contains(childType) && componentCtors.ContainsKey(childType))
            {
                children.Add(componentCtors[childType](childName, this));
            }
            else
            {
                throw new InvalidOperationException();
            }
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
