using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    abstract class Component
    {
        protected readonly List<Component> children;
        public Component(string name, Component parent)
        {
            Parent = parent;
            Name = name;
            children = new List<Component>();
        }
        public abstract string TypeName { get; }
        public Component Parent { get; }
        public IEnumerable<Component> Children { get; }
        public virtual string Name { get; protected set; }

        protected List<T> GetFilteredChildren<T>() where T: Component
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
