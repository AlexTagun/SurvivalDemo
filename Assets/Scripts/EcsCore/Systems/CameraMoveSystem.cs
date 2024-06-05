using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.Gameplay;

namespace SurvivalDemo.EcsCore.Systems
{
    public class CameraMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly CameraView _cameraView;

        private EcsFilter _filter;

        private EcsPool<Transform> _transformPool;

        public CameraMoveSystem(CameraView cameraView)
        {
            _cameraView = cameraView;
        }

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<Character>().Inc<ControlledByPlayer>().End();

            _transformPool = world.GetPool<Transform>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                Transform transform = _transformPool.Get(entity);
                _cameraView.SetPosition(transform.Position);
            }
        }
    }
}
