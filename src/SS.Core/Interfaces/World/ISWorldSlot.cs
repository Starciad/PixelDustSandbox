﻿using Microsoft.Xna.Framework;

using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.General;

namespace StardustSandbox.Core.Interfaces.World
{
    public interface ISWorldSlot : ISPoolableObject
    {
        bool IsEmpty { get; }
        Point Position { get; }
        ISWorldSlotLayer ForegroundLayer { get; }
        ISWorldSlotLayer BackgroundLayer { get; }
    }

    public interface ISWorldSlotLayer
    {
        ISElement Element { get; }
        bool IsEmpty { get; }
        short Temperature { get; }
        bool FreeFalling { get; }
        Color ColorModifier { get; }
    }
}
