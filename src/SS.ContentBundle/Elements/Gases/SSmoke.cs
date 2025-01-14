﻿using StardustSandbox.Core.Elements.Rendering;
using StardustSandbox.Core.Elements.Templates.Gases;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.ContentBundle.Elements.Gases
{
    internal sealed class SSmoke : SGas
    {
        internal SSmoke(ISGame gameInstance, string identifier) : base(gameInstance, identifier)
        {
            this.referenceColor = new(56, 56, 56, 191);
            this.texture = gameInstance.AssetDatabase.GetTexture("element_20");
            this.Rendering.SetRenderingMechanism(new SElementBlobRenderingMechanism());
            this.defaultTemperature = 350;
            this.defaultDensity = 2;
        }
    }
}
