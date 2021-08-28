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

            foreach (var cls in classes)
            {
                var joinedNode = graph.AddNode(cls.Name);
                joinedNode.UserData = cls;

                SetUpNodeDesign(joinedNode);
                //graph.AddEdge(rndNodeName, cls.Name).Attr.Color = Color.Transparent;

            }

            return graph;
        }

        void SetUpNodeDesign(Node node)
        {
            node.Attr.FillColor = backgroundColors[rnd.Next(backgroundColors.Length)]; // choose random color from a predefined set
            node.Attr.LabelMargin = 0; // remove space between line separators and box borders
            node.Attr.XRadius = 0; // set sharp edges
            node.LabelText = BuildLabelText(node);
        }

        string BuildLabelText(Node node)
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
