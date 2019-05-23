using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zenit.Ast;
using Zenit.Syntax;

namespace Zenit.FrontEnd
{
    class TestParser
    {
        private SyntacticAnalysis syntacticAnalysis;
        private bool keepSources;

        public TestParser(bool keepSources = false)
        {
            this.syntacticAnalysis = new SyntacticAnalysis();
            this.Sources = new List<string>();
            this.keepSources = keepSources;
        }

        public List<string> Sources { get; }

        public List<string> LoadFrom(string file)
        {
            if (!File.Exists(file))
                return new List<string>();

            var fileInfo = new FileInfo(file);

            var srcLines = File.ReadAllLines(file)
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith("//"))
                    .ToList();

            var lines = new List<string>();

            foreach (var line in srcLines)
            {
                if (line.StartsWith("@include"))
                {
                    lines.AddRange(this.LoadFrom($"{fileInfo.DirectoryName}/{line.Replace("@include", "").Trim()}"));
                }
                else
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public void SaveTo(string file)
        {
            File.WriteAllLines(file, this.Sources);
        }

        public (string source, Node ast) Parse(string source)
        {
            try
            {
                if (this.keepSources)
                    this.Sources.Add(source);

                return (source, this.syntacticAnalysis.Run(source));
            }
            catch (Exception e)
            {
                throw new Exception($"\n\nSyntactic analysis has failed with error: \"{e.Message}\". \n\n\"{source}\"\n\n", e);
            }
        }
    }
}
