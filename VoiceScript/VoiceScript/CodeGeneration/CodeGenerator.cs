using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript.CodeGeneration
{
    class CodeGenerator
    {
        static readonly string newLine = Environment.NewLine;
        static readonly int tabSpaces = 4;

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
                GenerateClassCode(cls);
            }

        }

        public void GenerateClassCode(Class cls)
        {
            WriteText("class", CodeColor.KeywordColor);
            WriteWhiteSpaceChar();
            WriteText(cls.Name, CodeColor.TypeColor);
            WriteWhiteSpaceChar(newLine);

            WriteText("{", CodeColor.Default);
            WriteWhiteSpaceChar(newLine);

            // fields, methods

            WriteText("}", CodeColor.Default);
            WriteWhiteSpaceChar(newLine);

        }

        void WriteText(string text, Color color)
        {
            textBox.SelectionColor = color;
            textBox.SelectedText = text;
        }

        void WriteTab() => WriteWhiteSpaceChar(" ", tabSpaces);

        void WriteWhiteSpaceChar(string character = " ", int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                textBox.AppendText(character);
            }
        }
    }

    class Person
    {
        public int Age;
        protected string name;

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public void SetName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
