using System.Reflection;

namespace SadConsole.Game.Entitas.CodeGenerator {
    public static class TypeReflectionCodeGenerator {
        public static CodeGenFile[] Generate(Assembly assembly, string[] poolNames, string[] blueprintNames, string directory, ICodeGenerator[] codeGenerators) {
            var provider = new TypeReflectionProvider(assembly.GetTypes(), poolNames, blueprintNames);
            return CodeGenerator.Generate(provider, directory, codeGenerators);
        }
    }
}

