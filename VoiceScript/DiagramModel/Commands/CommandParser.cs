using System;
using System.Text;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandParser
    {
        readonly static List<string> validKeywords = new() {
            AddCommand.DefaultFormat,
            EditCommand.DefaultFormat,
            DeleteCommand.DefaultFormat,
            DelimiterWrapper.CommandDefaultFormat
        };

        string[] parsedWords;
        int parsedOffset;

        public IList<Command> GetParsedCommands(string inputText)
        {
            parsedWords = inputText.Split(' ');
            parsedOffset = 0;

            var parsedCommands = new List<Command>();
            var command = GetNextCommand();

            while (command != null)
            {
                parsedCommands.Add(command);
                try
                {
                    command = GetNextCommand();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message + $" Successfully parsed commands: {parsedCommands.Count}.");
                }
                
            }

            return parsedCommands;
        }

        public static string ParseCamelCase(string pascalCaseWord)
        {
            if (pascalCaseWord.Length == 0) return pascalCaseWord;

            var firstLetter = pascalCaseWord[0].ToString().ToLower();
            var remainingLetters = pascalCaseWord[1..];

            return firstLetter + remainingLetters;
        }

        Command GetNextCommand()
        {
            var commandName = GetCommandName();
            parsedOffset++;

            if (!IsKeyword(commandName)) return null;

            var targetType = GetNextWord().ToLower();
            parsedOffset++;

            var targetName = GetTargetName();

            if (IncompleteCommandTarget(targetType, targetName) || !CommandFactory.CanCreateCommand(commandName))
            {
                throw new InvalidOperationException("Invalid command.");
            }

            // key for getting ctor is command default name, parameter for ctor should be actual command name
            return CommandFactory.GetCommandCtor(commandName)(commandName, targetType, targetName);
        }

        string GetCommandName()
        {
            var word = GetNextWord();

            if (word.Contains('\n')) word = GetWordAfterLineFeed(word);

            while (word != string.Empty && !IsKeyword(word))
            {
                parsedOffset++;
                word = GetNextWord();
                if (word.Contains('\n')) word = GetWordAfterLineFeed(word);
            }

            return word.ToLower();
        }

        string GetTargetName()
        {
            var nameParts = new List<string>();
            var delimiter = new DelimiterWrapper();

            var word = GetNextWord();

            while (word != string.Empty)
            {
                if (word.Contains('\n'))
                {
                    word = GetWordBeforeLineFeed(word);
                    if (word != string.Empty) nameParts.Add(word);
                    break;
                }
                if (IsKeyword(word))
                {
                    delimiter.UpdateDelimiterContext(word);
                    if (!delimiter.Escape() && !delimiter.DelimiterSet) break;

                    if (delimiter.Escape()) nameParts.Add(word);
                }
                else
                {
                    nameParts.Add(word);
                }

                parsedOffset++;
                word = GetNextWord();
            }

            return ParsePascalCase(nameParts);
        }

        string GetNextWord() => parsedOffset < parsedWords.Length ? parsedWords[parsedOffset] : string.Empty;

        static bool IsKeyword(string word) => validKeywords.Contains(word.ToLower());
        static bool IncompleteCommandTarget(string targetType, string targetName)
            => targetType == string.Empty || targetName == string.Empty;

        static string ParsePascalCase(IEnumerable<string> words)
        {
            var pascalCaseWord = new StringBuilder();

            foreach (var word in words)
            {
                var firstLetter = word[0].ToString().ToUpper();
                pascalCaseWord.Append(firstLetter + word[1..]);
            }

            return pascalCaseWord.ToString();
        }

        static string GetWordBeforeLineFeed(string word)
        {
            var substring = new StringBuilder();

            int index = 0;
            while (index < word.Length && word[index] != '\n')
            {
                substring.Append(word[index]);
                index++;
            }

            return substring.ToString();
        }

        static string GetWordAfterLineFeed(string word)
        {
            var wordFragments = word.Split('\n');
            var substring = string.Empty;

            for (int i = wordFragments.Length - 1; i >= 0; i--)
            {
                if (wordFragments[i] != string.Empty)
                {
                    substring = wordFragments[i];
                    break;
                }
            }

            return substring;
        }
    }
}
