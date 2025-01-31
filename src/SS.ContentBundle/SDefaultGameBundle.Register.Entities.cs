using StardustSandbox.ContentBundle.Entities.Living.Animalia.Arthropoda.Insecta.Hymenoptera.Formicidae;
using StardustSandbox.ContentBundle.Entities.Specials;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;

namespace StardustSandbox.ContentBundle
{
    public sealed partial class SDefaultGameBundle
    {
        protected override void OnRegisterEntities(ISGame game, ISEntityDatabase entityDatabase)
        {
            entityDatabase.RegisterEntityDescriptor(new SMagicCursorEntityDescriptor(game, SEntityConstants.MAGIC_CURSOR_IDENTIFIER));
            entityDatabase.RegisterEntityDescriptor(new SAntEntityDescriptor(game, SEntityConstants.ANT_IDENTIFIER));
        }
    }
}