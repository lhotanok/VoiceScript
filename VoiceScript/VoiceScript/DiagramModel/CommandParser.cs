﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceScript.DiagramModel
{
    class CommandParser
    {
        readonly List<string> validKeywords;
        readonly string delimiterCommand;
        readonly string[] parsedWords;

        int parsedOffset;

        static readonly Dictionary<string, Func<string, string, Command>> commandCtors = new()
        {
            { "add", (targetType, targetValue) => new AddCommand(targetType, targetValue) },
            { "edit", (targetType, targetValue) => new EditCommand(targetType, targetValue) },
            { "delete", (targetType, targetValue) => new DeleteCommand(targetType, targetValue) }
        };

        public CommandParser(string inputText)
        {
            parsedWords = inputText.Split(' ');

            delimiterCommand = "escape";
            validKeywords = new List<string>() { "add", "change", "exclude", delimiterCommand};
        }

        public IEnumerable<Command> GetParsedCommands()
        {
            var parsedCommands = new List<Command>();
            var command = GetNextCommand();

            while (command != null)
            {
                parsedCommands.Add(command);
                command = GetNextCommand();
            }

            return parsedCommands;
        }

        Command GetNextCommand()
        {
            var commandName = GetCommandName();
            parsedOffset++;

            if (!IsKeyword(commandName)) return null;

            var targetType = GetNextWord();
            parsedOffset++;

            var targetName = GetTargetName();

            if (targetType == string.Empty || targetName == string.Empty || !commandCtors.ContainsKey(commandName))
            {
                throw new InvalidOperationException("Invalid command.");
            }

            return commandCtors[commandName](targetType, targetName);
        }

        string GetCommandName()
        {
            var word = GetNextWord();

            while (word != string.Empty && !IsKeyword(word))
            {
                parsedOffset++;
                word = GetNextWord();
            }

            return word.ToLower();
        }

        string GetTargetName()
        {
            var nameParts = new List<string>();
            var word = GetNextWord();
            var delimiterSet = false;

            while (word != string.Empty)
            {
                if (IsKeyword(word))
                {
                    if (IsDelimiter(word)) delimiterSet = true;
                    else if (delimiterSet) delimiterSet = false;
                    else break;
                }

                nameParts.Add(word);
                parsedOffset++;
                word = GetNextWord();
            }

            return ConvertToPascalCase(nameParts);
        }

        string GetNextWord() => parsedOffset < parsedWords.Length ? parsedWords[parsedOffset] : string.Empty;

        bool IsKeyword(string word) => validKeywords.Contains(word.ToLower());

        bool IsDelimiter(string word) => word.ToLower() == delimiterCommand;

        static string ConvertToPascalCase(IEnumerable<string> words)
        {
            var pascalCaseWord = new StringBuilder();

            foreach (var word in words)
            {
                var lowerCaseWord = word.ToLower();
                var firstLetter = lowerCaseWord[0].ToString().ToUpper();
                pascalCaseWord.Append(firstLetter + lowerCaseWord[1..]);
            }

            return pascalCaseWord.ToString();
        }
    }
}
