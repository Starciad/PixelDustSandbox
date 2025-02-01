using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.Components.Entities.Specials
{
    internal sealed class SMagicCursorBehaviorComponent : SEntityComponent
    {
        private enum SMoveState
        {
            Static,
            Moving
        }

        private enum SBuildingState
        {
            Constructing,
            Removing
        }

        private static readonly string[] AllowedElements =
        [
            SElementConstants.DIRT_IDENTIFIER,
            SElementConstants.MUD_IDENTIFIER,
            SElementConstants.WATER_IDENTIFIER,
            SElementConstants.STONE_IDENTIFIER,
            SElementConstants.GRASS_IDENTIFIER,
            SElementConstants.SAND_IDENTIFIER,
            SElementConstants.LAVA_IDENTIFIER,
            SElementConstants.ACID_IDENTIFIER,
            SElementConstants.WOOD_IDENTIFIER,
            SElementConstants.TREE_LEAF_IDENTIFIER
        ];

        private SMoveState currentMoveState;
        private SBuildingState currentBuildingState;

        private Vector2 targetPosition;
        private string selectedElement;

        private int moveStateTimer = 0;
        private int buildingStateTimer = 0;
        private int elementChangeTimer = 0;

        private readonly ISWorld world;
        private readonly SSize2 worldSize;

        private readonly STransformComponent transformComponent;

        internal SMagicCursorBehaviorComponent(ISGame gameInstance, SEntity entityInstance, STransformComponent transformComponent) : base(gameInstance, entityInstance)
        {
            this.world = gameInstance.World;
            this.transformComponent = transformComponent;
            this.worldSize = this.world.Infos.Size * SWorldConstants.GRID_SIZE;

            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            HandleStateTransition();
            UpdateElementSelection();
            ExecuteStateActions();
            UpdateSmoothMovement();
        }

        private void HandleStateTransition()
        {
            this.moveStateTimer++;
            this.buildingStateTimer++;

            if (this.moveStateTimer > 10)
            {
                this.moveStateTimer = 0;
                this.currentMoveState = (SMoveState)SRandomMath.Range(0, 3);

                // If moving, select a new target
                if (this.currentMoveState == SMoveState.Moving)
                {
                    SelectRandomPosition();
                }
            }

            if (this.buildingStateTimer > 96)
            {
                this.buildingStateTimer = 0;
                this.currentBuildingState = (SBuildingState)SRandomMath.Range(0, 3);
            }
        }

        private void UpdateElementSelection()
        {
            this.elementChangeTimer++;
            if (this.elementChangeTimer > 350)
            {
                this.elementChangeTimer = 0;
                this.selectedElement = AllowedElements.GetRandomItem();
            }
        }

        private void ExecuteStateActions()
        {
            Point gridPosition = (this.transformComponent.Position / SWorldConstants.GRID_SIZE).ToPoint();

            switch (this.currentMoveState)
            {
                case SMoveState.Static:
                    break;

                case SMoveState.Moving:
                    SelectRandomPosition();
                    break;

                default:
                    break;
            }

            switch (this.currentBuildingState)
            {
                case SBuildingState.Constructing:
                    this.world.InstantiateElement(gridPosition, SWorldLayer.Foreground, this.selectedElement);
                    break;

                case SBuildingState.Removing:
                    this.world.DestroyElement(gridPosition, SWorldLayer.Foreground);
                    break;

                default:
                    break;
            }
        }

        private void UpdateSmoothMovement()
        {
            this.transformComponent.Position = Vector2.Lerp(this.transformComponent.Position, this.targetPosition, 0.1f);
        }

        private void SelectRandomPosition()
        {
            this.targetPosition = new(SRandomMath.Range(0, this.worldSize.Width), SRandomMath.Range(0, this.worldSize.Height));
        }

        public override void Reset()
        {
            this.currentMoveState = SMoveState.Moving;
            this.currentBuildingState = SBuildingState.Constructing;

            this.moveStateTimer = 0;
            this.buildingStateTimer = 0;
            this.elementChangeTimer = 0;

            this.selectedElement = AllowedElements.GetRandomItem();

            SelectRandomPosition();
        }
    }
}
