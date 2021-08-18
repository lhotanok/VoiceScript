using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Method : Component
    {
        public Method(string name, Component parent) : base(name, parent)
        {   
            // set default values
            children.Add(new ReturnType(this));
            children.Add(new Visibility(this));
        }

        public Visibility Visibility { get => GetFilteredChildren<Visibility>()[0]; }

        public ReturnType ReturnType { get => GetFilteredChildren<ReturnType>()[0]; }

        public IEnumerable<Parameter> RequiredParameters
        {
            get => GetFilteredParameters(parameter => parameter.Required.Value);
        }

        public IEnumerable<Parameter> OptionalParameters
        {
            get => GetFilteredParameters(parameter => !parameter.Required.Value);
        }

        public override string TypeName { get => GetType().Name; }

        IEnumerable<Parameter> GetFilteredParameters(Func<Parameter, bool> filterCallback)
        {
            var parameters = GetFilteredChildren<Parameter>();
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
