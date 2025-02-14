﻿namespace StardustSandbox.GameContent.GUISystem.Helpers.Options
{
    internal abstract class SOption(string name, string description)
    {
        internal string Name => name;
        internal string Description => description;

        internal abstract object GetValue();
        internal abstract void SetValue(object value);
    }
}
