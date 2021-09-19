using System.Drawing;

namespace CodeGeneration
{
    /// <summary>
    /// Wrapper for colors of individual code components. 
    /// </summary>
    interface ICodeColor
    {
        /// <summary>
        /// Color of code keywords such as `public`, `class`, `interface` or `int`.
        /// </summary>
        static Color KeywordColor { get; }

        /// <summary>
        /// Color of `throw` keyword.
        /// </summary>
        static Color ThrowKeywordColor { get; }

        /// <summary>
        /// Color of class typename such as `MyClass` or `String`.
        /// </summary>
        static Color ClassTypenameColor { get; }
        
        /// <summary>
        /// Color of method name such as `GetName`.
        /// </summary>
        static Color MethodNameColor { get; }

        /// <summary>
        /// Color of method parameter name such as `myParameter`.
        /// </summary>
        static Color ParameterColor { get; }

        /// <summary>
        /// Color of default code text.
        /// </summary>
        static Color Default { get; }
    }
}
