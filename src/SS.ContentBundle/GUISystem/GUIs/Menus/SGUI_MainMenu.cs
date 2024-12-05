﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.ContentBundle.Entities.Specials;
using StardustSandbox.ContentBundle.GUISystem.Elements.Textual;
using StardustSandbox.ContentBundle.Localization;
using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.GUISystem;
using StardustSandbox.Core.GUISystem.Events;
using StardustSandbox.Core.Interfaces.General;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus
{
    public sealed partial class SGUI_MainMenu : SGUISystem
    {
        private enum SMainMenuButtonIndex : byte
        {
            Create = 0,
            Play = 1,
            Options = 2,
            Credits = 3,
            Quit = 4
        }

        private Vector2 originalGameTitleElementPosition;

        private const float animationSpeed = 2f;
        private const float animationAmplitude = 10f;
        private const float ButtonAnimationSpeed = 1.5f;
        private const float ButtonAnimationAmplitude = 5f;

        private float animationTime = 0f;

        private Dictionary<SGUILabelElement, Vector2> buttonOriginalPositions;
        private float[] buttonAnimationOffsets;

        private readonly Texture2D gameTitleTexture;
        private readonly Texture2D particleTexture;
        private readonly Texture2D prosceniumCurtainTexture;

        private readonly SWorld world;

        private readonly Song mainMenuSong;

        private readonly Action[] menuButtonActions;
        private readonly string[] menuButtonNames;

        public SGUI_MainMenu(ISGame gameInstance, string identifier, SGUIEvents guiEvents) : base(gameInstance, identifier, guiEvents)
        {
            this.gameTitleTexture = gameInstance.AssetDatabase.GetTexture("game_title_1");
            this.particleTexture = this.SGameInstance.AssetDatabase.GetTexture("particle_1");
            this.prosceniumCurtainTexture = this.SGameInstance.AssetDatabase.GetTexture("miscellany_1");

            this.mainMenuSong = this.SGameInstance.AssetDatabase.GetSong("song_1");

            this.world = gameInstance.World;

            this.menuButtonNames = [
                SLocalization.GUI_Menu_MainMenu_Create,
                SLocalization.GUI_Menu_MainMenu_Play,
                SLocalization.GUI_Menu_MainMenu_Options,
                SLocalization.GUI_Menu_MainMenu_Credits,
                SLocalization.GUI_Menu_MainMenu_Quit
            ];

            this.menuButtonActions = [
                CreateMenuButton,
                PlayMenuButton,
                OptionsMenuButton,
                CreditsMenuButton,
                QuitMenuButton
            ];
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            DisableControls();
            LoadAnimationValues();
            LoadMainMenuWorld();
            LoadMagicCursor();

            SSongEngine.Play(this.mainMenuSong);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateAnimations(gameTime);
            UpdateButtons();
        }

        // =========================================== //
        // Load

        private void DisableControls()
        {
            this.SGameInstance.GameInputController.Disable();
        }

        private void LoadAnimationValues()
        {
            // GameTitle
            this.originalGameTitleElementPosition = this.gameTitleElement.Position;

            // Buttons
            this.buttonOriginalPositions = [];
            this.buttonAnimationOffsets = new float[this.menuButtonElements.Length];

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                this.buttonOriginalPositions[this.menuButtonElements[i]] = this.menuButtonElements[i].Position;
                this.buttonAnimationOffsets[i] = (float)(SRandomMath.GetDouble() * Math.PI * 2);
            }
        }

        private void LoadMainMenuWorld()
        {
            this.world.Resize(SWorldConstants.WORLD_SIZES_TEMPLATE[0]);
            this.world.Reset();
        }

        private void LoadMagicCursor()
        {
            _ = this.SGameInstance.EntityManager.Instantiate(this.SGameInstance.EntityDatabase.GetEntityDescriptor(typeof(SMagicCursorEntity)), null);
        }

        // =========================================== //
        // Update

        private void UpdateAnimations(GameTime gameTime)
        {
            UpdateGameTitleElementAnimation(gameTime);
            UpdateButtonElementsAnimation(gameTime);
        }

        private void UpdateGameTitleElementAnimation(GameTime gameTime)
        {
            this.animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds * animationSpeed;

            float offsetY = (float)Math.Sin(this.animationTime) * animationAmplitude;
            Vector2 newPosition = new(this.originalGameTitleElementPosition.X, this.originalGameTitleElementPosition.Y + offsetY);

            this.gameTitleElement.Position = newPosition;
        }

        private void UpdateButtonElementsAnimation(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement button = this.menuButtonElements[i];
                Vector2 originalPosition = this.buttonOriginalPositions[button];

                this.buttonAnimationOffsets[i] += elapsedTime * ButtonAnimationSpeed;
                float offsetY = (float)Math.Sin(this.buttonAnimationOffsets[i]) * ButtonAnimationAmplitude;

                button.Position = new Vector2(originalPosition.X, originalPosition.Y + offsetY);
            }
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < this.menuButtonElements.Length; i++)
            {
                SGUILabelElement labelElement = this.menuButtonElements[i];

                if (this.GUIEvents.OnMouseClick(labelElement.Position, labelElement.GetStringSize()))
                {
                    this.menuButtonActions[i].Invoke();
                }

                labelElement.Color = this.GUIEvents.OnMouseOver(labelElement.Position, labelElement.GetStringSize()) ? Color.Yellow : Color.White;
            }
        }
    }
}
