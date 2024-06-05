using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using UnityEngine;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class ProjectileMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<Transform> _transformPool;
        private EcsPool<MoveSpeed> _moveSpeedPool;
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            _filter = world.Filter<Projectile>().Inc<Transform>().Inc<MoveSpeed>().End();

            _transformPool = world.GetPool<Transform>();
            _moveSpeedPool = world.GetPool<MoveSpeed>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref Transform transform = ref _transformPool.Get(entity);
                MoveSpeed moveSpeed = _moveSpeedPool.Get(entity);

                Vector3 delta = transform.Rotation * Vector3.forward;
                transform.Position += Time.deltaTime * moveSpeed.Value * delta;
            }
        }
    }
}
