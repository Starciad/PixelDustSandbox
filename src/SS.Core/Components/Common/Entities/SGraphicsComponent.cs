using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Components.Common.Entities
{
    public sealed class SGraphicsComponent(ISGame gameInstance, SEntity entityInstance) : SEntityComponent(gameInstance, entityInstance)
    {
        public Texture2D Texture { get; set; }

        public override void Reset()
        {
            this.Texture = null;
        }
    }
}
