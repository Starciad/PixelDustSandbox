﻿using Microsoft.Xna.Framework;

using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.Elements.Templates.Liquids
{
    public abstract class SLiquid : SElement
    {
        public SLiquid(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.defaultDensity = 1000;
        }

        protected override void OnBehaviourStep()
        {
            foreach (Point belowPosition in SElementUtility.GetRandomSidePositions(this.Context.Slot.Position, SDirection.Down))
            {
                if (TrySetPosition(belowPosition))
                {
                    SElementUtility.NotifyFreeFallingFromAdjacentNeighbors(this.Context, belowPosition);
                    this.Context.SetElementFreeFalling(belowPosition, this.Context.Layer, true);
                    return;
                }
            }

            UpdateHorizontalPosition();
            this.Context.SetElementFreeFalling(this.Context.Layer, false);
        }

        private bool TrySetPosition(Point position)
        {
            if (this.Context.TrySetPosition(position))
            {
                return true;
            }

            if (this.Context.TryGetWorldSlot(position, out SWorldSlot value))
            {
                SWorldSlotLayer worldSlotLayer = value.GetLayer(this.Context.Layer);

                switch (worldSlotLayer.Element)
                {
                    case SGas:
                        this.Context.SwappingElements(position);
                        return true;

                    case SLiquid:
                        if ((worldSlotLayer.Element.GetType() == GetType() && worldSlotLayer.Temperature > this.Context.SlotLayer.Temperature) || worldSlotLayer.Element.DefaultDensity < this.DefaultDensity)
                        {
                            this.Context.SwappingElements(position);
                            return true;
                        }

                        break;

                    default:
                        break;
                }
            }

            return false;
        }

        private void UpdateHorizontalPosition()
        {
            Point left = GetDispersionPosition(-1);
            Point right = GetDispersionPosition(1);

            float leftDistance = SPointExtensions.Distance(this.Context.Slot.Position, left);
            float rightDistance = SPointExtensions.Distance(this.Context.Slot.Position, right);

            Point targetPosition = leftDistance == rightDistance
                ? (SRandomMath.Chance(50) ? left : right)
                : (leftDistance > rightDistance ? left : right);

            if (this.Context.IsEmptyWorldSlotLayer(targetPosition, this.Context.Layer))
            {
                this.Context.SetPosition(targetPosition, this.Context.Layer);
            }
            else
            {
                this.Context.SwappingElements(targetPosition, this.Context.Layer);
            }
        }

        private Point GetDispersionPosition(int direction)
        {
            Point dispersionPosition = this.Context.Slot.Position;

            for (int i = 0; i < this.defaultDispersionRate; i++)
            {
                Point nextPosition = new(dispersionPosition.X + direction, dispersionPosition.Y);

                if (this.Context.IsEmptyWorldSlotLayer(nextPosition, this.Context.Layer) ||
                    this.Context.GetElement(nextPosition, this.Context.Layer) is SLiquid or SGas)
                {
                    dispersionPosition = nextPosition;
                }
                else
                {
                    break;
                }
            }

            return dispersionPosition;
        }
    }
}