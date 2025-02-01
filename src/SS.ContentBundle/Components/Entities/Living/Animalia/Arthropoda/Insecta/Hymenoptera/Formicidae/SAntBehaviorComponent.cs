using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.ContentBundle.Components.Entities.Living.Animalia.Arthropoda.Insecta.Hymenoptera.Formicidae
{
    internal sealed class SAntBehaviorComponent : SEntityComponent
    {
        private Vector2 TargetPosition => SWorldMath.ToGlobalPosition(this.localPosition);

        private Point localPosition;

        private readonly STransformComponent transformComponent;

        internal SAntBehaviorComponent(ISGame gameInstance, SEntity entityInstance, STransformComponent transformComponent) : base(gameInstance, entityInstance)
        {
            this.transformComponent = transformComponent;
        }

        public override void Initialize()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            UpdateSmoothMovement();
        }

        protected override void OnStep()
        {
            this.localPosition = new(
                this.localPosition.X + SRandomMath.Range(-1, 1),
                this.localPosition.Y + SRandomMath.Range(-1, 1)
            );
        }

        private void UpdateSmoothMovement()
        {
            this.localPosition = SWorldMath.ToWorldPosition(this.transformComponent.Position);
            this.transformComponent.Position = Vector2.Lerp(this.transformComponent.Position, this.TargetPosition, 0.1f);
        }
    }
}
