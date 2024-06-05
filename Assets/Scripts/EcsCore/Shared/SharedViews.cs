using SurvivalDemo.EcsCore.Views;
using System.Collections.Generic;

namespace SurvivalDemo.EcsCore.Shared
{
    public class SharedViews
    {
        public readonly Dictionary<int, PlayerView> PlayerViews = new();
        public readonly Dictionary<int, EnemyView> EnemyViews = new();
        public readonly Dictionary<int, ProjectileView> ProjectileViews = new();
    }
}
