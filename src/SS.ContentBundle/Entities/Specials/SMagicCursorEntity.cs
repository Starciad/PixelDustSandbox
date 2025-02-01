using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.ContentBundle.Components.Entities.Specials;
using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Entities.Specials
{
    internal sealed class SMagicCursorEntityDescriptor(ISGame gameInstance, string identifier) : SEntityDescriptor(gameInstance, identifier)
    {
        public override SEntity CreateEntity()
        {
            return new SMagicCursorEntity(this.SGameInstance, this);
        }
    }

    internal sealed class SMagicCursorEntity : SEntity
    {
        private readonly Texture2D texture;

        private readonly STransformComponent transformComponent;
        private readonly SGraphicsComponent graphicsComponent;
        private readonly SRenderingComponent renderingComponent;
        private readonly SMagicCursorBehaviorComponent magicCursorBehaviorComponent;

        internal SMagicCursorEntity(ISGame gameInstance, SEntityDescriptor descriptor) : base(gameInstance, descriptor)
        {
            this.texture = gameInstance.AssetDatabase.GetTexture("cursor_1");

            this.transformComponent = new(this.SGameInstance, this);
            this.graphicsComponent = new(this.SGameInstance, this);
            this.renderingComponent = new(this.SGameInstance, this, this.transformComponent, this.graphicsComponent);
            this.magicCursorBehaviorComponent = new(this.SGameInstance, this, this.transformComponent);

            _ = this.ComponentContainer.AddComponent(this.transformComponent);
            _ = this.ComponentContainer.AddComponent(this.graphicsComponent);
            _ = this.ComponentContainer.AddComponent(this.renderingComponent);
            _ = this.ComponentContainer.AddComponent(this.magicCursorBehaviorComponent);
        }

        public override void Initialize()
        {
            this.graphicsComponent.Texture = this.texture;
            this.renderingComponent.TextureClipArea = new(new(0), new(36));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}