﻿using StardustSandbox.Core.Animations;
using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Extensions;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class SMountingBlock : SImmovableSolid
    {
        internal SMountingBlock(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.White;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_23");
            this.Rendering.SetRenderingMechanism(new SElementSingleRenderingMechanism(new SAnimation(gameInstance, [new(new(new(0), new(SSpritesConstants.SPRITE_SCALE)), 0)])));
            this.defaultTemperature = 20;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 150;
            this.defaultDensity = 1800;
        }

        protected override void OnInstantiateStep(SWorldSlot worldSlot, SWorldLayer worldLayer)
        {
            this.Context.SetElementColorModifier(worldLayer, SElementConstants.COLORS_OF_MOUNTING_BLOCKS.GetRandomItem());
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 300)
            {
                this.Context.ReplaceElement(this.Context.Layer, SElementConstants.IDENTIFIER_FIRE);
            }
        }
    }
}
