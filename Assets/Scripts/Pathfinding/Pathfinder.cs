using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Pathfinder : IPathFinder
    {
        readonly List<Edge> edgesList = new List<Edge>();
        readonly List<Vector2> points = new List<Vector2>();

        public IEnumerable<Vector2> GetPath(Vector2 start, Vector2 end, IEnumerable<Edge> edges)
        {
            Cone CalculateNextCone(ref int i, Cone currentCone)
            {
                while (i < edgesList.Count)
                {
                    Line currentLine;
                    Line nextLine;

                    if (i == 0) //process first point
                    {
                        currentLine = new Line(start, start);
                        nextLine = new Line(edgesList[i].Start, edgesList[i].End);
                    }
                    else if (i == edgesList.Count - 1) //process last point
                    {
                        currentLine = new Line(edgesList[i].Start, edgesList[i].End);
                        nextLine = new Line(end, end);
                    }
                    else
                    {
                        currentLine = new Line(edgesList[i - 1].Start, edgesList[i - 1].End);
                        nextLine = new Line(edgesList[i].Start, edgesList[i].End);
                    }

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
                    
                    Cone prevCone = currentCone;
                    currentCone = default;
                    Rect checkRect = edgesList[i].First;

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
                    }
                    
                    return prevCone;
                }
                
                points.Add(end);

                return currentCone;
            }

            edgesList.Clear();
            points.Clear();

            foreach (Edge edge in edges)
                edgesList.Add(edge);

            int index = 0;
            
            CalculateNextCone(ref index, default);
            
            points.Add(start);

            return points;
        }
    }
}