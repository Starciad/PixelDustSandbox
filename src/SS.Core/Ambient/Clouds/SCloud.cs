﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Ambient.Clouds
{
    public sealed class SCloud(ISGame gameInstance) : SGameObject(gameInstance), ISPoolableObject
    {
        public Texture2D Texture => this.texture;
        public Vector2 Position => this.position;

        private Texture2D texture;
        private Vector2 position;
        private float speed;
        private float opacity;

        public override void Initialize()
        {
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            this.position.X += this.speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White * this.opacity, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Reset()
        {
            this.position = new Vector2(-this.texture.Width - SWorldConstants.GRID_SIZE, SRandomMath.Range(0, SWorldConstants.GRID_SIZE * 10));
            this.speed = SRandomMath.Range(10, 50);
            this.opacity = ((float)SRandomMath.GetDouble() * 0.5f) + 0.5f;
        }
    }
}
