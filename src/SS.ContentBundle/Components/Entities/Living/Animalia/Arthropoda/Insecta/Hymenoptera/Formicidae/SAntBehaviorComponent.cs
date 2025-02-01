using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;

namespace StardustSandbox.ContentBundle.Components.Entities.Living.Animalia.Arthropoda.Insecta.Hymenoptera.Formicidae
{
    internal sealed class SAntBehaviorComponent : SEntityComponent
    {
        private static string[] INTERACTIVE_ELEMENTS_ALLOWED = [
            SElementConstants.DIRT_IDENTIFIER,
            SElementConstants.MUD_IDENTIFIER,
            SElementConstants.STONE_IDENTIFIER,
            SElementConstants.GRASS_IDENTIFIER,
            SElementConstants.SAND_IDENTIFIER,
            SElementConstants.SNOW_IDENTIFIER,
            SElementConstants.WOOD_IDENTIFIER,
            SElementConstants.TREE_LEAF_IDENTIFIER
        ];

        private Vector2 TargetPosition => SWorldMath.ToGlobalPosition(this.localPosition);

        private Point localPosition;

        private bool isClimbing;
        private SCardinalDirection direction;

        private readonly STransformComponent transformComponent;

        internal SAntBehaviorComponent(ISGame gameInstance, SEntity entityInstance, STransformComponent transformComponent) : base(gameInstance, entityInstance)
        {
            this.transformComponent = transformComponent;
        }

        public override void Initialize()
        {
            this.localPosition = SWorldMath.ToWorldPosition(this.transformComponent.Position);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateSmoothMovement();
            UpdateSprite();
        }

        protected override void OnStep()
        {
            ApplyGravity();

            if (SRandomMath.Chance(50, 100))
            {
                Act();
            }
        }

        private void UpdateSmoothMovement()
        {
            this.transformComponent.Position = Vector2.Lerp(this.transformComponent.Position, this.TargetPosition, 0.5f);
        }

        private void UpdateSprite()
        {

        }

        private void ApplyGravity()
        {
            Point positionBelow = new(this.localPosition.X, this.localPosition.Y + 1);

            if (this.SGameInstance.World.IsEmptyWorldSlotLayer(positionBelow, SWorldLayer.Foreground) &&
                this.SGameInstance.World.InsideTheWorldDimensions(positionBelow))
            {
                this.localPosition = positionBelow;
            }
        }

        private void Act()
        {

        }
    }
}
