using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Configs;
using SurvivalDemo.EcsCore.Views;
using SurvivalDemo.Gameplay.SpawnPoints;
using UnityEngine;

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
            PlayerView playerGo = Object.Instantiate(_sharedAssets.PlayerView, transform.position, transform.rotation);

            unit.Transform = playerGo.transform;
            unit.Position = Vector3.zero;
            unit.Rotation = Quaternion.identity;
            unit.MoveSpeed = 3f;
        }
    }
}
