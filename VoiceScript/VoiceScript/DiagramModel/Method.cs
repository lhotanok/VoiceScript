using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Method : IMethod
    {
        List<IParameter> parameters;

        public Method(string name)
        {
            Name = name;
            ContainsComponents = true;
            ReturnType = new ReturnType();
            Visibility = new Visibility();
        }

        public IVisibility Visibility { get; private set; }

        public IType ReturnType { get; private set; }

        public IEnumerable<IParameter> RequiredParameters
        {
            get => GetFilteredParameters(parameter => parameter.Required);
        }

        public IEnumerable<IParameter> OptionalParameters
        {
            get => GetFilteredParameters(parameter => !parameter.Required);
        }

        public string Name { get; private set; }

        public bool ContainsComponents { get; }

        IEnumerable<IParameter> GetFilteredParameters(Func<IParameter, bool> filterCallback)
        {
            var filtered = new List<IParameter>();

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
