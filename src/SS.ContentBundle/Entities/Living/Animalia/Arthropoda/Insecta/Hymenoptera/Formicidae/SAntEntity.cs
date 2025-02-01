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

        private readonly STransformComponent transformComponent;
        private readonly SGraphicsComponent graphicsComponent;
        private readonly SRenderingComponent renderingComponent;

        internal SAntEntity(ISGame gameInstance, SEntityDescriptor descriptor) : base(gameInstance, descriptor)
        {
            this.texture = gameInstance.AssetDatabase.GetTexture("entity_1");

            this.transformComponent = new(this.SGameInstance, this);
            this.graphicsComponent = new(this.SGameInstance, this);
            this.renderingComponent = new(this.SGameInstance, this, this.transformComponent, this.graphicsComponent);

            _ = this.ComponentContainer.AddComponent(this.transformComponent);
            _ = this.ComponentContainer.AddComponent(this.graphicsComponent);
            _ = this.ComponentContainer.AddComponent(this.renderingComponent);
        }

        public override void Initialize()
        {
            this.graphicsComponent.Texture = texture;
            this.renderingComponent.TextureClipArea = new(new(0, 64), new(32));

            base.Initialize();
        }
    }
}
