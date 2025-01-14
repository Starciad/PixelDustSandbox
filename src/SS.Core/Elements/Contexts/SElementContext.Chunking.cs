﻿using Microsoft.Xna.Framework;

namespace StardustSandbox.Core.Elements.Contexts
{
    internal sealed partial class SElementContext
    {
        public void NotifyChunk()
        {
            NotifyChunk(this.Position);
        }
        public void NotifyChunk(Point position)
        {
            this.world.NotifyChunk(position);
        }
        public bool TryNotifyChunk()
        {
            return TryNotifyChunk(this.Position);
        }
        public bool TryNotifyChunk(Point position)
        {
            return this.world.TryNotifyChunk(position);
        }
    }
}
