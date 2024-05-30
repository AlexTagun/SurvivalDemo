using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Configs;
using SurvivalDemo.EcsCore.Views;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly TimeSpan _spawnCooldown = TimeSpan.FromSeconds(1);

        private EcsWorld _world;
        private SharedAssets _sharedAssets;
        private EcsFilter _filter;
        private EcsPool<Unit> _unitPool;
        private EcsPool<ControlledByAi> _controlledByAiPool;
        private TimeSpan _timer;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _sharedAssets = systems.GetShared<SharedAssets>();
            _filter = _world.Filter<Unit>().Inc<ControlledByPlayer>().End();
            _unitPool = _world.GetPool<Unit>();
            _controlledByAiPool = _world.GetPool<ControlledByAi>();

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
                ref Unit unit = ref _unitPool.Get(entity);
                SpawnEnemy(GetRandomPoint(unit));
            }
        }

        private static Vector3 GetRandomPoint(Unit unit)
        {
            Vector2 point = Random.insideUnitCircle;
            point.Normalize();
            Vector3 delta = new(point.x, 0, point.y);
            return unit.Position + delta * 10;
        }

        private void SpawnEnemy(Vector3 spawnPosition)
        {
            int playerEntity = _world.NewEntity();

            ref Unit unit = ref _unitPool.Add(playerEntity);
            _controlledByAiPool.Add(playerEntity);

            EnemyView enemyGo = Object.Instantiate(_sharedAssets.EnemyView, spawnPosition, Quaternion.identity);

            unit.Transform = enemyGo.transform;
            unit.Position = spawnPosition;
            unit.Rotation = Quaternion.identity;
            unit.MoveSpeed = 2.5f;
        }
    }
}
