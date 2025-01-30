using MessagePack;

using StardustSandbox.Core.Entities;
using StardustSandbox.Core.IO.Files.Saving.World.Information;
using StardustSandbox.Core.World.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.IO.Files.Saving.World.Content.Entities
{
    [MessagePackObject]
    public sealed class SSaveFileEntity
    {
        [Key(0)] public uint EntityIndex { get; set; }
        [Key(1)] public IReadOnlyDictionary<string, object> Data { get; set; }

        public SSaveFileEntity()
        {

        }

        public SSaveFileEntity(SSaveFileWorldResources resources, SEntity entity)
        {
            this.EntityIndex = resources.Elements.FindIndexByValue(entity.Descriptor.Identifier);
            this.Data = entity.Serialize();
        }
    }
}
