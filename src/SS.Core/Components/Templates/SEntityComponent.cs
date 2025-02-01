using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Entities;
using System.Collections.Generic;

namespace StardustSandbox.Core.Components.Templates
{
    public abstract class SEntityComponent(ISGame gameInstance, SEntity entityInstance) : SComponent(gameInstance)
    {
        protected SEntity SEntityInstance => entityInstance;

        internal void Serialize(IDictionary<string, object> data)
        {
            OnSerialized(data);
        }
        internal void Deserialize(IReadOnlyDictionary<string, object> data)
        {
            OnDeserialized(data);
        }

        protected virtual void OnSerialized(IDictionary<string, object> data) { return; }
        protected virtual void OnDeserialized(IReadOnlyDictionary<string, object> data) { return; }
    }
}
