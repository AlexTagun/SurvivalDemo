using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;

namespace SurvivalDemo.EcsCore.Systems
{
    public class UnitMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<Unit> _unitPool;
        private EcsPool<MoveCommand> _moveCommandPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<Unit>().Inc<MoveCommand>().End();
            _unitPool = world.GetPool<Unit>();
            _moveCommandPool = world.GetPool<MoveCommand>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Unit unit = ref _unitPool.Get(entity);
                ref MoveCommand move = ref _moveCommandPool.Get(entity);

                Vector3 delta = new(move.Right, 0, move.Forward);
                delta.Normalize();

                unit.Position = Vector3.Lerp(unit.Position, unit.Position + delta, unit.MoveSpeed * Time.deltaTime);
                unit.Transform.localPosition = unit.Position;

                _moveCommandPool.Del(entity);
            }
        }
    }
}
