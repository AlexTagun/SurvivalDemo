using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Shared;
using SurvivalDemo.EcsCore.Views;
using UnityEngine;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class PlayerViewCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedAssets _sharedAssets;
        private SharedViews _sharedViews;

        private EcsFilter _filter;

        private EcsPool<InstantiatePlayerRequest> _requestPool;
        private EcsPool<Transform> _transformPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            SharedData sharedData = systems.GetShared<SharedData>();
            _sharedAssets = sharedData.Assets;
            _sharedViews = sharedData.Views;

            _filter = world.Filter<InstantiatePlayerRequest>().End();

            _requestPool = world.GetPool<InstantiatePlayerRequest>();
            _transformPool = world.GetPool<Transform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Transform transform = ref _transformPool.Get(entity);

                PlayerView playerGo =
                    Object.Instantiate(_sharedAssets.PlayerView, transform.Position, transform.Rotation);
                _sharedViews.PlayerViews.Add(entity, playerGo);
                _requestPool.Del(entity);
            }
        }
    }
}
