using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    public struct Pathfinder : IPathFinder
    {
        public List<Cone> cones;
        
        public IEnumerable<Vector2> GetPath(Vector2 start, Vector2 end, IEnumerable<Edge> edges)
        {
            List<Edge> edgesList = edges.ToList();
            List<Rect> rects = new List<Rect>(edgesList.Count + 1);
            List<Line> lines = new List<Line>(edgesList.Count);

            lines.Add(new Line(start, start));
            
            for (int i = 0; i < edgesList.Count; i++)
            {
                Edge edge = edgesList[i];

                rects.Add(edge.First);
                lines.Add(new Line(edge.Start, edge.End));
            }
            
            lines.Add(new Line(end, end));
            
            rects.Add(edgesList[edgesList.Count - 1].Second);


            List<Vector2> points = new List<Vector2>();
            
            points.Add(start);
            
            cones = new List<Cone>();

            Cone currentCone = default;

            bool shouldCalculatePoint = false;
            
            for (int i = 0; i < lines.Count - 1; i++)
            {
                Line currentLine = lines[i];
                Line nextLine = lines[i + 1];

                if (shouldCalculatePoint)
                {
                    Cone prevCone = currentCone;
                    currentCone = new Cone(currentLine, nextLine);

                    if (prevCone.TryGetIntersectionPoint(currentCone, rects[i - 1], out Vector2 intersection))
                    {
                        points.Add(intersection);
                    }
                    else
                    {
                        points.Add(prevCone.Target.Second);
                        points.Add(currentCone.Source.First);
                    }

                    continue;
                }
                
                if (currentCone == default)
                {
                    currentCone = new Cone(currentLine, nextLine);
                    continue;
                }

                if (currentCone.TryGetInnerCone(nextLine, out Cone innerCone))
                {
                    // cones.Add(currentCone);
                    currentCone = innerCone;
                    continue;
                }
                
                cones.Add(currentCone);

                shouldCalculatePoint = true;

                // if (IsOntoTheSameLine(currentLine, nextLine))
                // {
                //     shouldCalculatePoint = true;
                //     continue;
                // }
                //
                // Cone newCone = new Cone(currentCone.Target, nextLine);
                //
                // if (currentCone.TryGetIntersectionPoint(newCone, rects[i - 1], out Vector2 intersectionPoint))
                //     points.Add(intersectionPoint);
                //
                // currentCone = newCone;


                // cones.Add((currentCone, nextLine));

                // points.Add(currentCone.TryGetIntersectionPoint(newCone));
                // currentCone = newCone;
            }
            
            if (shouldCalculatePoint)
                points.Add(currentCone.Target.First);
            
            cones.Add(currentCone);
            points.Add(end);

            return points;
        }

        bool IsOntoTheSameLine(Line first, Line second)
        {
            bool isOntoTheSameHorizontalLine = Mathf.Approximately(first.First.y, first.Second.y)
                                               && Mathf.Approximately(second.First.y, second.Second.y)
                                               && Mathf.Approximately(first.First.y, second.First.y);
                
            bool isOntoTheSameVerticalLine = Mathf.Approximately(first.First.y, first.Second.y)
                                             && Mathf.Approximately(second.First.y, second.Second.y)
                                             && Mathf.Approximately(first.First.y, second.First.y);

            return isOntoTheSameHorizontalLine || isOntoTheSameVerticalLine;
        }
    }
}