using System;
using System.Collections.Generic;
using System.Text;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramDesign
{
    class ClassDiagramGrid
    {
        readonly List<ClassDiagramCell> cells = new();
        readonly Dictionary<string, char> visibilitySymbols = new()
        {
            { "public", '+' },
            { "private", '-' },
            { "protected", '#' },
            { "internal", '~' },
        };
        readonly char cellSeparator = '—';

        public string BuildGridText()
        {
            var gridText = new StringBuilder();
            var maxLineLength = (GetMaxLineLength() / 4) * 3;

            for (int i = 0; i < cells.Count - 1; i++)
            {
                var cell = cells[i];
                gridText.Append(cell.BuildCellText());
                gridText.Append(BuildCellSeparator(maxLineLength));
            }
            if (cells.Count != 0)
            {
                gridText.Append(cells[^1].BuildCellText());
                gridText.Append(ClassDiagramCell.MarginVertical);
            }

            return gridText.ToString();
        }

        public void AddNameCell(string name)
        {
            var cell = new ClassDiagramCell();
            cell.AddLine(name);
            cells.Add(cell);
        }

        public void AddFieldsCell(IReadOnlyList<Field> fields)
        {
            var fieldsCell = new ClassDiagramCell();

            foreach (var field in fields)
            {
                fieldsCell.AddLine(BuildFieldLine(field));
            }

            cells.Add(fieldsCell);
        }

        public void AddMethodsCell(IReadOnlyList<Method> methods)
        {
            var methodsCell = new ClassDiagramCell();

            foreach (var method in methods)
            {
                methodsCell.AddLine(BuildMethodLine(method));
            }

            cells.Add(methodsCell);
        }

        int GetMaxLineLength()
        {
            var maxLineLength = 0;

            foreach (var cell in cells)
            {
                var cellWidth = cell.GetMaxLineLength();
                maxLineLength = cellWidth > maxLineLength ? cellWidth : maxLineLength;
            }

            return maxLineLength;
        }

        string BuildCellSeparator(int length)
        {
            var separator = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                separator.Append(cellSeparator);
            }

            return separator.ToString();
        }

        string BuildFieldLine(Field field)
        {
            var fieldLine = new StringBuilder();

            var symbol = GetVisibilitySymbol(field.GetVisibility().Name);
            if (symbol != string.Empty) fieldLine.Append(symbol + ' ');

            AddComponentName(field, fieldLine);

            var fieldType = field.GetFieldType();
            if (fieldType != null) fieldLine.Append($" : {fieldType.Name}");

            return fieldLine.ToString();
        }

        string BuildMethodLine(Method method)
        {
            var methodLine = new StringBuilder();

            var symbol = GetVisibilitySymbol(method.GetVisibility().Name);
            if (symbol != string.Empty) methodLine.Append(symbol + ' ');

            AddComponentName(method, methodLine);

            methodLine.Append(BuildParametersText(method.GetParameters()));

            var returnType = method.GetReturnType();
            if (returnType != null) methodLine.Append($" : {returnType.Name}");

            return methodLine.ToString();
        }

        string BuildParametersText(IReadOnlyList<Parameter> parameters)
        {
            var line = new StringBuilder();
            line.Append('(');

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                if (i != 0) line.Append(", ");

                AddComponentName(parameter, line);

                var parameterType = parameter.GetParameterType();
                if (parameterType != null) line.Append($" : {parameterType.Name}");
            }

            line.Append(')');

            return line.ToString();
        }

        string GetVisibilitySymbol(string visibilityName)
        {
            var visibility = visibilityName.ToLower();

            if (visibilitySymbols.ContainsKey(visibility))
            {
                return visibilitySymbols[visibility].ToString();
            }
            return string.Empty;
        }

        static void AddComponentName(Component component, StringBuilder text) => text.Append(component.Name);
    }
}
