using Leopotam.EcsLite;
using SurvivalDemo.Constants;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;

namespace SurvivalDemo.EcsCore.Systems
{
    public class UserKeyboardInputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<MoveCommand> _moveCommandPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<Unit>().Inc<ControlledByPlayer>().End();
            _moveCommandPool = world.GetPool<MoveCommand>();
        }

        public void Run(IEcsSystems systems)
        {
            float vertInput = Input.GetAxisRaw(StringConstants.Input.VerticalAxis);
            float horizInput = Input.GetAxisRaw(StringConstants.Input.HorizontalAxis);

            if (vertInput == 0 && horizInput == 0)
            {
                return;
            }

            foreach (int entity in _filter)
            {
                ref MoveCommand moveCmd = ref _moveCommandPool.Add(entity);
                moveCmd.Forward = vertInput;
                moveCmd.Right = horizInput;
            }
        }
    }
}
