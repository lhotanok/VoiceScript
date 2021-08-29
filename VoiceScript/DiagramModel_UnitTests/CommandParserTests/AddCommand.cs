using System;
using System.Collections.Generic;
using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandParserTests
{
    [TestFixture("add class person")]
    [TestFixture("Add class person")]
    [TestFixture("ADD Class Person")]
    [TestFixture("unrecognized input add class person")]
    [TestFixture("add class escape person")]
    public class SingleAddCommand
    {
        readonly CommandParser parser;
        readonly string input;

        public SingleAddCommand(string inputText)
        {
            parser = new CommandParser();
            input = inputText;
        }

        [Test]
        public void ParseClassAditionCommand_ParseExactlyOneCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(1, parsedCommands.Count);
        }

        [Test]
        public void ParseClassAditionCommand_CheckIfParsedCommandIsAddCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.IsTrue(parsedCommands[0] is AddCommand);
        }

        [Test]
        public void ParseClassAditionCommand_CommandHasCorrectContent()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            foreach (var command in parsedCommands)
            {
                Assert.AreEqual("class", command.TargetType);
                Assert.AreEqual("Person", command.TargetValue);
            }
        }
    }

    [TestFixture("add class person add field name add method get name")]
    [TestFixture("unrecognized input add class person add field name add method get name")]
    public class MultipleAddCommands
    {
        readonly CommandParser parser;
        readonly string input;

        public MultipleAddCommands(string inputText)
        {
            parser = new CommandParser();
            input = inputText;
        }

        [Test]
        public void ParseAditionCommands_ParseExactlyThreeCommands()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(3, parsedCommands.Count);
        }

        [Test]
        public void ParseAditionCommands_CheckIfEachParsedCommandIsAddCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            foreach (var command in parsedCommands)
            {
                Assert.IsTrue(command is AddCommand);
            }
        }

        public void ParseAditionCommands_EachCommandHasCorrectContent()
        {
            var commandContent = new Dictionary<int, Tuple<string, string>>()
            {
                {0, new Tuple<string, string>("class", "person") },
                {1, new Tuple<string, string>("field", "name") },
                {2, new Tuple<string, string>("method", "GetName") }
            };

            var parsedCommands = parser.GetParsedCommands(input);

            for (int i = 0; i < parsedCommands.Count; i++)
            {
                Assert.AreEqual(commandContent[i].Item1, parsedCommands[i].TargetType);
                Assert.AreEqual(commandContent[i].Item2, parsedCommands[i].TargetValue);
            }
        }
    }

    [TestFixture("add class escape add result", "AddResult")]
    [TestFixture("add class Escape add result", "AddResult")]
    [TestFixture("add class escape edit result", "EditResult")]
    [TestFixture("add class escape delete result", "DeleteResult")]
    [TestFixture("add class escape escape result", "EscapeResult")]
    public class SingleAddCommandWithCommandNameUsedInTargetName
    {
        readonly CommandParser parser;
        readonly string input;
        readonly string expectedTargetValue;

        public SingleAddCommandWithCommandNameUsedInTargetName(string inputText, string targetValue)
        {
            parser = new CommandParser();
            input = inputText;
            expectedTargetValue = targetValue;
        }

        [Test]
        public void ParseAditionCommand_ParseExactlyOneCommandWithEscapedContent()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(1, parsedCommands.Count);
        }

        [Test]
        public void ParseAditionCommand_CheckIfParsedCommandWithEscapedContentIsAddCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.IsTrue(parsedCommands[0] is AddCommand);
        }

        [Test]
        public void ParseAditionCommand_CommandHasCorrectEscapedContent()
        {
            var parsedCommands = parser.GetParsedCommands(input);
            var command = parsedCommands[0];

            Assert.AreEqual("class", command.TargetType);
            Assert.AreEqual(expectedTargetValue, command.TargetValue);
        }
    }
}