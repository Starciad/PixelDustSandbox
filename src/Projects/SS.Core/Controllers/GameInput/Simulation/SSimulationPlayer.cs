﻿using StardustSandbox.Core.Catalog;

namespace StardustSandbox.Core.Controllers.GameInput.Simulation
{
    public sealed class SSimulationPlayer
    {
        public SItem SelectedItem => this.selectedItem;
        public float MovementSpeed => this.movementSpeed;
        public bool CanModifyEnvironment { get; set; }

        private readonly float movementSpeed = 10;
        private SItem selectedItem;

        public void SelectItem(SItem item)
        {
            this.selectedItem = item;
        }
    }
}
