using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Objects;

using System;
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

        internal void UpdateSteps()
        {
            foreach (SEntityComponent entityComponent in this.componentContainer.Components.OfType<SEntityComponent>())
            {
                entityComponent.UpdateSteps();
            }

            OnStep();
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

        internal IEnumerable<object[]> Serialize()
        {
            return this.componentContainer.Components.OfType<SEntityComponent>().Select(component => component.Serialize());
        }
        internal void Deserialize(IEnumerable<object[]> componentData)
        {
            SEntityComponent[] components = this.componentContainer.Components.OfType<SEntityComponent>().ToArray();
            object[][] dataArray = componentData as object[][] ?? componentData.ToArray();

            int count = Math.Min(components.Length, dataArray.Length);
            for (int i = 0; i < count; i++)
            {
                components[i].Deserialize(dataArray[i]);
            }
        }

        #region Events
        protected virtual void OnStep() { return; }
        protected virtual void OnRestarted() { return; }
        protected virtual void OnDestroyed() { return; }
        #endregion
    }
}