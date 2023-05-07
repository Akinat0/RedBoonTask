

using System;
using UnityEngine;

namespace Pathfinding
{
    [Serializable]
    public  struct Line
    {

        [field:SerializeField]
        public Vector2 First { get; private set; }
        [field:SerializeField]
        public Vector2 Second { get; private set; }
        public bool IsPoint => First == Second;
        public bool IsHorizontal => Mathf.Approximately(First.x, Second.x);
        public bool IsVertical => Mathf.Approximately(First.y, Second.y);

        public Line(Vector2 first, Vector2 second)
        {
            First = first;
            Second = second;
        }

        public Vector2 GetLeftPoint()
        {
            return First.x < Second.x ? First : Second;
        }

        public Vector2 GetRightPoint()
        {
            return First.x < Second.x ? Second : First;
        }
        
        public Vector2 GetTopPoint()
        {
            return First.y < Second.y ? Second : First;
        }

        public Vector2 GetBottomPoint()
        {
            return First.y < Second.y ? First : Second;
        }
        
    }
}