using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Shared;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyViewUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedViews _sharedViews;

        private EcsFilter _filter;

        private EcsPool<Transform> _transformPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _sharedViews = systems.GetShared<SharedData>().Views;

            _filter = world.Filter<Character>().Inc<Transform>().Inc<ControlledByAi>().End();

            _transformPool = world.GetPool<Transform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                Transform transform = _transformPool.Get(entity);
                _sharedViews.EnemyViews[entity].UpdateTransform(transform.Position, transform.Rotation);
            }
        }
    }
}
