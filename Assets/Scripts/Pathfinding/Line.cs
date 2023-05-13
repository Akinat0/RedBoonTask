using UnityEngine;

namespace Pathfinding
{
    public readonly struct Line
    {
        public Vector2 First { get; }
        public Vector2 Second { get; }

        public Line(Vector2 first, Vector2 second)
        {
            First = first;
            Second = second;
        }
    }
}