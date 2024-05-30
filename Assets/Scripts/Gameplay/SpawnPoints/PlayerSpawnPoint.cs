using UnityEngine;

namespace SurvivalDemo.Gameplay.SpawnPoints
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
    }
}
