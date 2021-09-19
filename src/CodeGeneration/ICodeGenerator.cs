using DiagramModel.Components;

namespace CodeGeneration
{
    /// <summary>
    /// Handles code generation from <see cref="DiagramModel.Components"/>.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Generates full code from <see cref="Diagram"/>.
        /// </summary>
        /// <param name="diagram">Component representing diagram model.</param>
        void GenerateCode(Diagram diagram);

        /// <summary>
        /// Generates code from <see cref="Class"/>.
        /// </summary>
        /// <param name="cls">Component representing class from diagram model.</param>
        /// <param name="indentation">Optional block indentation. 
        /// Corresponds to the number of tabs. Set to 0 by default.</param>
        void GenerateClassCode(Class cls, int indentation = 0);

        /// <summary>
        /// Generates code from <see cref="Field"/>.
        /// </summary>
        /// <param name="field">Component representing class field from diagram model.</param>
        /// <param name="indentation">Optional block indentation. Corresponds to the number of tabs.
        /// Set to 1 by default (with respect to class indentation).</param>
        void GenerateFieldCode(Field field, int indentation = 1);

        /// <summary>
        /// Generates code from <see cref="Method"/>.
        /// </summary>
        /// <param name="method">Component representing class method from diagram model.</param>
        /// <param name="indentation">Optional block indentation. Corresponds to the number of tabs.
        /// Set to 1 by default (with respect to class indentation).</param>
        void GenerateMethodCode(Method method, int indentation = 1);

        /// <summary>
        /// Generates code from all <see cref="Method.GetParameters"/>.
        /// </summary>
        /// <param name="method">Component representing class method from diagram model.</param>
        void GenerateParametersCode(Method method);

        /// <summary>
        /// Generates code from <see cref="Parameter"/>.
        /// </summary>
        /// <param name="parameter">Component representing method parameter from diagram model.</param>
        /// <param name="indentation">Optional block indentation. Corresponds to the number of tabs.
        /// Set to 0 by default (with respect to method code design).</param>
        void GenerateParameterCode(Parameter parameter, int indentation = 0);

        /// <summary>
        /// Generates code for exception with the given name.
        /// </summary>
        /// <param name="exceptionName">Name of the exception</param>
        /// <param name="indentation">Optional block indentation. Corresponds to the number of tabs.
        /// Set to 2 by default (with respect to class and method indentation).</param>
        void GenerateExceptionCode(string exceptionName, int indentation = 2);
    }
}
