using Leopotam.EcsLite;

namespace SurvivalDemo.EcsCore.Systems
{
    public class HealthSystem : IEcsInitSystem, IEcsRunSystem
    {
        // private EcsFilter _playerFilter;
        // private EcsFilter _enemyFilter;
        // private EcsPool<Unit> _unitPool;
        // private EcsPool<MoveCommand> _moveCommandPool;

        public void Init(IEcsSystems systems)
        {
            // EcsWorld world = systems.GetWorld();
            // _playerFilter = world.Filter<Character>().Inc<ControlledByPlayer>().End();
            // _enemyFilter = world.Filter<Unit>().Inc<ControlledByAi>().End();
            // _unitPool = world.GetPool<Unit>();
            // _moveCommandPool = world.GetPool<MoveCommand>();
        }

        public void Run(IEcsSystems systems)
        {

        }
    }
}
