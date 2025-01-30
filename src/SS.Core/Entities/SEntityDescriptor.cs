using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Entities
{
    public abstract class SEntityDescriptor(ISGame gameInstance, string identifier)
    {
        public string Identifier => identifier;
        protected ISGame SGameInstance => gameInstance;

        public abstract SEntity CreateEntity();
    }
}