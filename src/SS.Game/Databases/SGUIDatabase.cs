﻿using StardustSandbox.Game.GameContent.GUI.Content.Hud;
using StardustSandbox.Game.GameContent.GUI.Content.Menus.ItemExplorer;
using StardustSandbox.Game.GUI;
using StardustSandbox.Game.GUI.Events;
using StardustSandbox.Game.Objects;

using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.Game.Databases
{
    public sealed class SGUIDatabase : SGameObject
    {
        public IReadOnlyList<SGUISystem> RegisteredGUIs => this._registeredGUIs;

        private List<SGUISystem> _registeredGUIs = [];

        public SGUIDatabase(SGame gameInstance) : base(gameInstance)
        {

        }

        protected override void OnInitialize()
        {
            RegisterGUISystem(new SGUI_HUD(this.SGameInstance), this.SGameInstance.GUIManager.GUIEvents);
            RegisterGUISystem(new SGUI_ItemExplorer(this.SGameInstance), this.SGameInstance.GUIManager.GUIEvents);

            this._registeredGUIs.ForEach(x => x.Initialize());
            this._registeredGUIs = [.. this._registeredGUIs.OrderBy(x => x.ZIndex)];
        }

        private void RegisterGUISystem(SGUISystem guiSystem, SGUIEvents guiEvents)
        {
            guiSystem.Configure(guiEvents);
            this._registeredGUIs.Add(guiSystem);
        }

        public SGUISystem Find(string name)
        {
            return this._registeredGUIs.Find(x => x.Name == name);
        }
    }
}
