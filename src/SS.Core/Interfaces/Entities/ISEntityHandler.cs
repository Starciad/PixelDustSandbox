using StardustSandbox.Core.Entities;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Interfaces.Entities
{
    public interface ISEntityHandler
    {
        int ActiveEntitiesCount { get; }
        IEnumerable<SEntity> ActiveEntities { get; }

        SEntity InstantiateEntity(string entityIdentifier, Action<SEntity> entityConfigurationAction);
        bool TryInstantiateEntity(string entityIdentifier, Action<SEntity> entityConfigurationAction, out SEntity entity);

        void RemoveEntity(SEntity entity);
        void DestroyEntity(SEntity entity);

        void RemoveAllEntities();
        void DestroyAllEntities();
    }
}
