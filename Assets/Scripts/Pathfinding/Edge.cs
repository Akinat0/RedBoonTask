using System;
using UnityEngine;

namespace Pathfinding
{
    [Serializable]
    public struct Edge
    {
        public Rect First;
        public Rect Second;
        public Vector2 Start;
        public Vector2 End;
    }
}