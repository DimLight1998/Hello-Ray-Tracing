using System.Numerics;

namespace HelloRayTracing
{
    public class TriangleEntity : PlaneEntity
    {
        public TriangleEntity(Material material, Vector3 pointA, Vector3 pointB, Vector3 pointC)
            : base(material, Vector3.Zero, 0)
        {
            var verticalVector = Vector3.Cross(pointA - pointC, pointB - pointC);
            if (Vector3.Dot(verticalVector, pointA) < 0) verticalVector = -verticalVector;
            var distanceFromOrigin = Vector3.Dot(pointA, verticalVector) / verticalVector.Length();

            VerticalVector = verticalVector;
            DistanceFromOrigin = distanceFromOrigin;
            PointA = pointA;
            PointB = pointB;
            PointC = pointC;
        }

        public Vector3 PointA { get; }
        public Vector3 PointB { get; }
        public Vector3 PointC { get; }

        public override HitResult GetHitResult(Ray ray)
        {
            var baseRet = base.GetHitResult(ray);
            if (baseRet.Hit == false) return baseRet;

            var hitPoint = baseRet.HitPoint;
            var ap = hitPoint - PointA;
            var ab = PointB - PointA;
            var ac = PointC - PointA;
            var lambda = -(ap.X * ac.Y - ac.X * ap.Y) / (ac.X * ab.Y - ab.X * ac.Y);
            var mu = -(ab.X * ap.Y - ap.X * ab.Y) / (ac.X * ab.Y - ab.X * ac.Y);
            if (lambda >= 0 && mu >= 0 && lambda + mu <= 1) return baseRet;
            return new HitResult {Hit = false};
        }
    }
}