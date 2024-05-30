using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Shared;
using SurvivalDemo.EcsCore.Views;
using UnityEngine;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyViewCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedAssets _sharedAssets;
        private SharedViews _sharedViews;
        private EcsFilter _filter;
        private EcsPool<InstantiateEnemyRequest> _requestPool;
        private EcsPool<Transform> _transformPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            SharedData sharedData = systems.GetShared<SharedData>();
            _sharedAssets = sharedData.Assets;
            _sharedViews = sharedData.Views;

            _requestPool = world.GetPool<InstantiateEnemyRequest>();
            _transformPool = world.GetPool<Transform>();

            _filter = world.Filter<InstantiateEnemyRequest>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Transform transform = ref _transformPool.Get(entity);

                EnemyView enemyGo =
                    Object.Instantiate(_sharedAssets.EnemyView, transform.Position, transform.Rotation);
                _sharedViews.EnemyViews.Add(entity, enemyGo);
                _requestPool.Del(entity);
            }
        }
    }
}
