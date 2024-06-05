using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Shared;

namespace SurvivalDemo.EcsCore.Systems
{
    public class ProjectileViewUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedViews _sharedViews;

        private EcsFilter _filter;

        private EcsPool<Transform> _transformPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            _sharedViews = systems.GetShared<SharedData>().Views;

            _filter = world.Filter<Projectile>().Inc<Transform>().End();

            _transformPool = world.GetPool<Transform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Transform transform = ref _transformPool.Get(entity);
                _sharedViews.ProjectileViews[entity].UpdateTransform(transform.Position, transform.Rotation);
            }
        }
    }
}
