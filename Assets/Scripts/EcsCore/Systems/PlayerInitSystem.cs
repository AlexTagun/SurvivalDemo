using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Configs;
using SurvivalDemo.EcsCore.Views;
using SurvivalDemo.Gameplay.SpawnPoints;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SurvivalDemo.EcsCore.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly PlayerSpawnPoint _spawnPoint;

        private EcsPool<Unit> _unitPool;
        private EcsPool<ControlledByPlayer> _controlledByPlayerPool;
        private SharedAssets _sharedAssets;

        public PlayerInitSystem(PlayerSpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedAssets = systems.GetShared<SharedAssets>();
            int playerEntity = world.NewEntity();

            _unitPool = world.GetPool<Unit>();
            _controlledByPlayerPool = world.GetPool<ControlledByPlayer>();

            ref Unit unit = ref _unitPool.Add(playerEntity);
            _controlledByPlayerPool.Add(playerEntity);

            Transform transform = _spawnPoint.transform;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            PlayerView playerGo = Object.Instantiate(_sharedAssets.PlayerView, position, rotation);

            unit.Transform = playerGo.transform;
            unit.Position = position;
            unit.Rotation = rotation;
            unit.MoveSpeed = 3f;
        }
    }
}
