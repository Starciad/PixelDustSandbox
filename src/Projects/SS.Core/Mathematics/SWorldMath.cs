﻿using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;

namespace StardustSandbox.Core.Mathematics
{
    internal static class SWorldMath
    {
        public static Vector2 ToWorldPosition(Vector2 globalPosition)
        {
            return new(
                (int)(globalPosition.X / SWorldConstants.GRID_SIZE),
                (int)(globalPosition.Y / SWorldConstants.GRID_SIZE)
            );
        }

        public static Vector2 ToGlobalPosition(Vector2 worldPosition)
        {
            return new(
                (int)(worldPosition.X * SWorldConstants.GRID_SIZE),
                (int)(worldPosition.Y * SWorldConstants.GRID_SIZE)
            );
        }
    }
}
