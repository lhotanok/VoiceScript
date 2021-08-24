using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Components
{
    public class Field : Component
    {
        readonly static List<string> validChildTypes = new() { Visibility.TypeName, FieldType.TypeName };

        public Field(string name, Component parent) : base(name, parent, validChildTypes) { }

        public static string TypeName { get => nameof(Field).ToLower(); }

        /// <summary>
        /// If visibility is not defined return default visibility.
        /// </summary>
        /// <returns>Defined value of visibility or default.</returns>
        public Visibility GetVisibility() => GetUniqueChild<Visibility>() ?? new Visibility(this);

        /// <summary>
        /// If field type is not defined return default field type.
        /// </summary>
        /// <returns>Defined value of field type or default.</returns>
        public FieldType GetFieldType() => GetUniqueChild<FieldType>() ?? new FieldType(this);
        
        public override string GetUniqueTypeName() => TypeName;

        public override void AddChild(Component child)
        {
            if (child is Type) child = new FieldType(child.Name, child.Parent);

            base.AddChild(child);
        }

        public override Component Clone()
        {
            var clone = new Field(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
