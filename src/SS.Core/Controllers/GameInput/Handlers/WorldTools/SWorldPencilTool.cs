using Microsoft.Xna.Framework;

using StardustSandbox.Core.Components.Common.Entities;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Entities;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Mathematics;

using System.Collections.Generic;
using System.Linq;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.WorldTools
{
    internal sealed class SWorldPencilTool : SWorldTool
    {
        internal SWorldPencilTool(SWorldHandler worldHandler, ISGame gameInstance, SSimulationPen simulationPen) : base(worldHandler, gameInstance, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position)
        {
            IEnumerable<Point> targetPoints = this.simulationPen.GetShapePoints(position);

            switch (contentType)
            {
                case SItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case SWorldModificationType.Adding:
                            DrawElements(this.gameInstance.ElementDatabase.GetElementByIdentifier(referencedItemIdentifier), targetPoints);
                            break;

                        case SWorldModificationType.Removing:
                            EraseElements(targetPoints);
                            break;

                        default:
                            break;
                    }

                    break;

                case SItemContentType.Entity:
                    switch (worldModificationType)
                    {
                        case SWorldModificationType.Adding:
                            DrawEntities(referencedItemIdentifier, SWorldMath.ToGlobalPosition(position));
                            break;

                        case SWorldModificationType.Removing:
                            EraseEntities(SWorldMath.ToGlobalPosition(position));
                            break;

                        default:
                            break;
                    }
                    break;

                case SItemContentType.Tool:
                    switch (worldModificationType)
                    {
                        case SWorldModificationType.Adding:
                            ExecuteTool(referencedItemIdentifier, targetPoints);
                            break;

                        case SWorldModificationType.Removing:
                            EraseElements(targetPoints);
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        // ============================================ //
        // Elements

        private void DrawElements(ISElement element, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.InstantiateElement(position, this.simulationPen.Layer, element);
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.RemoveElement(position, this.simulationPen.Layer);
            }
        }

        // ============================================ //
        // Entities

        private void DrawEntities(string entityIdentifier, Vector2 position)
        {
            _ = this.world.InstantiateEntity(entityIdentifier, (entity) =>
            {
                STransformComponent transformComponent = entity.ComponentContainer.GetComponent<STransformComponent>();

                transformComponent.Position = position;
            });
        }

        private void EraseEntities(Vector2 position)
        {
            foreach (SEntity entity in this.world.ActiveEntities.ToList())
            {
                if (!entity.ComponentContainer.TryGetComponent(out STransformComponent transformComponent))
                {
                    continue;
                }

                if (Vector2.Distance(position, transformComponent.Position) < this.simulationPen.Size * SWorldConstants.GRID_SIZE + (SWorldConstants.GRID_SIZE / 2))
                {
                    this.world.RemoveEntity(entity);
                }
            }
        }

        // ============================================ //
        // Tools

        private void ExecuteTool(string identifier, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.worldHandler.ToolContext.Update(position, this.simulationPen.Layer);
                this.gameInstance.ToolDatabase.GetToolByIdentifier(identifier).Execute(this.worldHandler.ToolContext);
            }
        }
    }
}
