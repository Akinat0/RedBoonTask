using System.Diagnostics;
using UnityEngine;

namespace Pathfinding
{
    public readonly struct Cone
    {
        public float FirstAngle { get; }
        public float SecondAngle { get; }


        public Line Source { get; }
        public Line Target { get; }

        public bool IsExpanding => FirstAngle > SecondAngle;
        

        // public float AverageAngle => (SecondAngle - FirstAngle) / 2 + FirstAngle;
        // public Vector2 AverageSource => (Source.Second - Source.First) / 2 + Source.First;

        public Cone(Vector2 source, Line line) : this(new Line(source, source), line)
        {
        }

        // public Cone(Line source, double minAngle, double maxAngle)
        // {
        //     Source = source;
        //     FirstAngle = minAngle;
        //     SecondAngle = maxAngle;
        // }

        public Cone(Line source, Line target)
        {
            Source = source;
            Target = target;

            //если два отрезка пересекаются, то нужно выбрать другие два отрезка,
            if (Utility.TryGetLineSegmentsIntersection(Source.First, Target.First, Source.Second, Target.Second, out _))
                Target = new Line(Target.Second, Target.First);
            
            FirstAngle = Vector2.SignedAngle(Vector2.right, Target.First - Source.First);
            SecondAngle = Vector2.SignedAngle(Vector2.right, Target.Second - Source.Second);
        
        }

        public bool TryGetInnerCone(Line line, out Cone innerCone)
        {
            innerCone = new Cone();

            float firstLineToP1Sign = Utility.GetPointSide(Source.First, Target.First, line.First); 
            float firstLineToP2Sign = Utility.GetPointSide(Source.First, Target.First, line.Second); 
            float secondLineToP1Sign = Utility.GetPointSide(Source.Second, Target.Second, line.First); 
            float secondLineToP2Sign = Utility.GetPointSide(Source.Second, Target.Second, line.Second);

            bool isLinePointsOnTheSameSideFirst = firstLineToP1Sign >= 0 && firstLineToP2Sign >= 0 
                                                || firstLineToP1Sign <= 0 && firstLineToP2Sign <= 0; 
                
            bool isLinePointsOnTheSameSideSecond = secondLineToP1Sign >= 0 && secondLineToP2Sign >= 0 
                                                || secondLineToP1Sign <= 0 && secondLineToP2Sign <= 0;;
            
            //target line points are on the same side according to both first and second lines
            if (isLinePointsOnTheSameSideFirst && isLinePointsOnTheSameSideSecond)
            {
                int firstSign = firstLineToP1Sign > 0 ? 1 : -1;
                int secondSign = secondLineToP1Sign > 0 ? 1 : -1;
                
                if (firstSign == secondSign) //line is outside the cone 
                    return false;
                
                //line is inside the cone 
                innerCone = new Cone(Source, line);
                return true;
            }

            bool isP1Inside = firstLineToP1Sign > 0 && secondLineToP1Sign < 0 
                              || firstLineToP1Sign < 0 && secondLineToP1Sign > 0;
            
            bool isP2Inside = firstLineToP2Sign > 0 && secondLineToP2Sign < 0 
                              || firstLineToP2Sign < 0 && secondLineToP2Sign > 0;

            Vector2 firstPoint;
            Vector2 secondPoint;

            if (!Utility.TryGetLineAndLineSegmentIntersection(Source.First, Target.First, line.First, line.Second, out firstPoint))
            {
                //we don't have an intersection, so we shrink cone to line
                firstPoint = isP1Inside ? line.First : line.Second; //while shrink we should pick up point inside
            }
            
            //we have an intersection
            if (!Utility.TryGetLineAndLineSegmentIntersection(Source.Second, Target.Second, line.First, line.Second, out secondPoint))
            {
                //we don't have an intersection, so we shrink cone to line
                secondPoint = isP2Inside ? line.Second : line.First; //while shrink we should pick up point inside
            }

            //cone constructor will care about first and second points order
            innerCone = new Cone(Source, new Line(firstPoint, secondPoint));  
            
            return true;
        }

        public static bool IsLinePointsOnSameSide(Vector2 l1, Vector2 l2, Line checkLine, out int sign)
        {
            sign = 0;
            
            if (Utility.GetPointSide(l1, l2, checkLine.First) > 0 && Utility.GetPointSide(l1, l2, checkLine.Second) > 0)
            {
                sign = 1;
                return true;
            }
            
            if (Utility.GetPointSide(l1, l2, checkLine.First) < 0 && Utility.GetPointSide(l1, l2, checkLine.Second) < 0)
            {
                sign = -1;
                return true;
            }

            return false;
        }
        

        #region operators
        
        public bool TryGetIntersectionPoint(Cone other, Rect rect, out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.zero;
            
            //only cross line checks 
            
            Vector2 point = Utility.GetLinesIntersection(Source.First, Target.Second, other.Source.First, other.Target.Second);

            if (RectContainsPoint(rect, point))
            {
                intersectionPoint = point;
                return true;
            }
            
            point = Utility.GetLinesIntersection(Source.First, Target.Second, other.Source.Second, other.Target.First);

            if (RectContainsPoint(rect, point))
            {
                intersectionPoint = point;
                return true;
            }
            
            point = Utility.GetLinesIntersection(Source.Second, Target.First, other.Source.First, other.Target.Second);

            if (RectContainsPoint(rect, point))
            {
                intersectionPoint = point;
                return true;
            }
            
            point = Utility.GetLinesIntersection(Source.Second, Target.First, other.Source.Second, other.Target.First);

            if (RectContainsPoint(rect, point))
            {
                intersectionPoint = point;
                return true;
            }

            return false;
        }

        bool RectContainsPoint(Rect rect, Vector2 point)
        {
            //inclusive rect.Contains(point)
            return point.x >= rect.xMin && point.x <= rect.xMax && point.y >= rect.yMin && point.y <= rect.yMax;
        }
        
        public static bool operator ==(Cone lhs, Cone rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Cone lhs, Cone rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(Cone other)
        {
            return Mathf.Approximately(FirstAngle, other.FirstAngle)
                   && Mathf.Approximately(SecondAngle, other.SecondAngle)
                   && Source.First == other.Source.First
                   && Source.Second == other.Source.Second;
        }

        public override bool Equals(object obj)
        {
            return obj is Cone other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = FirstAngle.GetHashCode();
                hashCode = (hashCode * 397) ^ SecondAngle.GetHashCode();
                hashCode = (hashCode * 397) ^ Source.GetHashCode();
                return hashCode;
            }
        }
        
        #endregion
    }
}