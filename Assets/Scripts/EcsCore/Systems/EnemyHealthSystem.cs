using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyHealthSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;

        private EcsPool<DestroyEnemyRequest> _requestPool;
        private EcsPool<Health> _healthPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<Character>().Inc<Health>().Inc<ControlledByAi>().End();

            _healthPool = _world.GetPool<Health>();
            _requestPool = _world.GetPool<DestroyEnemyRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                Health health = _healthPool.Get(entity);

                if (health.Current <= 0)
                {
                    _requestPool.Add(entity);
                }
            }
        }
    }
}
