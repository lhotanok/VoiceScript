using System.Collections.Generic;

namespace DiagramModel.Components
{
    public class Class : Component
    {
        // parent value is needed for proper assignment of parent that class inherits from (using standard commands)
        readonly static List<string> validChildTypes = new() { Field.TypeName, Method.TypeName, nameof(parent) };

        Parent parent;
        public Class(string name, Component parent) : base(name, parent, validChildTypes ) { }

        public static string TypeName { get => nameof(Class).ToLower(); }

        public IReadOnlyList<Field> GetFields() => GetTypeFilteredChildren<Field>();

        public IReadOnlyList<Method> GetMethods() => GetTypeFilteredChildren<Method>();

        public override string GetUniqueTypeName() => TypeName;
        public override Component Clone()
        {
            var clone = new Class(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }

        public Parent GetInheritanceParent() => parent;

        public override void AddChild(Component child)
        {
            if (child is Parent p)
            {
                // only 1 parent a class inherits from is allowed
                // parent should not be included in children components semantically
                parent = p;
            }
            else
            {
                base.AddChild(child);
            }
        }
    }
}
