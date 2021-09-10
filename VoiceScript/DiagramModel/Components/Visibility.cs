using System;
using System.Collections.Generic;
using System.Text;

namespace DiagramModel.Components
{
    public class Visibility : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static List<string> validNames = new() { "public", "private", "protected", "internal" };
        readonly static string defaultName = "public";
        public Visibility(Component parent) : this(defaultName, parent) { }
        public Visibility(string name, Component parent) : base(name.ToLower(), parent, validChildTypes)
        {
            CheckValidName(Name);
        }
        public static string TypeName { get => nameof(Visibility).ToLower(); }

        public static string DefaultName { get => defaultName; }

        public static IEnumerable<string> ValidNames { get => validNames; }

        public override string GetUniqueTypeName() => TypeName;

        public override Component Clone()
        {
            var clone = new Visibility(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }

        public override string Name
        {
            get => base.Name;
            set
            {
                var name = value.ToLower();
                CheckValidName(name);
                base.Name = name;
            }
        }

        static string GetValidNamesJoined(string separator)
        {
            var validVisibilityValues = new StringBuilder();

            for (int i = 0; i < validNames.Count; i++)
            {
                if (i != 0) validVisibilityValues.Append(separator);
                validVisibilityValues.Append(validNames[i]);
            }

            return validVisibilityValues.ToString();
        }

        static void CheckValidName(string name)
        {
            if (!validNames.Contains(name))
            {
                var validNamesJoined = GetValidNamesJoined(", ");
                throw new InvalidOperationException($"Invalid name of visibility level provided: {name}. Valid values are: {validNamesJoined}.");
            }
        }
    }
}
