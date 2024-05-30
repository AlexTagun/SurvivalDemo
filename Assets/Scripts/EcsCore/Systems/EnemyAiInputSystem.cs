using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyAiInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsFilter _enemyFilter;
        private EcsPool<Transform> _transformPool;
        private EcsPool<MoveCommand> _moveCommandPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<Character>().Inc<ControlledByPlayer>().Inc<Transform>().End();
            _enemyFilter = world.Filter<Character>().Inc<ControlledByAi>().Inc<Transform>().End();
            _transformPool = world.GetPool<Transform>();
            _moveCommandPool = world.GetPool<MoveCommand>();
        }

        public void Run(IEcsSystems systems)
        {
            int playerEntity = -1;

            foreach (int entity in _playerFilter)
            {
                playerEntity = entity;
                break;
            }

            if (playerEntity < 0)
            {
                return;
            }

            ref Transform playerTransform = ref _transformPool.Get(playerEntity);


            foreach (int entity in _enemyFilter)
            {
                ref Transform enemyTransform = ref _transformPool.Get(entity);
                ref MoveCommand moveCmd = ref _moveCommandPool.Add(entity);

                Vector3 direction = playerTransform.Position - enemyTransform.Position;
                direction.Normalize();

                moveCmd.Forward = direction.z;
                moveCmd.Right = direction.x;
            }
        }
    }
}
