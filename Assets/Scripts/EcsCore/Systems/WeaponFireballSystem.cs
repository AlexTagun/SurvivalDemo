using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class WeaponFireballSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float ProjectileSpeed = 5;
        private static readonly TimeSpan Cooldown = TimeSpan.FromSeconds(1);
        private readonly Dictionary<int, TimeSpan> _cooldownByEntities = new();

        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsFilter _enemyFilter;

        private EcsPool<Transform> _transformPool;
        private EcsPool<CreateProjectileRequest> _requestPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _filter = _world.Filter<Character>().Inc<WeaponFireball>().Inc<Transform>().End();
            _enemyFilter = _world.Filter<Character>().Inc<ControlledByAi>().Inc<Transform>().End();

            _transformPool = _world.GetPool<Transform>();
            _requestPool = _world.GetPool<CreateProjectileRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_enemyFilter.GetEntitiesCount() <= 0)
            {
                return;
            }

            foreach (int entity in _filter)
            {
                _cooldownByEntities.TryAdd(entity, Cooldown);
                _cooldownByEntities[entity] -= TimeSpan.FromSeconds(Time.deltaTime);

                if (_cooldownByEntities[entity] > TimeSpan.FromSeconds(0))
                {
                    continue;
                }

                _cooldownByEntities[entity] = Cooldown;
                CreateRequest(entity);
            }
        }

        private void CreateRequest(int playerEntity)
        {
            int requestEntity = _world.NewEntity();
            Transform transform = _transformPool.Get(playerEntity);

            ref CreateProjectileRequest request = ref _requestPool.Add(requestEntity);
            request.Position = transform.Position;
            request.Rotation = GetProjectileRotation(playerEntity);
            request.MoveSpeed = ProjectileSpeed;
        }

        private Quaternion GetProjectileRotation(int playerEntity)
        {
            int nearestEnemyEntity = GetNearestEnemyEntity(playerEntity);
            Transform playerTransform = _transformPool.Get(playerEntity);
            Transform enemyTransform = _transformPool.Get(nearestEnemyEntity);

            Vector3 direction = enemyTransform.Position - playerTransform.Position;
            direction.Normalize();
            return Quaternion.LookRotation(direction);
        }

        private int GetNearestEnemyEntity(int playerEntity)
        {
            Transform playerTransform = _transformPool.Get(playerEntity);
            int nearestEnemyEntity = -1;
            float minDistance = float.MaxValue;

            foreach (int enemyEntity in _enemyFilter)
            {
                Transform enemyTransform = _transformPool.Get(enemyEntity);

                float distance = Vector3.Distance(playerTransform.Position, enemyTransform.Position);

                if (distance >= minDistance)
                {
                    continue;
                }

                minDistance = distance;
                nearestEnemyEntity = enemyEntity;
            }

            return nearestEnemyEntity;
        }
    }
}
