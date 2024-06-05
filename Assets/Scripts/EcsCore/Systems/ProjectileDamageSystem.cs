using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class ProjectileDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _enemyFilter;

        private EcsPool<Transform> _transformPool;
        private EcsPool<Health> _healthPool;
        private EcsPool<DestroyProjectileRequest> _requestPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            _filter = world.Filter<Projectile>().Inc<Transform>().End();
            _enemyFilter = world.Filter<Character>().Inc<ControlledByAi>().Inc<Transform>().Inc<Health>().End();

            _transformPool = world.GetPool<Transform>();
            _healthPool = world.GetPool<Health>();
            _requestPool = world.GetPool<DestroyProjectileRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Transform transform = ref _transformPool.Get(entity);
                bool toDestroy = false;

                foreach (int enemyEntity in _enemyFilter)
                {
                    ref Transform enemyTransform = ref _transformPool.Get(enemyEntity);

                    float distance = Vector3.Distance(transform.Position, enemyTransform.Position);

                    if (distance < 1)
                    {
                        ref Health health = ref _healthPool.Get(enemyEntity);
                        health.Current -= 100;
                        toDestroy = true;
                        break;
                    }
                }

                if (toDestroy)
                {
                    _requestPool.Add(entity);
                }
            }
        }
    }
}
