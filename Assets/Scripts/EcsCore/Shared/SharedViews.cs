using SurvivalDemo.EcsCore.Views;
using System.Collections.Generic;

namespace SurvivalDemo.EcsCore.Shared
{
    public class SharedViews
    {
        public Dictionary<int, PlayerView> PlayerViews = new();
        public Dictionary<int, EnemyView> EnemyViews = new();
    }
}
