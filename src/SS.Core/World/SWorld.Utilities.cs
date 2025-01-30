using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Interfaces.Collections;
using StardustSandbox.Core.IO.Files.Saving;
using StardustSandbox.Core.IO.Files.Saving.World.Content.Entities;
using StardustSandbox.Core.IO.Files.Saving.World.Content.Slots;
using StardustSandbox.Core.IO.Files.Saving.World.Information;
using StardustSandbox.Core.IO.Files.Saving.World.Information.Resources;
using StardustSandbox.Core.Mathematics.Primitives;
using StardustSandbox.Core.World.Slots;

namespace StardustSandbox.Core.World
{
    internal sealed partial class SWorld
    {
        #region System
        public void StartNew()
        {
            StartNew(this.Infos.Size);
        }
        public void StartNew(SSize2 size)
        {
            this.IsActive = true;
            this.IsVisible = true;

            if (this.Infos.Size != size)
            {
                Resize(size);
            }

            Reset();
        }
        public void Resize(SSize2 size)
        {
            DestroyWorldSlots();

            this.Infos.Size = size;
            this.slots = new SWorldSlot[size.Width, size.Height];

            InstantiateWorldSlots();
        }
        public void Reload()
        {
            if (this.currentlySelectedWorldSaveFile != null)
            {
                LoadFromWorldSaveFile(this.currentlySelectedWorldSaveFile);
            }
            else
            {
                Clear();
            }
        }
        #endregion

        #region Load From World File
        public void LoadFromWorldSaveFile(SSaveFile worldSaveFile)
        {
            this.SGameInstance.GameManager.GameState.IsSimulationPaused = true;

            // World
            StartNew(worldSaveFile.World.Information.Size);

            // Cache
            this.currentlySelectedWorldSaveFile = worldSaveFile;

            // Metadata
            this.Infos.Identifier = worldSaveFile.Header.Metadata.Identifier;
            this.Infos.Name = worldSaveFile.Header.Metadata.Name;
            this.Infos.Description = worldSaveFile.Header.Metadata.Description;

            // Time
            this.Time.SetTime(worldSaveFile.World.Environment.Time.CurrentTime);
            this.Time.IsFrozen = worldSaveFile.World.Environment.Time.IsFrozen;

            // Load Slots
            foreach (SSaveFileWorldSlot worldSlot in worldSaveFile.World.Content.Slots)
            {
                if (worldSlot.ForegroundLayer != null)
                {
                    LoadWorldSlotLayer(worldSaveFile.World.Resources.Elements, SWorldLayer.Foreground, worldSlot.Position, worldSlot.ForegroundLayer);
                }

                if (worldSlot.BackgroundLayer != null)
                {
                    LoadWorldSlotLayer(worldSaveFile.World.Resources.Elements, SWorldLayer.Background, worldSlot.Position, worldSlot.BackgroundLayer);
                }
            }

            // Load Entities
            foreach (SSaveFileEntity entity in worldSaveFile.World.Content.Entities)
            {
                LoadEntity(worldSaveFile.World.Resources.Entities, entity);
            }
        }
        private void LoadWorldSlotLayer(SSaveFileResourceContainer resourceContainer, SWorldLayer worldLayer, Point position, SSaveFileWorldSlotLayer saveFileWorldSlotLayer)
        {
            InstantiateElement(position, worldLayer, resourceContainer.FindValueByIndex(saveFileWorldSlotLayer.ElementIndex));

            SWorldSlot worldSlot = GetWorldSlot(position);

            worldSlot.SetTemperatureValue(worldLayer, saveFileWorldSlotLayer.Temperature);
            worldSlot.SetFreeFalling(worldLayer, saveFileWorldSlotLayer.FreeFalling);
            worldSlot.SetColorModifier(worldLayer, saveFileWorldSlotLayer.ColorModifier);
            worldSlot.SetStoredElement(worldLayer, this.SGameInstance.ElementDatabase.GetElementByIdentifier(resourceContainer.FindValueByIndex(saveFileWorldSlotLayer.StoredElementIndex)));
        }
        private void LoadEntity(SSaveFileResourceContainer resourceContainer, SSaveFileEntity saveFileEntity)
        {
            _ = InstantiateEntity(resourceContainer.FindValueByIndex(saveFileEntity.EntityIndex), (SEntity entity) =>
            {
                entity.Deserialize(saveFileEntity.Data);
            });
        }
        #endregion

        #region Clear
        public void Clear()
        {
            ClearSlots();
            ClearEntities();
        }
        private void ClearSlots()
        {
            if (this.slots == null)
            {
                return;
            }

            for (int x = 0; x < this.Infos.Size.Width; x++)
            {
                for (int y = 0; y < this.Infos.Size.Height; y++)
                {
                    if (IsEmptyWorldSlot(new(x, y)))
                    {
                        continue;
                    }

                    RemoveElement(new(x, y), SWorldLayer.Foreground);
                    RemoveElement(new(x, y), SWorldLayer.Background);
                }
            }
        }
        private void ClearEntities()
        {
            RemoveAllEntities();
        }
        #endregion

        #region Build and Destroy World
        private void InstantiateWorldSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.Infos.Size.Width; x++)
                {
                    this.slots[x, y] = this.worldSlotsPool.TryGet(out ISPoolableObject value) ? (SWorldSlot)value : new();
                }
            }
        }
        private void DestroyWorldSlots()
        {
            if (this.slots == null || this.slots.Length == 0)
            {
                return;
            }

            for (int y = 0; y < this.Infos.Size.Height; y++)
            {
                for (int x = 0; x < this.Infos.Size.Width; x++)
                {
                    if (this.slots[x, y] == null)
                    {
                        continue;
                    }

                    this.worldSlotsPool.Add(this.slots[x, y]);
                    this.slots[x, y] = null;
                }
            }
        }
        #endregion

        #region Tools
        public void SetSpeed(SSimulationSpeed speed)
        {
            switch (speed)
            {
                case SSimulationSpeed.Normal:
                    this.Time.SecondsPerFrames = STimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
                    this.Simulation.SetSpeed(SSimulationSpeed.Normal);
                    break;

                case SSimulationSpeed.Fast:
                    this.Time.SecondsPerFrames = STimeConstants.DEFAULT_FAST_SECONDS_PER_FRAMES;
                    this.Simulation.SetSpeed(SSimulationSpeed.Fast);
                    break;

                case SSimulationSpeed.VeryFast:
                    this.Time.SecondsPerFrames = STimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
                    this.Simulation.SetSpeed(SSimulationSpeed.VeryFast);
                    break;

                default:
                    this.Time.SecondsPerFrames = STimeConstants.DEFAULT_NORMAL_SECONDS_PER_FRAMES;
                    this.Simulation.SetSpeed(SSimulationSpeed.Normal);
                    break;
            }
        }
        public bool InsideTheWorldDimensions(Point position)
        {
            return position.X >= 0 && position.X < this.Infos.Size.Width &&
                   position.Y >= 0 && position.Y < this.Infos.Size.Height;
        }
        #endregion
    }
}
