﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Colors;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.General;

using System;

namespace StardustSandbox.Core
{
    public sealed partial class SGame
    {
        protected override void Draw(GameTime gameTime)
        {
            #region RENDERING (ELEMENTS)
            DrawBackground(gameTime);
            DrawWorld(gameTime);
            DrawLighting(gameTime);
            DrawGUI(gameTime);
            #endregion

            #region RENDERING (SCREEN)
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.ScreenRenderTarget);
            this.GraphicsDevice.Clear(SColorPalette.DarkGray);
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.graphicsManager.BackgroundRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.WorldRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.LightingRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.Draw(this.graphicsManager.GuiRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            this.spriteBatch.End();
            #endregion

            #region RENDERING (FINAL)
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(SColorPalette.DarkGray);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.spriteBatch.Draw(this.graphicsManager.ScreenRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, this.graphicsManager.GetScreenScaleFactor(), SpriteEffects.None, 0f);
            this.cursorManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }

        private void DrawBackground(GameTime gameTime)
        {
            // Define
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.BackgroundRenderTarget);
            this.GraphicsDevice.Clear(this.backgroundManager.SolidColor);

            // Sky
            if (this.backgroundManager.SkyHandler.IsActive)
            {
                SDayPeriod dayPeriod = this.world.Time.GetCurrentDayPeriod();

                SSkyGradientColorMap skyGradientColorMap = this.backgroundManager.SkyHandler.GradientColorMap[(byte)dayPeriod];
                float interpolation = skyGradientColorMap.GetInterpolationFactor(this.world.Time.CurrentTime);

                this.backgroundManager.SkyHandler.Effect.Parameters["StartColor1"].SetValue(skyGradientColorMap.Color1.Start.ToVector4());
                this.backgroundManager.SkyHandler.Effect.Parameters["StartColor2"].SetValue(skyGradientColorMap.Color2.Start.ToVector4());
                this.backgroundManager.SkyHandler.Effect.Parameters["EndColor1"].SetValue(skyGradientColorMap.Color1.End.ToVector4());
                this.backgroundManager.SkyHandler.Effect.Parameters["EndColor2"].SetValue(skyGradientColorMap.Color2.End.ToVector4());
                this.backgroundManager.SkyHandler.Effect.Parameters["TimeNormalized"].SetValue(interpolation);

                this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, this.backgroundManager.SkyHandler.Effect, null);
                this.spriteBatch.Draw(this.backgroundManager.SkyHandler.Texture, Vector2.Zero, null, SColorPalette.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                this.spriteBatch.End();
            }

            // Background
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.backgroundManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawWorld(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.WorldRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.world.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }

        private void DrawLighting(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.LightingRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, this.cameraManager.GetViewMatrix());
            this.spriteBatch.End();
        }

        private void DrawGUI(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.graphicsManager.GuiRenderTarget);
            this.GraphicsDevice.Clear(Color.Transparent);
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, null);
            this.guiManager.Draw(gameTime, this.spriteBatch);
            this.spriteBatch.End();
        }
    }
}
