using Leopotam.EcsLite;
using SurvivalDemo.EcsCore.Configs;
using SurvivalDemo.EcsCore.Systems;
using SurvivalDemo.Gameplay.SpawnPoints;
using UnityEngine;

namespace SurvivalDemo.Startup
{
    internal sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private SharedAssets _sharedAssets;
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        [SerializeField] private Transform _cameraAnchor;

        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world, _sharedAssets);
            _systems
                .Add(new UserKeyboardInputSystem())
                .Add(new PlayerInitSystem(_playerSpawnPoint))
                .Add(new UnitMoveSystem())
                .Add(new CameraMoveSystem(_cameraAnchor))
                .Add(new EnemyInitSystem())
                .Add(new EnemyAiInputSystem())

                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())

                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
                #if UNITY_EDITOR

                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
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
