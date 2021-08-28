using System;
using System.Collections.Generic;
using System.Text;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript
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

        public void AddFieldsCell(IReadOnlyList<Field> fields,
            bool showVisibility = true, bool showType = true)
        {
            var fieldsCell = new ClassDiagramCell();

            foreach (var field in fields)
            {
                fieldsCell.AddLine(BuildFieldLine(field, showVisibility, showType));
            }

            cells.Add(fieldsCell);
        }

        public void AddMethodsCell(IReadOnlyList<Method> methods,
            bool showVisibility = true, bool showReturnType = true, bool showParameterTypes = true)
        {
            var methodsCell = new ClassDiagramCell();

            foreach (var method in methods)
            {
                methodsCell.AddLine(BuildMethodLine(method, showVisibility, showReturnType, showParameterTypes));
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

        string BuildFieldLine(Field field, bool showVisibility, bool showType)
        {
            var fieldLine = new StringBuilder();

            if (showVisibility)
            {
                var symbol = GetVisibilitySymbol(field.GetVisibility().Name);
                if (symbol != string.Empty) fieldLine.Append(symbol + ' ');
            }

            AddComponentName(field, fieldLine);

            if (showType) fieldLine.Append($" : {field.GetFieldType().Name}");

            return fieldLine.ToString();
        }

        string BuildMethodLine(Method method, bool showVisibility, bool showReturnType, bool showParameterTypes)
        {
            var methodLine = new StringBuilder();

            if (showVisibility)
            {
                var symbol = GetVisibilitySymbol(method.GetVisibility().Name);
                if (symbol != string.Empty) methodLine.Append(symbol + ' ');
            }

            AddComponentName(method, methodLine);

            methodLine.Append(BuildParametersText(method.GetParameters(), showParameterTypes));

            if (showReturnType) methodLine.Append($" : {method.GetReturnType().Name}");

            return methodLine.ToString();
        }

        string BuildParametersText(IReadOnlyList<Parameter> parameters, bool showParameterTypes)
        {
            var line = new StringBuilder();
            line.Append('(');

            for (int i = 0; i < parameters.Count - 1; i++)
            {
                var parameter = parameters[i];
                AddComponentName(parameter, line);
                if (showParameterTypes) line.Append($" : {parameter.GetParameterType().Name}");
                line.Append(", ");
            }

            if (parameters.Count != 0)
            {
                var parameter = parameters[^1];
                AddComponentName(parameter, line);
                if (showParameterTypes) line.Append($" : {parameter.GetParameterType().Name}");
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
