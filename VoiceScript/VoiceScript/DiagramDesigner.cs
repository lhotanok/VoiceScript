using System;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

using VoiceScript.DiagramModel.Components;

namespace VoiceScript
{
    class DiagramDesigner
    {
        static readonly Color[] backgroundColors = new Color[]
        {
                Color.Bisque,
                Color.LavenderBlush,
                Color.LightCoral,
                Color.LightGoldenrodYellow,
                Color.LightGreen,
                Color.LightSalmon,
                Color.LightSeaGreen,
                Color.LightSkyBlue,
                Color.LightSteelBlue,
                Color.MistyRose,
                Color.PaleGoldenrod,
                Color.PaleTurquoise,
                Color.PaleVioletRed,
                Color.Plum,
                Color.PowderBlue,
        };

        readonly Random rnd = new((int)DateTime.Now.Ticks);

        public Graph CreateGraphDiagram(IReadOnlyList<Class> classes)
        {
            var graph = new Graph("graph");
            graph.Attr.BackgroundColor = Color.Transparent;
            graph.LayoutAlgorithmSettings.NodeSeparation = 10;

            for (int i = 0; i < classes.Count - 1; i++)
            {
                if (!graph.NodeMap.ContainsKey(classes[i].Name) || !graph.NodeMap.ContainsKey(classes[i + 1].Name))
                {
                    graph.AddEdge(classes[i].Name, classes[i + 1].Name).Attr.Color = Color.Transparent;
                }

                ProcessNode(graph, classes[i]);
            }

            ProcessNode(graph, classes[classes.Count - 1]);

            return graph;
        }

        void ProcessNode(Graph graph, Class nodeClass)
        {
            var node = graph.FindNode(nodeClass.Name);
            node.UserData = nodeClass;
            SetUpNodeDesign(node);
        }

        void SetUpNodeDesign(Node node)
        {
            node.Attr.FillColor = backgroundColors[rnd.Next(backgroundColors.Length)]; // choose random color from a predefined set
            node.Attr.LabelMargin = 0; // remove space between line separators and box borders
            node.Attr.XRadius = 0; // set sharp edges
            node.LabelText = BuildLabelText(node);
        }

        static string BuildLabelText(Node node)
        {
            var classGrid = new ClassDiagramGrid();

            var classObject = (Class)node.UserData;

            classGrid.AddNameCell(classObject.Name);
            classGrid.AddFieldsCell(classObject.GetFields());
            classGrid.AddMethodsCell(classObject.GetMethods());

            return classGrid.BuildGridText();
        }
    }
}
