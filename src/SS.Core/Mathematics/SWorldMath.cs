using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;

namespace StardustSandbox.Core.Mathematics
{
    public static class SWorldMath
    {
        public static Point ToWorldPosition(Vector2 globalPosition)
        {
            return new(
                (int)(globalPosition.X / SWorldConstants.GRID_SIZE),
                (int)(globalPosition.Y / SWorldConstants.GRID_SIZE)
            );
        }

        public static Vector2 ToGlobalPosition(Point worldPosition)
        {
            return new(
                (int)(worldPosition.X * SWorldConstants.GRID_SIZE),
                (int)(worldPosition.Y * SWorldConstants.GRID_SIZE)
            );
        }
    }
}
