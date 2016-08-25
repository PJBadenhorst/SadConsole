using System;

namespace SadConsole.Game.Entitas.CodeGenerator {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class CustomPrefixAttribute : Attribute {
        public readonly string prefix;

        public CustomPrefixAttribute(string prefix) {
            this.prefix = prefix;
        }
    }
}