namespace SurvivalDemo.EcsCore.Shared
{
    public class SharedData
    {
        public SharedViews Views = new();
        public SharedAssets Assets;

        public SharedData(SharedAssets assets)
        {
            Assets = assets;
        }
    }
}
