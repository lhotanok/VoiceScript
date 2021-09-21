using System.Collections.Generic;

namespace DiagramModel.Components
{
    /// <summary>
    /// Represents component in the diagram model. Requires
    /// component name, parent component and valid children component types.
    /// Holds info about children components, allows their filtration based
    /// on provided type and offers interface for their manipulation
    /// (namely for adding and deleting certain children components).
    /// </summary>
    public abstract class Component
    {
        protected readonly List<Component> children;
        readonly ICollection<string> validChildrenTypes;
        string name;
        public Component(string name, Component parent, ICollection<string> validChildren)
        {
            this.name = name;
            Parent = parent;
            children = new List<Component>();
            validChildrenTypes = validChildren;
        }

        /// <summary>
        /// Holds unique name of the component.
        /// Must be 1-word only to preserve correct command parsing.
        /// </summary>
        public abstract string UniqueTypeName { get; }

        /// <summary>
        /// Parent component this component belongs to.
        /// Represents component layers using tree structure
        /// where root component is <see cref="Diagram"/>.
        /// </summary>
        public Component Parent { get; protected set; }

        /// <summary>
        /// Collection of component's children. Each of these
        /// children has this component set as <see cref="Component.Parent"/>
        /// to allow bidirectional component tree traversing.
        /// </summary>
        public IReadOnlyList<Component> Children { get => children; }

        /// <summary>
        /// Returns collection of valid children component typenames.
        /// </summary>
        public ICollection<string> ValidChildrenTypes { get => validChildrenTypes; }

        /// <summary>
        /// Represents component name. Could be interpreted also as component
        /// value based on the context. It is specified explicitly for each component
        /// as component constructor requires name parameter.
        /// </summary>
        public virtual string Name { get => name; set => name = value; }

        /// <summary>
        /// Performs deep copy of the component. It is useful mainly for command execution
        /// phase. During this phase, commands are executed one by one which modifies
        /// component tree. If an error occurrs while executing one of the commands, we 
        /// need to revert back all changes made. For that, we can use the previously created
        /// deep copy of the whole component tree.
        /// </summary>
        /// <returns></returns>
        public abstract Component Clone();

        /// <summary>
        /// Appends new component child. Does not check if the child
        /// has valid type for this component. Check needs to be performed
        /// before this method is called.
        /// </summary>
        /// <param name="child">Validated component child to add.</param>
        public virtual void AddChild(Component child) => children.Add(child);

        /// <summary>
        /// Tries to delete child based on its typename and child component name.
        /// </summary>
        /// <param name="childType">For valid values see <see cref="Component.UniqueTypeName"/></param>
        /// <param name="childName">Corresponds to the <see cref="Component.Name"/></param>
        /// <returns>Result of the delete operation.</returns>
        public virtual bool TryDeleteChild(string childType, string childName)
        {
            int childIndex = GetChildIndex(childType, childName);

            if (childIndex != -1)
            {
                children.RemoveAt(childIndex);
            }

            return childIndex != -1;
        }

        /// <summary>
        /// Searches for child component based on its typename and child component name.
        /// </summary>
        /// <param name="childType">For valid values see <see cref="Component.UniqueTypeName"/></param>
        /// <param name="childName">Corresponds to the <see cref="Component.Name"/></param>
        /// <returns>Searched child component or null if no matching component was found.</returns>
        public virtual Component FindChild(string childType, string childName)
        {
            int childIndex = GetChildIndex(childType, childName);

            if (childIndex != -1)
            {
                return children[childIndex];
            }

            return null;
        }

        /// <summary>
        /// Filters children components that match the given type.
        /// </summary>
        /// <typeparam name="T">Component type to filter.</typeparam>
        /// <returns>Collection of child components of the provided type.</returns>
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

        /// <summary>
        /// Gets child component matching the provided type.
        /// Relies on child being unique for this component, 
        /// such as <see cref="Visibility"/> or <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">Child component type.</typeparam>
        /// <returns>Child component or null if no component of the 
        /// matching type exists.</returns>
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

        /// <summary>
        /// Inserts deep copy of this component's children
        /// into parent component which is provided as a parameter. 
        /// </summary>
        /// <param name="parent"></param>
        protected void CloneChildrenInto(Component parent)
        {
            foreach (var child in children)
            {
                parent.children.Add(child.Clone());
            }
        }

        int GetChildIndex(string childType, string childName)
        {
            var lowerChildName = childName.ToLower();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                if (child.UniqueTypeName == childType && child.Name.ToLower() == lowerChildName)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
