﻿using StardustSandbox.Core.IO.Handlers;

using System;

namespace StardustSandbox.ContentBundle.GUISystem.GUIs.Menus.WorldsExplorer
{
    internal sealed partial class SGUI_WorldsExplorerMenu
    {
        protected override void OnOpened()
        {
            ReloadButtonAction();
            ChangeWorldsCatalog();
        }

        protected override void OnClosed()
        {
            Array.Clear(this.savedWorldFilesLoaded);
        }

        private void LoadAllLocalSavedWorlds()
        {
            this.savedWorldFilesLoaded = SWorldSavingHandler.LoadAllSavedWorldData(this.SGameInstance.GraphicsManager.GraphicsDevice);
        }
    }
}
