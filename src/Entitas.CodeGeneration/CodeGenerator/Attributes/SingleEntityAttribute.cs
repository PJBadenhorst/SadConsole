using System;

namespace SadConsole.Game.Entitas.CodeGenerator {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class SingleEntityAttribute : Attribute {
    }
}