using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.Core.Entities
{
    public abstract class SEntity : SGameObject, ISPoolableObject
    {
        public SComponentContainer ComponentContainer => componentContainer;
        public SEntityDescriptor Descriptor => descriptor;

        protected ISWorld SWorldInstance { get; set; }

        private readonly SComponentContainer componentContainer;
        private readonly SEntityDescriptor descriptor;

        public SEntity(ISGame gameInstance, SEntityDescriptor descriptor) : base(gameInstance)
        {
            this.descriptor = descriptor;
            this.componentContainer = new(gameInstance);

            this.SWorldInstance = gameInstance.World;

            Reset();
        }

        public override void Initialize()
        {
            this.componentContainer.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.componentContainer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.componentContainer.Draw(gameTime, spriteBatch);
        }

        public void Reset()
        {
            this.componentContainer.Reset();
            OnRestarted();
        }

        public void Destroy()
        {
            OnDestroyed();
        }

        internal IReadOnlyDictionary<string, object> Serialize()
        {
            Dictionary<string, object> data = [];

            foreach (SEntityComponent component in this.componentContainer.Components.Cast<SEntityComponent>())
            {
                component.Serialize(data);
            }

            return data;
        }
        internal void Deserialize(IReadOnlyDictionary<string, object> data)
        {
            foreach (SEntityComponent component in this.componentContainer.Components.Cast<SEntityComponent>())
            {
                component.Deserialize(data);
            }
        }

        #region Events
        protected virtual void OnRestarted() { return; }
        protected virtual void OnDestroyed() { return; }
        #endregion
    }
}