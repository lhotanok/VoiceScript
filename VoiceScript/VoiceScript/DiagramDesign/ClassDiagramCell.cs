using System;
using System.Text;
using System.Collections.Generic;

namespace VoiceScript.DiagramDesign
{
    class ClassDiagramCell
    {
        static readonly string marginHorizontal = "     ";
        static readonly string marginVertical = Environment.NewLine;
        readonly List<string> lines = new();
        public static string MarginHorizontal { get => marginHorizontal; }
        public static string MarginVertical { get => marginVertical; }
        public string BuildCellText()
        {
            var cellText = new StringBuilder();

            foreach (var line in lines)
            {
                cellText.Append(marginVertical);
                cellText.Append(line);
            }

            cellText.Append(marginVertical);

            return cellText.ToString();
        }

        public void AddLine(string text)
        {
            lines.Add(marginHorizontal + text + marginHorizontal);
        }

        public int GetMaxLineLength()
        {
            var maxLength = 0;

            foreach (var line in lines)
            {
                maxLength = line.Length > maxLength ? line.Length : maxLength;
            }

            return maxLength;
        }
    }
}
