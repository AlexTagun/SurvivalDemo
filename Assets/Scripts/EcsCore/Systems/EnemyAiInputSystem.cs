using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;

namespace SurvivalDemo.EcsCore.Systems
{
    public class EnemyAiInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _playerFilter;
        private EcsFilter _enemyFilter;
        private EcsPool<Unit> _unitPool;
        private EcsPool<MoveCommand> _moveCommandPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _playerFilter = world.Filter<Unit>().Inc<ControlledByPlayer>().End();
            _enemyFilter = world.Filter<Unit>().Inc<ControlledByAi>().End();
            _unitPool = world.GetPool<Unit>();
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

            ref Unit playerUnit = ref _unitPool.Get(playerEntity);


            foreach (int entity in _enemyFilter)
            {
                ref Unit enemyUnit = ref _unitPool.Get(entity);
                ref MoveCommand moveCmd = ref _moveCommandPool.Add(entity);

                Vector3 direction = playerUnit.Position - enemyUnit.Position;
                direction.Normalize();

                moveCmd.Forward = direction.z;
                moveCmd.Right = direction.x;
            }
        }
    }
}
