using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Helpers;
using StardustSandbox.Core.Interfaces;

using System;

namespace StardustSandbox.Core.Components.Common.Entities
{
    public sealed class SEntityRenderingComponent : SEntityComponent
    {
        public Rectangle? TextureClipArea { get; set; }
        public Color Color { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        private readonly SEntityTransformComponent transformComponent;
        private readonly SEntityGraphicsComponent graphicsComponent;

        public SEntityRenderingComponent(ISGame gameInstance, SEntity entityInstance, SEntityTransformComponent transformComponent, SEntityGraphicsComponent graphicsComponent) : base(gameInstance, entityInstance)
        {
            this.transformComponent = transformComponent;
            this.graphicsComponent = graphicsComponent;

            Reset();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.graphicsComponent.Texture == null)
            {
                return;
            }

            spriteBatch.Draw(this.graphicsComponent.Texture, this.transformComponent.Position, this.TextureClipArea, this.Color, this.transformComponent.Rotation, this.Origin, this.transformComponent.Scale, this.SpriteEffect, 0f);
        }

        public override void Reset()
        {
            this.TextureClipArea = null;
            this.Color = Color.White;
            this.Origin = Vector2.Zero;
            this.SpriteEffect = SpriteEffects.None;
        }

        protected override object[] OnSerialized()
        {
            return [
                this.Color.R, // [0]
                this.Color.G, // [1]
                this.Color.B, // [2]
                this.Color.A, // [3]
            ];
        }

        protected override void OnDeserialized(ReadOnlySpan<object> data)
        {
            this.Color = new(
                SConversionHelper.ConvertTo<byte>(data[0]),
                SConversionHelper.ConvertTo<byte>(data[1]),
                SConversionHelper.ConvertTo<byte>(data[2]),
                SConversionHelper.ConvertTo<byte>(data[3])
            );
        }
    }
}