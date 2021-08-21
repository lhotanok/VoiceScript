using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Commands.LanguageFormats
{
    abstract class Format
    {
        public abstract Dictionary<string, List<string>> CommandFormats { get; }
        public abstract string DelimiterFormat { get; }
        /// <summary>
        /// Get list of possible command formats.
        /// </summary>
        /// <param name="commandDefault"></param>
        /// <returns>List of formats or null
        /// if command's default format is unknown.</returns>
        public List<string> GetFormats(string commandDefault)
        {
            return CommandFormats.ContainsKey(commandDefault)
                    ? CommandFormats[commandDefault]
                    : null;
        }
    }
}
