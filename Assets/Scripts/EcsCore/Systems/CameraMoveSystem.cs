using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;

namespace SurvivalDemo.EcsCore.Systems
{
    public class CameraMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Transform _cameraAnchor;

        private EcsFilter _filter;
        private EcsPool<Unit> _unitPool;

        public CameraMoveSystem(Transform cameraAnchor)
        {
            _cameraAnchor = cameraAnchor;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<Unit>().Inc<ControlledByPlayer>().End();
            _unitPool = world.GetPool<Unit>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Unit unit = ref _unitPool.Get(entity);

                _cameraAnchor.position = unit.Position;
            }
        }
    }
}
