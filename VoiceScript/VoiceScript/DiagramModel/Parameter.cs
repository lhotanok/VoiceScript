﻿using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Parameter : Component
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
        public Required IsRequired() => GetUniqueChild<Required>() ?? new Required(this);

        public override string GetTypeName() => TypeName;
    }
}
