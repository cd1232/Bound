using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject {

    [SerializeField]
    public enum ObstacleType
    {
        Square,
        LShape,
        LilEdge,
        BigEdge,
        SquareBox
    }

    [System.Serializable]
    public class Obstacle
    {
        public ObstacleType type;
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
    }

    [System.Serializable]
    public class Portal
    {
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
    }

    [SerializeField]
    public Vector3 ballStartPosition;

    [SerializeField]
    public Vector3 goalPosition;

    [SerializeField]
    public Sprite backgroundGradient;
    
    [SerializeField]
    public Obstacle[] obstacles;

    [SerializeField]
    public Portal[] portals;
    
}
