using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript.CodeGeneration
{
    class CodeGenerator
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

        readonly Diagram diagram;
        readonly RichTextBox textBox;

        public CodeGenerator(Diagram diagramModel, RichTextBox richTextBox)
        {
            diagram = diagramModel;
            textBox = richTextBox;
        }

        public void GenerateCode()
        {
            textBox.Clear();

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

            WriteText(method.Name, CodeColor.MethodColor);
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

        public void GenerateParameterCode(Parameter parameter)
        {
            WriteTypename(GetTypename(parameter.GetParameterType()));
            WriteWhiteSpaceChar();

            WriteText(parameter.Name, CodeColor.ParameterColor);

            if (!parameter.IsRequired)
            {
                WriteDefault(" = ");
                WriteKeyword("default");
            }
        }

        public void GenerateExceptionCode(string exceptionName, int indentation = 2)
        {
            WriteIndentation(indentation);

            WriteText("throw ", CodeColor.ThrowKeywordColor);

            WriteKeyword("new ");

            WriteClassTypename(exceptionName);
            WriteDefault("();");
            WriteNewLine();
        }

        static string GetTypename(Component typeComponent) => typeComponent == null ? defaultTypename : typeComponent.Name;
        static string GetReturnTypename(Component typeComponent) => typeComponent == null ? defaultReturnTypename : typeComponent.Name;

        void WriteText(string text, Color color)
        {
            textBox.SelectionColor = color;
            textBox.SelectedText = text;
        }

        void WriteKeyword(string keyword) => WriteText(keyword, CodeColor.KeywordColor);

        void WriteClassTypename(string typename) => WriteText(typename, CodeColor.ClassTypeColor);

        void WriteDefault(string text) => WriteText(text, CodeColor.Default);

        void WriteTypename(string typename)
        {
            var lowerTypename = typename.ToLower();
            if (lowerTypename == "integer") lowerTypename = "int";

            if (keywordTypenames.Contains(lowerTypename))
            {
                WriteText(lowerTypename, CodeColor.KeywordColor);
            }
            else
            {
                WriteClassTypename(typename);
            }
        }

        void WriteVisibility(IVisibleComponent component) => WriteKeyword(component.GetVisibility().Name);

        void WriteTab() => WriteWhiteSpaceChar(" ", tabSpaces);

        void WriteNewLine() => WriteWhiteSpaceChar(newLine);

        void WriteSemicolon() => WriteText(semicolon, CodeColor.Default);

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
            for (int i = 0; i < count; i++)
            {
                textBox.AppendText(character);
            }
        }
    }
}
