﻿using Microsoft.Xna.Framework.Content;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.Core.Plugins
{
    public abstract partial class SPluginBuilder
    {
        internal void Initialize(ISGame game, ContentManager contentManager)
        {
            OnRegisterAssets(game, contentManager, game.AssetDatabase);
            OnRegisterElements(game, game.ElementDatabase);
            OnRegisterCatalog(game, game.CatalogDatabase);
            OnRegisterGUIs(game, game.GUIDatabase);
            OnRegisterBackgrounds(game, game.BackgroundDatabase);
            OnRegisterEntities(game, game.EntityDatabase);
        }

        protected virtual void OnRegisterAssets(ISGame game, ContentManager contentManager, ISAssetDatabase assetDatabase) { return; }
        protected virtual void OnRegisterElements(ISGame game, ISElementDatabase elementDatabase) { return; }
        protected virtual void OnRegisterGUIs(ISGame game, ISGUIDatabase guiDatabase) { return; }
        protected virtual void OnRegisterCatalog(ISGame game, ISCatalogDatabase catalogDatabase) { return; }
        protected virtual void OnRegisterBackgrounds(ISGame game, ISBackgroundDatabase backgroundDatabase) { return; }
        protected virtual void OnRegisterEntities(ISGame game, ISEntityDatabase entityDatabase) { return; }
    }
}
