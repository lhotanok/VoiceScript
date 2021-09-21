using System;
using System.Collections.Generic;

namespace DiagramModel.Components
{
    /// <summary>
    /// Provides interface for creating components
    /// based on their unique typename. To be used with add command.
    /// </summary>
    public class ComponentFactory
    {
        static readonly Dictionary<string, Func<string, Component, Component>> componentCtors = new()
        {
            { Diagram.TypeName, (childName, parent) => new Diagram(childName, null) },
            { Class.TypeName, (childName, parent) => new Class(childName, parent) },
            { Field.TypeName, (childName, parent) => new Field(childName, parent) },
            { Method.TypeName, (childName, parent) => new Method(childName, parent) },
            { Parameter.TypeName, (childName, parent) => new Parameter(childName, parent) },
            { Required.TypeName, (childName, parent) => new Required(childName, parent) },
            { Type.TypeName, (childName, parent) => new Type(childName, parent) },
            { Visibility.TypeName, (childName, parent) => new Visibility(childName, parent) },
            { Parent.TypeName, (childName, parent) => new Parent(childName, parent) }
        };

        /// <summary>
        /// Checks whether component with the given typename can be created.
        /// </summary>
        /// <param name="type">Component's default and unique typename.</param>
        /// <returns>The result of the check.</returns>
        public static bool CanCreateComponent(string type) => componentCtors.ContainsKey(type);

        /// <summary>
        /// Creates new component from the provided typename, name and parent component.
        /// Doesn't do any validation checks as <see cref="CanCreateComponent(string)"/> 
        /// is designed for these. 
        /// </summary>
        /// <param name="type">Component's default and unique typename.</param>
        /// <param name="name">Component's name that should be passed in the constructor.</param>
        /// <param name="parent">Parent component instance.</param>
        /// <returns></returns>
        public static Component CreateComponent(string type, string name, Component parent) => componentCtors[type](name, parent);

    }
}
