using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Templates;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Interfaces;

using System;
using System.Collections.Generic;

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

        protected override void OnSerialized(IDictionary<string, object> data)
        {
            data.Add("transform_position", this.Position);
            data.Add("transform_scale", this.Scale);
            data.Add("transform_rotation", this.Rotation);
        }

        protected override void OnDeserialized(IReadOnlyDictionary<string, object> data)
        {
            this.Position = (Vector2)data["transform_position"];
            this.Scale = (Vector2)data["transform_position"];
            this.Rotation = Convert.ToSingle(data["transform_position"]);
        }
    }
}