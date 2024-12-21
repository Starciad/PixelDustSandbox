﻿using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements;
using StardustSandbox.Core.Elements.Contexts;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Interfaces.World;
using StardustSandbox.Core.World.Data;

using System.Collections.Generic;

namespace StardustSandbox.Core.Components.Common.World
{
    public sealed class SWorldUpdatingComponent(ISGame gameInstance, ISWorld worldInstance) : SWorldComponent(gameInstance, worldInstance)
    {
        private readonly SElementContext elementUpdateContext = new(worldInstance);
        private readonly List<Point> capturedSlots = [];

        public override void Update(GameTime gameTime)
        {
            this.capturedSlots.Clear();
            GetAllSlotsForUpdating(gameTime);
            UpdateAllCapturedSlots(gameTime);
        }

        private void GetAllSlotsForUpdating(GameTime gameTime)
        {
            SWorldChunk[] worldChunks = this.SWorldInstance.GetActiveChunks();

            for (int i = 0; i < worldChunks.Length; i++)
            {
                SWorldChunk worldChunk = worldChunks[i];

                for (int y = 0; y < SWorldConstants.CHUNK_SCALE; y++)
                {
                    for (int x = 0; x < SWorldConstants.CHUNK_SCALE; x++)
                    {
                        Point position = new((worldChunk.Position.X / SWorldConstants.GRID_SCALE) + x, (worldChunk.Position.Y / SWorldConstants.GRID_SCALE) + y);

                        if (this.SWorldInstance.IsEmptyWorldSlot(position))
                        {
                            continue;
                        }

                        UpdateSlotTarget(gameTime, SWorldLayer.Foreground, position, SWorldThreadUpdateType.Update);
                        UpdateSlotTarget(gameTime, SWorldLayer.Background, position, SWorldThreadUpdateType.Update);

                        this.capturedSlots.Add(position);
                    }
                }
            }
        }

        private void UpdateAllCapturedSlots(GameTime gameTime)
        {
            this.capturedSlots.ForEach((position) =>
            {
                UpdateSlotTarget(gameTime, SWorldLayer.Foreground, position, SWorldThreadUpdateType.Step);
                UpdateSlotTarget(gameTime, SWorldLayer.Background, position, SWorldThreadUpdateType.Step);
            });
        }

        private void UpdateSlotTarget(GameTime gameTime, SWorldLayer worldLayer, Point position, SWorldThreadUpdateType updateType)
        {
            ISWorldSlot slot = this.SWorldInstance.GetWorldSlot(position);

            if (this.SWorldInstance.TryGetElement(position, worldLayer, out ISElement value))
            {
                this.elementUpdateContext.UpdateInformation(position, worldLayer, slot);
                value.Context = this.elementUpdateContext;

                switch (updateType)
                {
                    case SWorldThreadUpdateType.Update:
                        ((SElement)value).Update(gameTime);
                        break;

                    case SWorldThreadUpdateType.Step:
                        ((SElement)value).Steps();
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
