namespace SadConsole.Game.Entitas.CodeGenerator {
    public interface IComponentCodeGenerator : ICodeGenerator {
        CodeGenFile[] Generate(ComponentInfo[] componentInfos);
    }
}

