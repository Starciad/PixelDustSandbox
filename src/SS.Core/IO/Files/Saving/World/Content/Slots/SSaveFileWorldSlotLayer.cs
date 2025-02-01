﻿using MessagePack;

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Enums.General;
using StardustSandbox.Core.IO.Files.Saving.World.Information;
using StardustSandbox.Core.IO.Files.Saving.World.Information.Resources;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.IO.Files.Saving.World.Content.Slots
{
    [MessagePackObject]
    public sealed class SSaveFileWorldSlotLayer
    {
        [IgnoreMember]
        public Color ColorModifier
        {
            get => new(this.ColorModifierR, this.ColorModifierG, this.ColorModifierB, this.ColorModifierA);

            set
            {
                this.ColorModifierR = value.R;
                this.ColorModifierG = value.G;
                this.ColorModifierB = value.B;
                this.ColorModifierA = value.A;
            }
        }

        [Key(0)] public uint ElementIndex { get; set; }
        [Key(1)] public short Temperature { get; set; }
        [Key(2)] public bool FreeFalling { get; set; }
        [Key(3)] public byte ColorModifierR { get; set; }
        [Key(4)] public byte ColorModifierG { get; set; }
        [Key(5)] public byte ColorModifierB { get; set; }
        [Key(6)] public byte ColorModifierA { get; set; }
        [Key(7)] public SUpdateCycleFlag UpdateCycleFlag { get; set; }
        [Key(8)] public SUpdateCycleFlag StepCycleFlag { get; set; }
        [Key(9)] public uint StoredElementIndex { get; set; }

        public SSaveFileWorldSlotLayer()
        {

        }

        public SSaveFileWorldSlotLayer(SSaveFileResourceContainer container, SWorldSlotLayer worldSlotLayer)
        {
            this.ElementIndex = container.FindIndexByValue(worldSlotLayer.Element.Identifier);
            this.Temperature = worldSlotLayer.Temperature;
            this.FreeFalling = worldSlotLayer.FreeFalling;
            this.ColorModifier = worldSlotLayer.ColorModifier;
            this.UpdateCycleFlag = worldSlotLayer.UpdateCycleFlag;
            this.StepCycleFlag = worldSlotLayer.StepCycleFlag;

            if (worldSlotLayer.StoredElement != null)
            {
                this.StoredElementIndex = container.FindIndexByValue(worldSlotLayer.StoredElement.Identifier);
            }
        }
    }
}
