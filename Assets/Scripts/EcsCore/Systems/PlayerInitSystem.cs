using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.Gameplay.SpawnPoints;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        private readonly PlayerSpawnPoint _spawnPoint;

        private EcsPool<Character> _characterPool;
        private EcsPool<MoveSpeed> _moveSpeedPool;
        private EcsPool<Health> _healthPool;
        private EcsPool<ControlledByPlayer> _controlledByPlayerPool;
        private EcsPool<Transform> _transformPool;
        private EcsPool<InstantiatePlayerRequest> _requestPool;

        public PlayerInitSystem(PlayerSpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            _characterPool = world.GetPool<Character>();
            _transformPool = world.GetPool<Transform>();
            _moveSpeedPool = world.GetPool<MoveSpeed>();
            _requestPool = world.GetPool<InstantiatePlayerRequest>();
            _healthPool = world.GetPool<Health>();
            _controlledByPlayerPool = world.GetPool<ControlledByPlayer>();

            int playerEntity = world.NewEntity();

            _characterPool.Add(playerEntity);
            _controlledByPlayerPool.Add(playerEntity);

            ref Transform transform = ref _transformPool.Add(playerEntity);
            transform.Position = _spawnPoint.Position;
            transform.Rotation = _spawnPoint.Rotation;

            ref MoveSpeed moveSpeed = ref _moveSpeedPool.Add(playerEntity);
            moveSpeed.Value = 3;

            ref Health health = ref _healthPool.Add(playerEntity);
            health.Max = 100;
            health.Current = 100;

            _requestPool.Add(playerEntity);
        }
    }
}
