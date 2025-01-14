﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Managers;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.IO.Handlers;

namespace StardustSandbox.Core.Managers
{
    internal sealed class SGraphicsManager : SManager, ISGraphicsManager
    {
        public GraphicsDeviceManager GraphicsDeviceManager => this._graphicsDeviceManager;
        public GraphicsDevice GraphicsDevice => this._graphicsDeviceManager.GraphicsDevice;
        public GameWindow GameWindow { get; private set; }

        public Viewport Viewport => this.GraphicsDevice.Viewport;

        internal RenderTarget2D ScreenRenderTarget => this.screenRenderTarget;
        internal RenderTarget2D GuiRenderTarget => this.guiRenderTarget;
        internal RenderTarget2D BackgroundRenderTarget => this.backgroundRenderTarget;
        internal RenderTarget2D WorldRenderTarget => this.worldRenderTarget;
        internal RenderTarget2D WorldLightingRenderTarget => this.worldLightingRenderTarget;

        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        // ENGINE
        private RenderTarget2D screenRenderTarget;

        // SCENE
        private RenderTarget2D guiRenderTarget;
        private RenderTarget2D backgroundRenderTarget;
        private RenderTarget2D worldRenderTarget;
        private RenderTarget2D worldLightingRenderTarget;

        public SGraphicsManager(ISGame gameInstance, GraphicsDeviceManager graphicsDeviceManager) : base(gameInstance)
        {
            this._graphicsDeviceManager = graphicsDeviceManager;
            ApplySettings();
        }

        public override void Initialize()
        {
            int width = SScreenConstants.DEFAULT_SCREEN_WIDTH;
            int height = SScreenConstants.DEFAULT_SCREEN_HEIGHT;

            this.screenRenderTarget = new(this.GraphicsDevice, width, height);
            this.guiRenderTarget = new(this.GraphicsDevice, width, height);
            this.backgroundRenderTarget = new(this.GraphicsDevice, width, height);
            this.worldRenderTarget = new(this.GraphicsDevice, width, height);
            this.worldLightingRenderTarget = new(this.GraphicsDevice, width, height);
        }

        internal void SetGameWindow(GameWindow gameWindow)
        {
            this.GameWindow = gameWindow;
        }

        public void ApplySettings()
        {
            SVideoSettings videoSettings = SSettingsHandler.LoadSettings<SVideoSettings>();

            if (this.GameWindow != null)
            {
                this.GameWindow.IsBorderless = videoSettings.Borderless;
            }

            if (videoSettings.Resolution.Width == 0 || videoSettings.Resolution.Height == 0)
            {
                this._graphicsDeviceManager.PreferredBackBufferWidth = SScreenConstants.DEFAULT_SCREEN_WIDTH;
                this._graphicsDeviceManager.PreferredBackBufferHeight = SScreenConstants.DEFAULT_SCREEN_HEIGHT;
            }
            else
            {
                this._graphicsDeviceManager.PreferredBackBufferWidth = videoSettings.Resolution.Width;
                this._graphicsDeviceManager.PreferredBackBufferHeight = videoSettings.Resolution.Height;
            }

            this._graphicsDeviceManager.IsFullScreen = videoSettings.FullScreen;
            this._graphicsDeviceManager.SynchronizeWithVerticalRetrace = videoSettings.VSync;
            this._graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
            this._graphicsDeviceManager.ApplyChanges();
        }

        public Vector2 GetScreenScaleFactor()
        {
            return new(
                this._graphicsDeviceManager.PreferredBackBufferWidth / (float)SScreenConstants.DEFAULT_SCREEN_WIDTH,
                this._graphicsDeviceManager.PreferredBackBufferHeight / (float)SScreenConstants.DEFAULT_SCREEN_HEIGHT
            );
        }

        public void Reset()
        {
            return;
        }
    }
}
