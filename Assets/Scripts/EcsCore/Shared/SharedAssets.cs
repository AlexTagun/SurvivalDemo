using SurvivalDemo.EcsCore.Views;
using UnityEngine;

namespace SurvivalDemo.EcsCore.Shared
{
    [CreateAssetMenu]
    public class SharedAssets : ScriptableObject
    {
        public PlayerView PlayerView;
        public EnemyView EnemyView;
        public ProjectileView ProjectileView;
    }
}
