using UnityEngine;

namespace SurvivalDemo.EcsCore.Components
{
    public struct Unit {
        public Transform Transform;
        public Vector3 Position;
        public Quaternion Rotation;
        public float MoveSpeed;
    }
}
