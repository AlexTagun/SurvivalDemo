using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly TimeSpan _spawnCooldown = TimeSpan.FromSeconds(1);

        private EcsWorld _world;
        private EcsFilter _filter;

        private EcsPool<Character> _characterPool;
        private EcsPool<Transform> _transformPool;
        private EcsPool<ControlledByAi> _controlledByAiPool;
        private EcsPool<Health> _healthPool;
        private EcsPool<MoveSpeed> _moveSpeedPool;
        private EcsPool<InstantiateEnemyRequest> _requestPool;

        private TimeSpan _timer;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Character>().Inc<Transform>().Inc<ControlledByPlayer>().End();

            _characterPool = _world.GetPool<Character>();
            _transformPool = _world.GetPool<Transform>();
            _controlledByAiPool = _world.GetPool<ControlledByAi>();
            _healthPool = _world.GetPool<Health>();
            _moveSpeedPool = _world.GetPool<MoveSpeed>();
            _requestPool = _world.GetPool<InstantiateEnemyRequest>();

            _timer = _spawnCooldown;
        }

        public void Run(IEcsSystems systems)
        {
            _timer -= TimeSpan.FromSeconds(Time.deltaTime);

            if (_timer.TotalSeconds > 0)
            {
                return;
            }

            _timer = _spawnCooldown;

            foreach (int entity in _filter)
            {
                ref Transform transform = ref _transformPool.Get(entity);
                SpawnEnemy(GetRandomPoint(transform));
            }
        }

        private static Vector3 GetRandomPoint(Transform transform)
        {
            Vector2 point = Random.insideUnitCircle;
            point.Normalize();
            Vector3 delta = new(point.x, 0, point.y);
            return transform.Position + delta * 10;
        }

        private void SpawnEnemy(Vector3 spawnPosition)
        {
            int entity = _world.NewEntity();

            _characterPool.Add(entity);
            ref Transform transform = ref _transformPool.Add(entity);
            ref MoveSpeed moveSpeed = ref _moveSpeedPool.Add(entity);
            _controlledByAiPool.Add(entity);

            transform.Position = spawnPosition;
            transform.Rotation = Quaternion.identity;
            moveSpeed.Value = 2.5f;

            ref Health health = ref _healthPool.Add(entity);
            health.Max = 100;
            health.Current = 100;

            _requestPool.Add(entity);
        }
    }
}
