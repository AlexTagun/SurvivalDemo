using UnityEngine;

namespace SurvivalDemo.EcsCore.Views
{
    public class EnemyView : MonoBehaviour
    {
        public void UpdateTransform(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
