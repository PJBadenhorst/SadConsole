namespace SadConsole.Game.Entitas.CodeGenerator {
    public interface IBlueprintsCodeGenerator : ICodeGenerator {
        CodeGenFile[] Generate(string[] blueprintNames);
    }
}

