using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class CharacterMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<Transform> _transformPool;
        private EcsPool<MoveSpeed> _moveSpeedPool;
        private EcsPool<MoveCommand> _moveCommandPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<Character>().Inc<Transform>().Inc<MoveSpeed>().Inc<MoveCommand>().End();
            _transformPool = world.GetPool<Transform>();
            _moveSpeedPool = world.GetPool<MoveSpeed>();
            _moveCommandPool = world.GetPool<MoveCommand>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref MoveCommand move = ref _moveCommandPool.Get(entity);
                ref Transform transform = ref _transformPool.Get(entity);
                ref MoveSpeed moveSpeed = ref _moveSpeedPool.Get(entity);


                Vector3 delta = new(move.Right, 0, move.Forward);
                delta.Normalize();

                transform.Position = Vector3.Lerp(transform.Position,
                    transform.Position + delta,
                    moveSpeed.Value * Time.deltaTime);
                _moveCommandPool.Del(entity);
            }
        }
    }
}
