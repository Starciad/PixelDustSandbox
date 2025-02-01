using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Common.Entities;
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
        private readonly Texture2D texture;

        private readonly SEntityTransformComponent transformComponent;
        private readonly SEntityGraphicsComponent graphicsComponent;
        private readonly SEntityRenderingComponent renderingComponent;

        internal SAntEntity(ISGame gameInstance, SEntityDescriptor descriptor) : base(gameInstance, descriptor)
        {
            this.texture = gameInstance.AssetDatabase.GetTexture("entity_1");

            this.transformComponent = new(this.SGameInstance, this);
            this.graphicsComponent = new(this.SGameInstance, this)
            {
                Texture = this.texture
            };
            this.renderingComponent = new(this.SGameInstance, this, this.transformComponent, this.graphicsComponent)
            {
                TextureClipArea = new(new(0, 64), new(32))
            };

            _ = this.ComponentContainer.AddComponent(this.transformComponent);
            _ = this.ComponentContainer.AddComponent(this.graphicsComponent);
            _ = this.ComponentContainer.AddComponent(this.renderingComponent);
        }
    }
}
