using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Method : Component
    {
        readonly static List<string> validChildTypes = new() { Visibility.TypeName, ReturnType.TypeName, Parameter.TypeName };
        public Method(string name, Component parent) : base(name, parent, validChildTypes) { }
        public static string TypeName { get => nameof(Method).ToLower(); }

        /// <summary>
        /// If visibility is not defined return default visibility.
        /// </summary>
        /// <returns>Defined value of visibility or default.</returns>
        public Visibility GetVisibility() => GetUniqueChild<Visibility>() ?? new Visibility(this);

        /// <summary>
        /// If return type is not defined return default return type.
        /// </summary>
        /// <returns>Defined value of return type or default.</returns>
        public ReturnType GetReturnType() => GetUniqueChild<ReturnType>() ?? new ReturnType(this);

        public IEnumerable<Parameter> GetRequiredParameters()
            => GetFilteredParameters(parameter => parameter.IsRequired().Value);

        public IEnumerable<Parameter> GetOptionalParameters()
            => GetFilteredParameters(parameter => !parameter.IsRequired().Value);

        public override string GetTypeName() => TypeName;

        public override void AddChild(Component child)
        {
            if (child is Type) child = new ReturnType(child.Name, child.Parent);

            base.AddChild(child);
        }
        IEnumerable<Parameter> GetFilteredParameters(Func<Parameter, bool> filterCallback)
        {
            var parameters = GetTypeFilteredChildren<Parameter>();
            var filtered = new List<Parameter>();
       
            foreach (var parameter in parameters)
            {
                if (filterCallback(parameter))
                {
                    filtered.Add(parameter);
                }
            }

            return filtered;
        }
    }
}
