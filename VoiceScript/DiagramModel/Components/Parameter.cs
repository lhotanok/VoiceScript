﻿using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Components
{
    public class Parameter : Component
    {
        readonly static List<string> validChildTypes = new() { ParameterType.TypeName, Required.TypeName };
        public Parameter(string name, Component parent) : base(name, parent, validChildTypes) { }
        public static string TypeName { get => nameof(Parameter).ToLower(); }

        /// <summary>
        /// If parameter type is not defined return default required value.
        /// </summary>
        /// <returns>Defined value of parameter type or default.</returns>
        public ParameterType GetParameterType() => GetUniqueChild<ParameterType>() ?? new ParameterType(this);

        /// <summary>
        /// If required value is not defined return default required value.
        /// </summary>
        /// <returns>Defined value of required level or default.</returns>
        public Required GetRequireInfo() => GetUniqueChild<Required>() ?? new Required(this);

        public bool IsRequired { get => GetRequireInfo().Value; }
        public override string GetUniqueTypeName() => TypeName;

        public override void AddChild(Component child)
        {
            if (child is Type) child = new ParameterType(child.Name, child.Parent);

            base.AddChild(child);
        }
        public override Component Clone()
        {
            var clone = new Parameter(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
