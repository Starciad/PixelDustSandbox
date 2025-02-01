using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Helpers;
using StardustSandbox.Core.Interfaces;

using System;

namespace StardustSandbox.Core.Components.Common.Entities
{
    public sealed class SEntityTransformComponent : SEntityComponent
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        public SEntityTransformComponent(ISGame gameInstance, SEntity entityInstance) : base(gameInstance, entityInstance)
        {
            Reset();
        }

        public override void Reset()
        {
            this.Position = Vector2.Zero;
            this.Scale = Vector2.One;
            this.Rotation = 0f;
        }

        protected override object[] OnSerialized()
        {
            return [
                this.Position.X, // [0]
                this.Position.Y, // [1]
                this.Scale.X, // [2]
                this.Scale.Y, // [3]
                this.Rotation, // [4]
            ];
        }

        protected override void OnDeserialized(ReadOnlySpan<object> data)
        {
            this.Position = new(
                SConversionHelper.ConvertTo<float>(data[0]),
                SConversionHelper.ConvertTo<float>(data[1])
            );
            this.Scale = new(
                SConversionHelper.ConvertTo<float>(data[2]),
                SConversionHelper.ConvertTo<float>(data[3])
            );
            this.Rotation = SConversionHelper.ConvertTo<float>(data[4]);
        }
    }
}