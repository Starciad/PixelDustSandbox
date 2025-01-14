﻿using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Immovables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Immovables
{
    internal sealed class STreeLeaf : SImmovableSolid
    {
        internal STreeLeaf(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = SColorPalette.MossGreen;
            this.texture = gameInstance.AssetDatabase.GetTexture("element_22");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 5;
            this.defaultDensity = 400;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue >= 220)
            {
                this.Context.ReplaceElement(SElementConstants.IDENTIFIER_FIRE);
            }
        }
    }
}
