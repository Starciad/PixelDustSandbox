﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Objects;

using System.Collections.Generic;

namespace StardustSandbox.Core.Databases
{
    internal sealed class SAssetDatabase(ISGame gameInstance) : SGameObject(gameInstance), ISAssetDatabase
    {
        private readonly Dictionary<string, Texture2D> textures = [];
        private readonly Dictionary<string, SpriteFont> fonts = [];
        private readonly Dictionary<string, Song> songs = [];
        private readonly Dictionary<string, SoundEffect> sounds = [];
        private readonly Dictionary<string, Effect> effects = [];

        public void RegisterTexture(string identifier, Texture2D value)
        {
            this.textures.Add(identifier, value);
        }

        public void RegisterSpriteFont(string identifier, SpriteFont value)
        {
            this.fonts.Add(identifier, value);
        }

        public void RegisterSong(string identifier, Song value)
        {
            this.songs.Add(identifier, value);
        }

        public void RegisterSoundEffect(string identifier, SoundEffect value)
        {
            this.sounds.Add(identifier, value);
        }

        public void RegisterEffect(string identifier, Effect value)
        {
            this.effects.Add(identifier, value);
        }

        // =============================================================== //

        public Texture2D GetTexture(string name)
        {
            return this.textures[name];
        }

        public SpriteFont GetSpriteFont(string name)
        {
            return this.fonts[name];
        }

        public Song GetSong(string name)
        {
            return this.songs[name];
        }

        public SoundEffect GetSound(string name)
        {
            return this.sounds[name];
        }

        public Effect GetEffect(string name)
        {
            return this.effects[name];
        }

        // =============================================================== //

        internal Effect[] GetAllEffects()
        {
            return [.. this.effects.Values];
        }
    }
}