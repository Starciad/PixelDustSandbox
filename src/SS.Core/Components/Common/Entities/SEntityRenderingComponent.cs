using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

using System.Collections.Generic;

namespace StardustSandbox.Core.Components.Common.Entities
{
    public sealed class SEntityRenderingComponent : SEntityComponent
    {
        public Rectangle? TextureClipArea { get; set; }
        public Color Color { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects SpriteEffect { get; set; }

        private const string TEXTURE_CLIP_AREA_KEY = "rendering_texture_clip_area";
        private const string COLOR_KEY = "rendering_color";
        private const string ORIGIN_KEY = "rendering_origin";
        private const string SPRITE_EFFECTS_KEY = "rendering_sprite_effects";

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

        protected override void OnSerialized(IDictionary<string, object> data)
        {
            if (this.TextureClipArea != null)
            {
                data.Add(TEXTURE_CLIP_AREA_KEY, this.TextureClipArea);
            }
            
            data.Add(COLOR_KEY, this.Color);
            data.Add(ORIGIN_KEY, this.Origin);
            data.Add(SPRITE_EFFECTS_KEY, this.SpriteEffect);
        }

        protected override void OnDeserialized(IReadOnlyDictionary<string, object> data)
        {
            if (data.TryGetValue(TEXTURE_CLIP_AREA_KEY, out object value))
            {
                this.TextureClipArea = (Rectangle)value;
            }
            
            this.Color = (Color)data[COLOR_KEY];
            this.Origin = (Vector2)data[ORIGIN_KEY];
            this.SpriteEffect = (SpriteEffects)data[SPRITE_EFFECTS_KEY];
        }
    }
}