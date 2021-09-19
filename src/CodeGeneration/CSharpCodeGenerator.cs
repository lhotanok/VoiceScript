using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;

using DiagramModel.Components;

namespace CodeGeneration
{
    /// <summary>
    /// Generates C# code from <see cref="DiagramModel.Components"/>.
    /// </summary>
    public class CSharpCodeGenerator : ICodeGenerator
    {
        static readonly string newLine = Environment.NewLine;
        static readonly string semicolon = ";";
        static readonly int tabSpaces = 4;

        static readonly string defaultTypename = "object";
        static readonly string defaultReturnTypename = "void";

        static readonly List<string> keywordTypenames = new()
        {
            "int", "string", "object", "float", "double", "default", "null", "void"
        };

        readonly Action<string, Color> textCallback;

        public CSharpCodeGenerator(Action<string, Color> writeTextCallback)
        {
            textCallback = writeTextCallback;
        }

        public void GenerateCode(Diagram diagram)
        {
            var classes = diagram.GetClasses();

            foreach (var cls in classes)
            {
                GenerateClassCode(cls, 0);
            }

        }

        public void GenerateClassCode(Class cls, int indentation = 0)
        {
            WriteKeyword("class ");
            WriteClassTypename(cls.Name);
            WriteClassParent(cls);
            WriteNewLine();

            WriteCurlyBracket("{", indentation);

            foreach (var field in cls.GetFields())
            {
                GenerateFieldCode(field, indentation + 1);
            }

            if (cls.GetFields().Count != 0 && cls.GetMethods().Count != 0)
            {
                WriteNewLine();
            }

            foreach (var method in cls.GetMethods())
            {
                GenerateMethodCode(method, indentation + 1);
            }

            WriteCurlyBracket("}", indentation);
            WriteNewLine();
        }

        public void GenerateFieldCode(Field field, int indentation = 1)
        {
            WriteIndentation(indentation);

            WriteVisibility(field);
            WriteWhiteSpaceChar();

            WriteTypename(GetTypename(field.GetFieldType()));
            WriteWhiteSpaceChar();

            WriteDefault(field.Name);
            WriteSemicolon();
            WriteNewLine();
        }

        public void GenerateMethodCode(Method method, int indentation = 1)
        {
            WriteIndentation(indentation);

            WriteVisibility(method);
            WriteWhiteSpaceChar();

            WriteTypename(GetReturnTypename(method.GetReturnType()));
            WriteWhiteSpaceChar();

            textCallback(method.Name, CSharpCodeColor.MethodColor);
            GenerateParametersCode(method);
            WriteNewLine();

            WriteCurlyBracket("{", indentation);
            GenerateExceptionCode("NotImplementedException", indentation + 1);
            WriteCurlyBracket("}", indentation);
        }

        public void GenerateParametersCode(Method method)
        {
            var parameters = method.GetParameters();

            WriteDefault("(");

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                if (i != 0) WriteDefault(", ");
                GenerateParameterCode(parameter);
            }

            WriteDefault(")");
        }

        public void GenerateParameterCode(Parameter parameter, int indentation = 0)
        {
            WriteTypename(GetTypename(parameter.GetParameterType()));
            WriteWhiteSpaceChar();

            textCallback(parameter.Name, CSharpCodeColor.ParameterColor);

            if (!parameter.IsRequired)
            {
                WriteDefault(" = ");
                WriteKeyword("default");
            }
        }

        public void GenerateExceptionCode(string exceptionName, int indentation = 2)
        {
            WriteIndentation(indentation);

            textCallback("throw ", CSharpCodeColor.ThrowKeywordColor);

            WriteKeyword("new ");

            WriteClassTypename(exceptionName);
            WriteDefault("();");
            WriteNewLine();
        }

        static string GetTypename(Component typeComponent) => typeComponent == null ? defaultTypename : typeComponent.Name;
        static string GetReturnTypename(Component typeComponent) => typeComponent == null ? defaultReturnTypename : typeComponent.Name;

        void WriteKeyword(string keyword) => textCallback(keyword, CSharpCodeColor.KeywordColor);

        void WriteClassTypename(string typename) => textCallback(typename, CSharpCodeColor.ClassTypeColor);

        void WriteDefault(string text) => textCallback(text, CSharpCodeColor.Default);

        void WriteTypename(string typename)
        {
            var isArray = TryParseArray(ref typename);

            var lowerTypename = typename.ToLower(); 
            if (lowerTypename == "integer") lowerTypename = "int";

            if (keywordTypenames.Contains(lowerTypename))
            {            
                textCallback(lowerTypename, CSharpCodeColor.KeywordColor);
            }
            else
            {
                WriteClassTypename(typename);
            }

            if (isArray) WriteDefault("[]");
        }

        void WriteClassParent(Class cls)
        {
            var parent = cls.GetInheritanceParent();

            if (parent != null)
            {
                WriteDefault(" : ");
                WriteClassTypename(parent.Name);
            }
        }

        static bool TryParseArray(ref string typename)
        {
            var suffix = "[]";
            if (typename.Contains(suffix))
            {
                typename = typename.Substring(0, typename.Length - suffix.Length);
                return true;
            }

            return false;
        }

        void WriteVisibility(IVisibleComponent component) => WriteKeyword(component.GetVisibility().Name);

        void WriteTab() => WriteWhiteSpaceChar(" ", tabSpaces);

        void WriteNewLine() => WriteWhiteSpaceChar(newLine);

        void WriteSemicolon() => textCallback(semicolon, CSharpCodeColor.Default);

        void WriteCurlyBracket(string curlyBracket, int indentation)
        {
            WriteIndentation(indentation);
            WriteDefault(curlyBracket);
            WriteNewLine();
        }

        void WriteIndentation(int indentation)
        {
            for (int i = 0; i < indentation; i++)
            {
                WriteTab();
            }
        }

        void WriteWhiteSpaceChar(string character = " ", int count = 1)
        {
            var whitespaces = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                whitespaces.Append(character);
            }

            WriteDefault(whitespaces.ToString());
        }
    }
}
