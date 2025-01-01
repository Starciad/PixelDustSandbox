﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Background.Handlers;
using StardustSandbox.Core.Objects;

using System;

namespace StardustSandbox.Core.Background.Handlers
{
    internal sealed class SSkyHandler(ISGame gameInstance) : SGameObject(gameInstance), ISSkyHandler
    {
        public bool IsActive { get; set; } = true;
        public Texture2D Texture => this.texture;
        public Effect Effect => this.effect;

        private Texture2D texture;
        private Effect effect;

        private readonly SGradientColorMap[] skyGradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(3, 0, 0),  // Late Night
                Color1 = (SColorPalette.DarkPurple, SColorPalette.NavyBlue),
                Color2 = (SColorPalette.NavyBlue, SColorPalette.DarkTeal),
            },

            new()
            {
                StartTime = new(3, 0, 0), // Late Night
                EndTime = new(6, 0, 0),  // Dawn
                Color1 = (SColorPalette.NavyBlue, SColorPalette.DarkTeal),
                Color2 = (SColorPalette.DarkTeal, SColorPalette.OrangeRed),
            },

            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(8, 0, 0),  // Early Morning
                Color1 = (SColorPalette.DarkTeal, SColorPalette.OrangeRed),
                Color2 = (SColorPalette.SkyBlue, SColorPalette.Orange),
            },

            new()
            {
                StartTime = new(8, 0, 0), // Early Morning
                EndTime = new(12, 0, 0), // Noon
                Color1 = (SColorPalette.SkyBlue, SColorPalette.Orange),
                Color2 = (SColorPalette.SkyBlue, SColorPalette.LemonYellow),
            },

            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(15, 0, 0),  // Early Afternoon
                Color1 = (SColorPalette.SkyBlue, SColorPalette.LemonYellow),
                Color2 = (SColorPalette.SkyBlue, SColorPalette.Gold),
            },

            new()
            {
                StartTime = new(15, 0, 0), // Early Afternoon
                EndTime = new(18, 0, 0),  // Dusk
                Color1 = (SColorPalette.SkyBlue, SColorPalette.Gold),
                Color2 = (SColorPalette.OrangeRed, SColorPalette.DarkTeal),
            },

            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(20, 0, 0), // Evening
                Color1 = (SColorPalette.OrangeRed, SColorPalette.DarkTeal),
                Color2 = (SColorPalette.DarkTeal, SColorPalette.NavyBlue),
            },

            new()
            {
                StartTime = new(20, 0, 0), // Evening
                EndTime = new(23, 59, 59), // Midnight
                Color1 = (SColorPalette.DarkTeal, SColorPalette.NavyBlue),
                Color2 = (SColorPalette.DarkPurple, SColorPalette.NavyBlue),
            },
        ];
        private readonly SGradientColorMap[] backgroundGradientColorMap = [
            new()
            {
                StartTime = new(0, 0, 0), // Midnight
                EndTime = new(3, 0, 0),  // Late Night
                Color1 = (new Color(60, 40, 90, 200), new Color(30, 30, 60, 180)),
                Color2 = (new Color(30, 30, 60, 180), new Color(15, 20, 40, 160)),
            },
        
            new()
            {
                StartTime = new(3, 0, 0), // Late Night
                EndTime = new(6, 0, 0),  // Dawn
                Color1 = (new Color(30, 30, 60, 180), new Color(20, 25, 50, 170)),
                Color2 = (new Color(25, 40, 70, 170), new Color(60, 30, 50, 150)),
            },
        
            new()
            {
                StartTime = new(6, 0, 0), // Dawn
                EndTime = new(8, 0, 0),  // Early Morning
                Color1 = (new Color(70, 80, 100, 220), new Color(100, 70, 50, 200)),
                Color2 = (new Color(110, 130, 170, 240), new Color(180, 100, 70, 220)),
            },
        
            new()
            {
                StartTime = new(8, 0, 0), // Early Morning
                EndTime = new(12, 0, 0), // Noon
                Color1 = (new Color(110, 130, 170, 240), new Color(190, 160, 120, 230)),
                Color2 = (new Color(160, 200, 250, 255), new Color(230, 210, 150, 250)),
            },
        
            new()
            {
                StartTime = new(12, 0, 0), // Noon
                EndTime = new(15, 0, 0),  // Early Afternoon
                Color1 = (new Color(160, 200, 250, 255), new Color(220, 180, 130, 245)),
                Color2 = (new Color(200, 220, 250, 255), new Color(250, 220, 160, 255)),
            },
        
            new()
            {
                StartTime = new(15, 0, 0), // Early Afternoon
                EndTime = new(18, 0, 0),  // Dusk
                Color1 = (new Color(200, 220, 250, 255), new Color(250, 200, 130, 235)),
                Color2 = (new Color(220, 130, 100, 220), new Color(130, 80, 60, 200)),
            },
        
            new()
            {
                StartTime = new(18, 0, 0), // Dusk
                EndTime = new(20, 0, 0), // Evening
                Color1 = (new Color(220, 130, 100, 220), new Color(120, 70, 50, 200)),
                Color2 = (new Color(80, 40, 50, 180), new Color(30, 30, 60, 170)),
            },
        
            new()
            {
                StartTime = new(20, 0, 0), // Evening
                EndTime = new(23, 59, 59), // Midnight
                Color1 = (new Color(80, 40, 50, 180), new Color(40, 30, 60, 160)),
                Color2 = (new Color(30, 20, 50, 150), new Color(15, 15, 30, 140)),
            },
        ];

        public override void Initialize()
        {
            this.texture = this.SGameInstance.AssetDatabase.GetTexture("background_4");
            this.effect = this.SGameInstance.AssetDatabase.GetEffect("effect_1");
        }

        public SGradientColorMap GetBackgroundGradientByTime(TimeSpan currentTime)
        {
            return Array.Find(this.backgroundGradientColorMap, x =>
            {
                return currentTime >= x.StartTime && currentTime < x.EndTime;
            });
        }

        public SGradientColorMap GetSkyGradientByTime(TimeSpan currentTime)
        {
            return Array.Find(this.skyGradientColorMap, x =>
            {
                return currentTime >= x.StartTime && currentTime < x.EndTime;
            });
        }
    }
}
