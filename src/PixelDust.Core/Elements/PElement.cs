﻿using Microsoft.Xna.Framework;

using PixelDust.Core.Mathematics;
using PixelDust.Core.Worlding;

namespace PixelDust.Core.Elements
{
    /// <summary>
    /// Base class for defining PixelDust elements, together with their particularities, configurations and behaviors.
    /// </summary>
    public abstract class PElement
    {
        #region Settings (Header)

        /// <summary>
        /// The element name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The description of the element.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// The category the element is in.
        /// </summary>
        public string Category { get; protected set; }

        /// <summary>
        /// The id number of the element.
        /// </summary>
        public byte Id { get; internal set; }

        #endregion

        #region Settings (Defaults)

        /// <summary>
        /// The default dispersion rate set to how much liquids can move in a given range.
        /// </summary>
        public int DefaultDispersionRate { get; protected set; } = 1;

        #endregion

        #region Settings (Enables)

        /// <summary>
        /// Enables the default behavior of the respective element executed by the <see cref="OnBehaviourStep"/> method.
        /// </summary>
        /// <remarks>
        /// The default behavior of the element can vary according to its associated type, and can change drastically between <see cref="PSolid"/>, <see cref="PGas"/> and <see cref="PLiquid"/> and among other special elements.
        /// </remarks>
        public bool EnableDefaultBehaviour { get; protected set; } = true;

        /// <summary>
        /// Enables the <see cref="OnNeighbors(ValueTuple{Vector2, PWorldSlot}[], int)"/> method that searches for the element's closest neighbors in a 3x3 area at each update.
        /// </summary>
        public bool EnableNeighborsAction { get; protected set; }

        #endregion

        #region Helpers

        /// <summary>
        /// Set of render associated with this element.
        /// </summary>
        public PElementRender Render { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected PElementContext Context { get; private set; }
        
        #endregion

        // ======= //

        #region Engine

        /// <summary>
        /// Builds the element.
        /// </summary>
        internal void Build()
        {
            Render = new();
            OnSettings();
        }

        /// <summary>
        /// Updates the element's behavior.
        /// </summary>
        internal void Update(PElementContext updateContext)
        {
            Context = updateContext;

            Render.Update();
            OnUpdate();
            
            if (EnableNeighborsAction && Context.TryGetNeighbors(Context.Position, out (Vector2Int, PWorldSlot)[] neighbors))
                OnNeighbors(neighbors, neighbors.Length);
        }

        /// <summary>
        /// Draws the element's associated tile sprites.
        /// </summary>
        internal void Draw(PElementContext drawContext)
        {
            Render.Draw(drawContext);
        }

        /// <summary>
        /// Executes various steps in the element's behavior.
        /// </summary>
        internal void Steps(PElementContext updateContext)
        {
            Context = updateContext;

            OnBeforeStep();
            OnStep();
            if (EnableDefaultBehaviour) { OnBehaviourStep(); }
            OnAfterStep();
        }

        #endregion

        // ======= //

        #region Configurations

        /// <summary>
        /// Invoked during the build process to configure settings.
        /// </summary>
        protected virtual void OnSettings() { return; }

        /// <summary>
        /// Invoked during each update cycle to handle element-specific updates.
        /// </summary>
        protected virtual void OnUpdate() { return; }

        // Steps
        /// <summary>
        /// Invoked before the element's main step execution.
        /// </summary>
        protected virtual void OnBeforeStep() { return; }

        /// <summary>
        /// Invoked during the main step execution of the element.
        /// </summary>
        protected virtual void OnStep() { return; }

        /// <summary>
        /// Invoked after the element's main step execution.
        /// </summary>
        protected virtual void OnAfterStep() { return; }

        /// <summary>
        /// Invoked to handle the element's default behavior step.
        /// </summary>
        internal virtual void OnBehaviourStep() { return; }

        #endregion

        // ======= //

        #region Events

        /// <summary>
        /// Invoked to handle neighbor-related logic.
        /// </summary>
        /// <param name="neighbors">Array of neighboring elements.</param>
        /// <param name="length">Number of neighbors.</param>
        protected virtual void OnNeighbors((Vector2Int, PWorldSlot)[] neighbors, int length) { return; }

        #endregion
    }
}
