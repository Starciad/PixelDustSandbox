using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Mathematics.Primitives;

namespace StardustSandbox.ContentBundle.Entities.Specials
{
    internal sealed class SMagicCursorEntityDescriptor(ISGame gameInstance, string identifier) : SEntityDescriptor(gameInstance, identifier)
    {
        public override SEntity CreateEntity()
        {
            return new SMagicCursorEntity(this.SGameInstance, this);
        }
    }

    internal sealed class SMagicCursorEntity : SEntity
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

        private Vector2 targetPosition;
        private string selectedElement;

        private SMoveState currentMoveState;
        private SBuildingState currentBuildingState;

        private int moveStateTimer;
        private int buildingStateTimer;
        private int elementChangeTimer;

        private readonly Texture2D texture;
        private readonly SSize2 worldSize;

        internal SMagicCursorEntity(ISGame gameInstance, SEntityDescriptor descriptor) : base(gameInstance, descriptor)
        {
            this.texture = gameInstance.AssetDatabase.GetTexture("cursor_1");
            this.worldSize = this.SWorldInstance.Infos.Size * SWorldConstants.GRID_SIZE;
        }

        public override void Update(GameTime gameTime)
        {
            HandleStateTransition();
            UpdateElementSelection();
            ExecuteStateActions();
            UpdateSmoothMovement();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position, new(new(0), new(36)), Color.White, this.Rotation, Vector2.Zero, this.Scale, SpriteEffects.None, 0f);
        }

        #region Update
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
            Point gridPosition = (this.Position / SWorldConstants.GRID_SIZE).ToPoint();

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
                    this.SWorldInstance.InstantiateElement(gridPosition, SWorldLayer.Foreground, this.selectedElement);
                    break;

                case SBuildingState.Removing:
                    this.SWorldInstance.DestroyElement(gridPosition, SWorldLayer.Foreground);
                    break;

                default:
                    break;
            }
        }

        private void UpdateSmoothMovement()
        {
            this.Position = Vector2.Lerp(this.Position, this.targetPosition, 0.1f);
        }

        private void SelectRandomPosition()
        {
            this.targetPosition = new(SRandomMath.Range(0, this.worldSize.Width), SRandomMath.Range(0, this.worldSize.Height));
        }
        #endregion

        protected override void OnRestarted()
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