using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Objects;

namespace StardustSandbox.Core.Entities
{
    public abstract class SEntity : SGameObject, ISPoolableObject
    {
        public SEntityDescriptor Descriptor => descriptor;

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        protected ISWorld SWorldInstance { get; set; }

        private readonly SEntityDescriptor descriptor;

        public SEntity(ISGame gameInstance, SEntityDescriptor descriptor) : base(gameInstance)
        {
            this.descriptor = descriptor;

            this.SWorldInstance = gameInstance.World;

            Reset();
        }

        public void Reset()
        {
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Rotation = 0f;

            OnRestarted();
        }

        public void Destroy()
        {
            OnDestroyed();
        }

        #region Events
        protected virtual void OnSerialized() { return; }
        protected virtual void OnDeserialized() { return; }
        protected virtual void OnRestarted() { return; }
        protected virtual void OnDestroyed() { return; }
        #endregion
    }
}