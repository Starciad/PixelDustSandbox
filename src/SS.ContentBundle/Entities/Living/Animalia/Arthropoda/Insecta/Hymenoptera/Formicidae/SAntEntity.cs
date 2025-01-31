using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Entities.Living.Animalia.Arthropoda.Insecta.Hymenoptera.Formicidae
{
    internal sealed class SAntEntityDescriptor(ISGame gameInstance, string identifier) : SEntityDescriptor(gameInstance, identifier)
    {
        public override SEntity CreateEntity()
        {
            return new SAntEntity(this.SGameInstance, this);
        }
    }

    internal sealed class SAntEntity : SEntity
    {
        internal SAntEntity(ISGame gameInstance, SEntityDescriptor descriptor) : base(gameInstance, descriptor)
        {

        }
    }
}
