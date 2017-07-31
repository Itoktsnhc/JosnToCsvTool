using System;
using System.IO;
using Newtonsoft.Json;
using JsonToCsv;
using Newtonsoft.Json.Linq;

namespace JsonToCsvTool
{
    class Program
    {
        static void Main(String[] args)
        {
            var outputDir = @"output";
            var jsonStr = File.ReadAllText(@"example.json");
            var converter = new JsonToCsvConverter();
            var csvStr = converter.Convert(jsonStr);

        }
    }
}