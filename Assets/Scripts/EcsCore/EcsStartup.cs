using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Shared;
using SurvivalDemo.EcsCore.Systems;
using SurvivalDemo.Gameplay;
using SurvivalDemo.Gameplay.SpawnPoints;
using UnityEngine;

namespace SurvivalDemo.Startup
{
    internal sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private SharedAssets _sharedAssets;
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        [SerializeField] private CameraView _cameraView;

        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start()
        {
            _world = new EcsWorld();

            SharedData sharedData = new(_sharedAssets);

            _systems = new EcsSystems(_world, sharedData);
            _systems
                .Add(new UserKeyboardInputSystem())
                .Add(new CharacterMoveSystem())
                .Add(new CameraMoveSystem(_cameraView))

                .Add(new EnemyAiInputSystem())

                .Add(new PlayerInitSystem(_playerSpawnPoint))
                .Add(new PlayerViewCreateSystem())
                .Add(new PlayerViewUpdateSystem())

                .Add(new EnemyHealthSystem())
                .Add(new EnemyInitSystem())
                .Add(new EnemyViewCreateSystem())
                .Add(new EnemyViewUpdateSystem())
                .Add(new EnemyViewDestroySystem())

                .Add(new ProjectileMoveSystem())
                .Add(new ProjectileDamageSystem())

                .Add(new ProjectileViewCreateSystem())
                .Add(new ProjectileViewUpdateSystem())
                .Add(new ProjectileViewDestroySystem())

                .Add(new WeaponFireballSystem())

                #if UNITY_EDITOR

                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                #endif
                .Init();
        }

        private void Update()
        {
            // process systems here.
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                // list of custom worlds will be cleared
                // during IEcsSystems.Destroy(). so, you
                // need to save it here if you need.
                _systems.Destroy();
                _systems = null;
            }

            // cleanup custom worlds here.

            // cleanup default world.
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}
