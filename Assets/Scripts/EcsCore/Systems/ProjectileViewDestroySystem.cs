using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Shared;
using SurvivalDemo.EcsCore.Views;
using UnityEngine;

namespace SurvivalDemo.EcsCore.Systems
{
    public class ProjectileViewDestroySystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedViews _sharedViews;

        private EcsWorld _world;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _sharedViews = systems.GetShared<SharedData>().Views;

            _world = systems.GetWorld();
            _filter = _world.Filter<DestroyProjectileRequest>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                _sharedViews.ProjectileViews.Remove(entity, out ProjectileView view);
                Object.Destroy(view.gameObject);
                _world.DelEntity(entity);
            }
        }
    }
}
