﻿using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Shared;
using SurvivalDemo.EcsCore.Views;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyViewUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedViews _sharedViews;
        private EcsPool<Transform> _transformPool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            SharedData sharedData = systems.GetShared<SharedData>();
            _sharedViews = sharedData.Views;
            _transformPool = world.GetPool<Transform>();

            _filter = world.Filter<Character>().Inc<Transform>().Inc<ControlledByAi>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Transform transform = ref _transformPool.Get(entity);

                if (_sharedViews.EnemyViews.TryGetValue(entity, out EnemyView enemyView))
                {
                    enemyView.UpdateTransform(transform.Position, transform.Rotation);
                }
            }
        }
    }
}
