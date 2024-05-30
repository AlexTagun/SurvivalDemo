using UnityEngine;

namespace SurvivalDemo.EcsCore.Components
{
    public struct Character { }

    public struct InstantiatePlayerRequest { }
    public struct InstantiateEnemyRequest { }

    public struct Transform
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    public struct MoveSpeed
    {
        public float Value;
    }
}
