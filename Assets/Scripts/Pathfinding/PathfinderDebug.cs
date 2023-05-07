using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Pathfinding
{
    public class PathfinderDebug : MonoBehaviour
    {
        [SerializeField] Edge[] edges;
        [SerializeField] Vector2 startPoint;
        [SerializeField] Vector2 endPoint;

        [Header("defug")]
        [SerializeField] Line[] lines; 

        void OnDrawGizmos()
        {
            Rect[] rects = Utility.GetRects(edges).Distinct().ToArray();
            
            foreach (Rect rect in rects)
                DrawRect(rect);
            
            DrawPoint(startPoint, "start", Color.green);
            DrawPoint(endPoint, "end", Color.red);
            
            Pathfinder pathfinder = new Pathfinder();
            List<Vector2> path = pathfinder.GetPath(startPoint, endPoint, edges).ToList();
            
            foreach (Cone cone in pathfinder.cones)
            {
                DrawCone(cone, Color.cyan);
            }
            
            foreach (Edge edge in edges)
                DrawLine(edge.Start, edge.End, Color.blue);
            
            for (int i = 0; i < path.Count - 1; i++)
            {
                DrawLine(path[i], path[i+1], Color.yellow);
            }


            // Cone currentCone = default;
            //
            // for (int i = 0; i < lines.Length - 1; i++)
            // {
            //     Line currentLine = lines[i];
            //     Line nextLine = lines[i + 1];
            //
            //     if (currentCone == default)
            //     {
            //         currentCone = new Cone(currentLine, nextLine);
            //         DrawCone(currentCone, currentCone.Target, Color.magenta);
            //         continue;
            //     }
            //
            //     if (currentCone.TryGetInnerCone(nextLine, out Cone innerCone))
            //     {
            //         currentCone = innerCone;
            //         DrawCone(currentCone, currentCone.Target, Color.magenta);
            //     }
            // }
            //
            // foreach (var line in lines)
            // {
            //     DrawLine(line.First, line.Second, Color.blue);
            // }
        }

        void DrawRect(Rect rect)
        {
            
            Gizmos.DrawWireCube(rect.center, rect.size);
         
            Handles.Label(rect.min, rect.min.ToString());
            Handles.Label(rect.max, rect.max.ToString());
        }

        void DrawLine(Vector2 from, Vector2 to, Color color)
        {
            Color gizmosColor = Gizmos.color;
            Gizmos.color = color;
            
            Gizmos.DrawLine(from, to);
            
            Gizmos.color = gizmosColor;
        }

        void DrawPoint(Vector2 point, string label, Color color)
        {
            Color gizmosColor = Gizmos.color;
            Gizmos.color = color;
            
            Gizmos.DrawSphere(point, 0.05f);
            Handles.Label(point, label);

            Gizmos.color = gizmosColor;
        }

        void DrawCone(Cone cone, Color color)
        {
            Color gizmosColor = Gizmos.color;
            Gizmos.color = color;
            
            Gizmos.DrawLine(cone.Source.First, cone.Source.Second);
            // Gizmos.DrawRay(cone.Source.First, new Vector2(Mathf.Cos(Mathf.Rad2Deg * cone.SecondAngle), Mathf.Sin(Mathf.Rad2Deg * cone.SecondAngle)));
            // Gizmos.DrawRay(cone.Source.Second, new Vector2(Mathf.Cos(Mathf.Rad2Deg * cone.FirstAngle), Mathf.Sin(Mathf.Rad2Deg * cone.FirstAngle)));
            
            Gizmos.DrawLine(cone.Source.First, cone.Target.First);
            Gizmos.DrawLine(cone.Source.Second, cone.Target.Second);
            Gizmos.color = gizmosColor;
        }
    }
}