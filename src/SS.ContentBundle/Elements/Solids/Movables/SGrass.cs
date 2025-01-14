﻿using StardustSandbox.Core.Constants.Elements;
using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Solids.Movables;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Solids.Movables
{
    internal sealed class SGrass : SMovableSolid
    {
        internal SGrass(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(69, 110, 55, 255);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_5");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 22;
            this.enableFlammability = true;
            this.defaultFlammabilityResistance = 10;
            this.defaultDensity = 1100;
        }

        protected override void OnTemperatureChanged(short currentValue)
        {
            if (currentValue > 200)
            {
                this.Context.ReplaceElement(SElementConstants.IDENTIFIER_FIRE);
            }
        }
    }
}
