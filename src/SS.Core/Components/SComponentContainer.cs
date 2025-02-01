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
        public IEnumerable<SComponent> Components => this.components;

        private readonly HashSet<SComponent> components = [];

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

        public bool AddComponent<T>(T value) where T : SComponent
        {
            return this.components.Add(value);
        }

        public T GetComponent<T>() where T : SComponent
        {
            _ = TryGetComponent(out T value);
            return value;
        }

        public bool TryGetComponent<T>(out T value) where T : SComponent
        {
            foreach (SComponent component in this.components)
            {
                if (component is T typedComponent)
                {
                    value = typedComponent;
                    return true;
                }
            }

            value = null;
            return false;
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
