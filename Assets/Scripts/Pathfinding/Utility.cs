using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public static class Utility
    {
        public static IEnumerable<Rect> GetRects(IEnumerable<Edge> edges)
        {
            foreach (Edge edge in edges)
            {
                yield return edge.First;
                yield return edge.Second;
            }
        }
        
        
        public static Vector2 GetLinesIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            float tmp = (b2.x - b1.x) * (a2.y - a1.y) - (b2.y - b1.y) * (a2.x - a1.x);

            if (tmp == 0)
            {
                return Vector2.negativeInfinity;
                 // no solution
            } 

            float mu = ((a1.x - b1.x) * (a2.y - a1.y) - (a1.y - b1.y) * (a2.x - a1.x)) / tmp;

            return new Vector2(
                b1.x + (b2.x - b1.x) * mu,
                b1.y + (b2.y - b1.y) * mu
            );
        }

        public static Vector2 GetLinesIntersection(Vector2 p1, float theta1, Vector2 p2, float theta2)
        {
            return GetLinesIntersection(p1, p1 + new Vector2(Mathf.Cos(Mathf.Deg2Rad * theta1), Mathf.Sin(Mathf.Deg2Rad * theta1)), 
                p2, p2 + new Vector2(Mathf.Cos(Mathf.Deg2Rad * theta2), Mathf.Sin(Mathf.Deg2Rad * theta2)));
        }
        
        public static Vector2 GetLinesIntersection(Vector2 p1, float theta1, Vector2 a2, Vector2 b2)
        {
            return GetLinesIntersection(p1, p1 + new Vector2(Mathf.Cos(Mathf.Deg2Rad * theta1), Mathf.Sin(Mathf.Deg2Rad * theta1)), 
                a2, b2);
        }
        
        public static bool TryGetLineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            float d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

            if (d == 0.0f)
            {
                return false;
            }

            float u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
            float v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

            if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
            {
                return false;
            }

            intersection.x = p1.x + u * (p2.x - p1.x);
            intersection.y = p1.y + u * (p2.y - p1.y);

            return true;
        }


        public static bool TryGetLineAndLineSegmentIntersection(Vector2 l1, Vector2 l2, Vector2 s1, Vector2 s2, out Vector2 result)
        {
            result = Vector2.zero;
            
            float s1Sign = GetPointSide(l1, l2, s1);
            float s2Sign = GetPointSide(l1, l2, s2);

            const float epsilon = 0.000001f;
            
            bool isZeroSign = Mathf.Abs(s1Sign) < epsilon || Mathf.Abs(s2Sign) < epsilon;
            
            if ((s1Sign > 0 && s2Sign > 0 || s1Sign < 0 && s2Sign < 0) && !isZeroSign)
            {
                return false;
            }

            result = GetLinesIntersection(l1, l2, s1, s2);

            return true;

        }

        public static float GetPointSide(Vector2 l1, Vector2 l2, Vector2 p)
        {
            return (l2.x - l1.x) * (p.y - l1.y) - (l2.y - l1.y) * (p.x - l1.x);
        }

        
    }
}