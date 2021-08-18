using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Method : Component
    {
        public static List<string> ValidChildTypes = new() { Visibility.TypeName, ReturnType.TypeName };
        public Method(string name, Component parent) : base(name, parent, ValidChildTypes)
        {
            // set default values
            children.Add(new Visibility(this));
            children.Add(new ReturnType(this));
        }
        public static string TypeName { get => nameof(Method).ToLower(); }
        public Visibility Visibility { get => GetTypeFilteredChildren<Visibility>()[0]; }

        public ReturnType GetReturnType() => GetTypeFilteredChildren<ReturnType>()[0];

        public IEnumerable<Parameter> GetRequiredParameters()
            => GetFilteredParameters(parameter => parameter.IsRequired().Value);

        public IEnumerable<Parameter> GetOptionalParameters()
            => GetFilteredParameters(parameter => !parameter.IsRequired().Value);

        public override string GetTypeName() => TypeName;

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
