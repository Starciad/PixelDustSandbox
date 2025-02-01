using MessagePack;

using StardustSandbox.Core.Entities;
using StardustSandbox.Core.IO.Files.Saving.World.Information.Resources;

using System.Collections.Generic;

namespace StardustSandbox.Core.IO.Files.Saving.World.Content.Entities
{
    [MessagePackObject]
    public sealed class SSaveFileEntity
    {
        [Key(0)] public uint EntityIndex { get; set; }
        [Key(1)] public IEnumerable<object[]> ComponentData { get; set; }

        public SSaveFileEntity()
        {

        }

        public SSaveFileEntity(SSaveFileResourceContainer container, SEntity entity)
        {
            this.EntityIndex = container.FindIndexByValue(entity.Descriptor.Identifier);
            this.ComponentData = entity.Serialize();
        }
    }
}
