﻿using StardustSandbox.ContentBundle.Enums.GUISystem.Tools.InputSystem;

namespace StardustSandbox.ContentBundle.GUISystem.Helpers.Tools.InputSystem
{
    internal sealed class STextValidationState
    {
        internal SValidationStatus Status { get; set; }
        internal string Message { get; set; }

        internal STextValidationState()
        {
            this.Status = SValidationStatus.Success;
            this.Message = string.Empty;
        }
    }
}
