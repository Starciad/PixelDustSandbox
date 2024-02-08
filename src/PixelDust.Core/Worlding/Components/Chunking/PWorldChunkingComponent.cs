﻿#pragma warning disable IDE0051

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Engine.Assets;
using PixelDust.Core.Engine.Components;
using PixelDust.Mathematics;

namespace PixelDust.Core.Worlding.Components.Chunking
{
    internal sealed class PWorldChunkingComponent : PWorldComponent
    {
        internal static short DefaultChunkSize => 6;

        private int worldChunkWidth;
        private int worldChunkHeight;

        private PWorldChunk[,] _chunks;

        protected override void OnInitialize()
        {
            this._chunks = new PWorldChunk[(this.WorldInstance.Infos.Size.Width / DefaultChunkSize) + 1,
                                      (this.WorldInstance.Infos.Size.Height / DefaultChunkSize) + 1];

            this.worldChunkWidth = this._chunks.GetLength(0);
            this.worldChunkHeight = this._chunks.GetLength(1);

            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this._chunks[x, y] = new(new(x * DefaultChunkSize * PWorld.Scale, y * DefaultChunkSize * PWorld.Scale), DefaultChunkSize);
                }
            }
        }

        protected override void OnUpdate()
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    this._chunks[x, y].Update();
                }
            }
        }
        protected override void OnDraw()
        {
#if DEBUG
            // Debug methods
            // DEBUG_DrawActiveChunks();
#endif
        }

        internal bool TryGetChunkUpdateState(Vector2Int pos, out bool result)
        {
            result = false;
            Vector2Int targetPos = ToChunkCoordinateSystem(pos);

            if (!IsWithinChunkBoundaries(targetPos))
            {
                return false;
            }

            result = this._chunks[targetPos.X, targetPos.Y].ShouldUpdate;
            return true;
        }
        internal int GetActiveChunksCount()
        {
            int result = 0;
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this._chunks[x, y].ShouldUpdate)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        internal bool TryNotifyChunk(Vector2Int pos)
        {
            Vector2Int targetPos = ToChunkCoordinateSystem(pos);

            if (IsWithinChunkBoundaries(targetPos))
            {
                this._chunks[targetPos.X, targetPos.Y].Notify();
                TryNotifyNeighboringChunks(pos, targetPos);

                return true;
            }

            return false;
        }
        private void TryNotifyNeighboringChunks(Vector2Int ePos, Vector2Int cPos)
        {
            if (ePos.X % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPos.X - 1, cPos.Y)))
            {
                this._chunks[cPos.X - 1, cPos.Y].Notify();
            }

            if (ePos.X % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPos.X + 1, cPos.Y)))
            {
                this._chunks[cPos.X + 1, cPos.Y].Notify();
            }

            if (ePos.Y % DefaultChunkSize == 0 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y - 1)))
            {
                this._chunks[cPos.X, cPos.Y - 1].Notify();
            }

            if (ePos.Y % DefaultChunkSize == DefaultChunkSize - 1 && IsWithinChunkBoundaries(new(cPos.X, cPos.Y + 1)))
            {
                this._chunks[cPos.X, cPos.Y + 1].Notify();
            }
        }

        private bool IsWithinChunkBoundaries(Vector2Int pos)
        {
            return pos.X >= 0 && pos.X < this.worldChunkWidth &&
                   pos.Y >= 0 && pos.Y < this.worldChunkHeight;
        }

        internal static Vector2Int ToChunkCoordinateSystem(Vector2Int pos)
        {
            return new(pos.X / DefaultChunkSize, pos.Y / DefaultChunkSize);
        }

#if DEBUG
        private void DEBUG_DrawActiveChunks()
        {
            for (int x = 0; x < this.worldChunkWidth; x++)
            {
                for (int y = 0; y < this.worldChunkHeight; y++)
                {
                    if (this._chunks[x, y].ShouldUpdate)
                    {
                        PGraphics.SpriteBatch.Draw(PTextures.Pixel, new Vector2(this._chunks[x, y].Position.X, this._chunks[x, y].Position.Y), null, new Color(255, 0, 0, 35), 0f, Vector2.Zero, DefaultChunkSize * PWorld.Scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }
#endif
    }
}
#pragma warning restore IDE0051