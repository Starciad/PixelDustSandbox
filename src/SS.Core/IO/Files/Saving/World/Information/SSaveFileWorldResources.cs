using StardustSandbox.Core.IO.Files.Saving.World.Information.Resources;

namespace StardustSandbox.Core.IO.Files.Saving.World.Information
{
    public sealed class SSaveFileWorldResources
    {
        public SSaveFileResourceContainer Elements { get; set; } = new();
        public SSaveFileResourceContainer Entities { get; set; } = new();
    }
}
