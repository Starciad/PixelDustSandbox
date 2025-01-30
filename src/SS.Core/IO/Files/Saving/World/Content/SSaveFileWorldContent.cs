using StardustSandbox.Core.IO.Files.Saving.World.Content.Entities;
using StardustSandbox.Core.IO.Files.Saving.World.Content.Slots;

using System.Collections.Generic;

namespace StardustSandbox.Core.IO.Files.Saving.World.Content
{
    public sealed class SSaveFileWorldContent
    {
        public IEnumerable<SSaveFileWorldSlot> Slots { get; set; } = null;
        public IEnumerable<SSaveFileEntity> Entities { get; set; } = null;
    }
}
