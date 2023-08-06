﻿using Microsoft.Xna.Framework;

using PixelDust.Core.Elements;

using PixelDust.Game.Elements.Gases;
using PixelDust.Game.Elements.Solid.Immovable;
using PixelDust.Game.Elements.Solid.Movable;

namespace PixelDust.Game.Elements.Liquid
{
    [PElementRegister]
    internal class Lava : PLiquid
    {
        protected override void OnSettings()
        {
            Name = "Lava";
            Description = string.Empty;
            Color = new(255, 116, 0);
        }

        protected override void OnBeforeStep(PElementContext ctx)
        {
            Vector2[] targets = new Vector2[]
            {
                new(ctx.Position.X    , ctx.Position.Y - 1),
                new(ctx.Position.X + 1, ctx.Position.Y - 1),
                new(ctx.Position.X - 1, ctx.Position.Y - 1),

                new(ctx.Position.X + 1, ctx.Position.Y),
                new(ctx.Position.X - 1, ctx.Position.Y),

                new(ctx.Position.X    , ctx.Position.Y + 1),
                new(ctx.Position.X + 1, ctx.Position.Y + 1),
                new(ctx.Position.X - 1, ctx.Position.Y + 1),
            };

            foreach (Vector2 targetPos in targets)
            {
                if (ctx.TryGetElement(targetPos, out PElement value))
                {
                    if (value is Stone)
                    {
                        ctx.TryReplace<Lava>(targetPos);
                        return;
                    }

                    if (value is Water)
                    {
                        ctx.TryReplace<Stone>(ctx.Position);
                        ctx.TryReplace<Steam>(targetPos);
                        return;
                    }

                    if (value is Sand)
                    {
                        ctx.TryReplace<Glass>(targetPos);
                        return;
                    }

                    if (value is Grass)
                    {
                        ctx.TryDestroy(targetPos);
                        return;
                    }
                }
            }
        }
    }
}