using System.Collections.Generic;

namespace DiagramModel.Components
{
    public class Parent : Component
    {
        static readonly List<string> validChildNames = new();
        public Parent(string name, Component parent) : base(name, parent, validChildNames) { }
        public static string TypeName { get => nameof(Parent).ToLower(); }

        public override Component Clone() => new Parent(Name, Parent);

        public override string GetUniqueTypeName() => TypeName;
    }
}
