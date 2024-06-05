using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Components;
using SurvivalDemo.EcsCore.Shared;
using SurvivalDemo.EcsCore.Views;
using Object = UnityEngine.Object;
using Transform = SurvivalDemo.EcsCore.Components.Transform;

namespace SurvivalDemo.EcsCore.Systems
{
    public class ProjectileViewCreateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedViews _sharedViews;
        private SharedAssets _sharedAssets;

        private EcsFilter _filter;

        private EcsPool<Projectile> _projectilePool;
        private EcsPool<Transform> _transformPool;
        private EcsPool<CreateProjectileRequest> _requestPool;
        private EcsPool<MoveSpeed> _moveSpeedPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();

            SharedData sharedData = systems.GetShared<SharedData>();
            _sharedAssets = sharedData.Assets;
            _sharedViews = sharedData.Views;

            _filter = world.Filter<CreateProjectileRequest>().End();

            _projectilePool = world.GetPool<Projectile>();
            _transformPool = world.GetPool<Transform>();
            _requestPool = world.GetPool<CreateProjectileRequest>();
            _moveSpeedPool = world.GetPool<MoveSpeed>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter)
            {
                ref CreateProjectileRequest request = ref _requestPool.Get(entity);

                _projectilePool.Add(entity);

                ref Transform transform = ref _transformPool.Add(entity);
                transform.Position = request.Position;
                transform.Rotation = request.Rotation;

                ref MoveSpeed moveSpeed = ref _moveSpeedPool.Add(entity);
                moveSpeed.Value = request.MoveSpeed;

                ProjectileView view =
                    Object.Instantiate(_sharedAssets.ProjectileView, transform.Position, transform.Rotation);
                _sharedViews.ProjectileViews.Add(entity, view);

                _requestPool.Del(entity);
            }
        }
    }
}
