using UnityEngine;

namespace SurvivalDemo.Gameplay
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Transform _anchor;

        public void SetPosition(Vector3 position)
        {
            _anchor.position = position;
        }
    }
}
