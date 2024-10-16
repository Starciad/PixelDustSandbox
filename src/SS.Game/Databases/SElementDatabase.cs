﻿using StardustSandbox.Game.Elements;
using StardustSandbox.Game.Objects;

using System;
using System.Collections.Generic;
using System.Linq;


namespace StardustSandbox.Game.Databases
{
    public sealed class SElementDatabase : SGameObject
    {
        private List<SElement> _registeredElements = [];

        public SElementDatabase(SGame gameInstance) : base(gameInstance)
        {

        }

        protected override void OnInitialize()
        {
            this._registeredElements = [.. this._registeredElements.OrderBy(x => x.Id)];
        }

        internal void RegisterElement(SElement element)
        {
            element.Initialize();

            this._registeredElements.Add(element);
        }

        public T GetElementById<T>(uint id) where T : SElement
        {
            return (T)GetElementById(id);
        }

        public SElement GetElementById(uint id)
        {
            return this._registeredElements[(int)id];
        }

        public uint GetIdOfElementType<T>() where T : SElement
        {
            return GetIdOfElementType(typeof(T));
        }

        public uint GetIdOfElementType(Type type)
        {
            return GetElementByType(type).Id;
        }

        public T GetElementByType<T>() where T : SElement
        {
            return (T)GetElementByType(typeof(T));
        }

        public SElement GetElementByType(Type type)
        {
            return this._registeredElements.Find(x => x.GetType() == type);
        }
    }
}
