using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.System;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Components
{
    public sealed class SComponentContainer(ISGame gameInstance) : SGameObject(gameInstance), ISResettable
    {
        private readonly List<SComponent> components = [];

        public override void Initialize()
        {
            foreach (SComponent component in this.components)
            {
                component.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (SComponent component in this.components)
            {
                component.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (SComponent component in this.components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        public SComponent AddComponent(SComponent value)
        {
            this.components.Add(value);
            return value;
        }

        public void Reset()
        {
            foreach (SComponent component in this.components)
            {
                component.Reset();
            }
        }
    }
}
