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

        Pathfinder Pathfinder { get; set; }

        void Start()
        {
            Pathfinder = new Pathfinder();
        }

        void Update()
        {
            Pathfinder.GetPath(startPoint, endPoint, edges);
        }

        void OnDrawGizmosSelected()
        {
            if(Application.isPlaying)
                return;
            
            foreach (Edge edge in edges)
                DrawRect(edge.First);
            
            DrawRect(edges.Last().Second);
            
            DrawPoint(startPoint, "start", Color.green);
            DrawPoint(endPoint, "end", Color.red);

            Pathfinder ??= new Pathfinder();
            
            List<Vector2> path = Pathfinder.GetPath(startPoint, endPoint, edges).ToList();

            foreach (Edge edge in edges)
                DrawLine(edge.Start, edge.End, Color.blue);
            
            for (int i = 0; i < path.Count - 1; i++)
                DrawLine(path[i], path[i+1], Color.yellow);
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
            Gizmos.DrawLine(cone.Source.First, cone.Target.First);
            Gizmos.DrawLine(cone.Source.Second, cone.Target.Second);
            
            Gizmos.color = gizmosColor;
        }
    }
}