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
        private EcsPool<Transform> _transformPool;
        private EcsPool<ControlledByPlayer> _controlledByPlayerPool;
        private EcsPool<MoveSpeed> _moveSpeedPool;
        private EcsPool<Health> _healthPool;
        private EcsPool<InstantiatePlayerRequest> _requestPool;
        private EcsPool<WeaponFireball> _weaponFireballPool;

        public PlayerInitSystem(PlayerSpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            _characterPool = world.GetPool<Character>();
            _transformPool = world.GetPool<Transform>();
            _controlledByPlayerPool = world.GetPool<ControlledByPlayer>();
            _moveSpeedPool = world.GetPool<MoveSpeed>();
            _healthPool = world.GetPool<Health>();
            _requestPool = world.GetPool<InstantiatePlayerRequest>();
            _weaponFireballPool = world.GetPool<WeaponFireball>();

            int entity = world.NewEntity();

            _characterPool.Add(entity);

            ref Transform transform = ref _transformPool.Add(entity);
            transform.Position = _spawnPoint.Position;
            transform.Rotation = _spawnPoint.Rotation;

            _controlledByPlayerPool.Add(entity);

            ref MoveSpeed moveSpeed = ref _moveSpeedPool.Add(entity);
            moveSpeed.Value = 3;

            ref Health health = ref _healthPool.Add(entity);
            health.Max = 100;
            health.Current = 100;

            _requestPool.Add(entity);
            _weaponFireballPool.Add(entity);
        }
    }
}
