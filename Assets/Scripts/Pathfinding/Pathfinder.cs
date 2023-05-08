using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Pathfinding
{
    public class Pathfinder : IPathFinder
    {
        public List<Cone> cones;
        
        //пора делать эту хуйню рекурсивной
        public IEnumerable<Vector2> GetPath(Vector2 start, Vector2 end, IEnumerable<Edge> edges)
        {
            List<Edge> edgesList = edges.ToList();
            List<Rect> rects = new List<Rect>(edgesList.Count + 1);
            List<Line> lines = new List<Line>(edgesList.Count);
            List<Vector2> points = new List<Vector2>();
            cones = new List<Cone>();

            lines.Add(new Line(start, start));
            
            for (int i = 0; i < edgesList.Count; i++)
            {
                Edge edge = edgesList[i];

                rects.Add(edge.First);
                lines.Add(new Line(edge.Start, edge.End));
            }
            
            lines.Add(new Line(end, end));
            rects.Add(edgesList[edgesList.Count - 1].Second);
            
            Cone CalculateNextCone(ref int i, Cone currentCone)
            {
                while (i < lines.Count - 1)
                {
                    Line currentLine = lines[i];
                    Line nextLine = lines[i + 1];
                    
                    if (currentCone == default)
                    {
                        currentCone = new Cone(currentLine, nextLine);
                        i++;
                        continue;
                    }
                    
                    if (currentCone.TryGetInnerCone(nextLine, out Cone innerCone))
                    {
                        currentCone = innerCone;
                        i++;
                        continue;
                    }
                    
                    cones.Add(currentCone);
                    
                    Cone prevCone = currentCone;
                    currentCone = default;
                    Rect checkRect = rects[i];

                    i++;

                    currentCone = CalculateNextCone(ref i, currentCone);
                    
                    if (currentCone == default) //it was not possible to enter inner cycle loop
                        currentCone = new Cone(currentLine, nextLine);

                    if (prevCone.HasDirectIntersection(currentCone, out Vector2 intersection))
                    {
                        points.Add(intersection);
                    }
                    else if (prevCone.TryGetOuterIntersectionPoint(currentCone, checkRect, out intersection, out Vector2 outerLinePoint))
                    { 
                        points.Add(intersection);
                        Vector2 firstShrinkConePoint = Utility.GetLinesIntersection(intersection, outerLinePoint, prevCone.Source.First, prevCone.Source.Second);

                        Vector2 secondShrinkConePoint = 
                            Utility.TryGetLineAndLineSegmentIntersection(intersection, prevCone.Source.First, prevCone.Target.First, prevCone.Target.Second, out _) 
                            ? prevCone.Source.First 
                            : prevCone.Source.Second;

                        prevCone = new Cone(new Line(firstShrinkConePoint, secondShrinkConePoint), new Line(intersection, intersection));
                    }
                    else
                    {
                        points.Add(currentCone.Source.First);
                        points.Add(prevCone.Target.Second);
                        // prevCone = new Cone(prevCone.Source, new Line(prevCone.Target.Second, prevCone.Target.Second));
                    }

                    // cones.Add(prevCone);
                    return prevCone;
                }
                
                cones.Add(currentCone);
                points.Add(end);

                return currentCone;
            }

            int index = 0;
            
            CalculateNextCone(ref index, default);
            
            points.Add(start);

            return points;
        }
    }
}