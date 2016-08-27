using Entitas.CodeGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntitasCodeGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Building SadConsole entities");

            // All code generators that should be used
            var codeGenerators =
                                    new ICodeGenerator[] {
                                    new ComponentIndicesGenerator(),
                                    new ComponentExtensionsGenerator(),
                                    new PoolAttributesGenerator(),
                                    new PoolsGenerator(),
                                    new BlueprintsGenerator()
            };

            // Specify all pools
            var poolNames = new[] { "Core" };

            // Specify all blueprints
            var blueprintNames = new string[0];

            var assembly = Assembly.GetAssembly(typeof(Entitas.Entity));
            var provider = new TypeReflectionProvider(Assembly.GetEntryAssembly().GetTypes(), poolNames, blueprintNames);

            const string path = "./Generated/";
            var files = CodeGenerator.Generate(provider, path, codeGenerators);

            foreach (var file in files)
            {
                Console.WriteLine(file.generatorName + ": " + file.fileName);
            }

            Console.WriteLine("Done. Press any key...");
            Console.Read();

        }
    }
}
