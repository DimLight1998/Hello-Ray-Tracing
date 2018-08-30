using System;
using System.Numerics;

namespace HelloRayTracing
{
    public class PlaneEntity : Entity
    {
        public PlaneEntity(Material entityMaterial, Vector3 verticalVector, float distanceFromOrigin)
            : base(entityMaterial)
        {
            VerticalVector = verticalVector;
            DistanceFromOrigin = distanceFromOrigin;
        }

        public Vector3 VerticalVector { get; protected set; }
        public float DistanceFromOrigin { get; protected set; }

        public override HitResult GetHitResult(Ray ray)
        {
            var a = Vector3.Dot(VerticalVector, ray.Direction);
            if (Math.Abs(a) < float.Epsilon) return new HitResult {Hit = false};

            var b = Vector3.Dot(VerticalVector, ray.Origin) - VerticalVector.Length() * DistanceFromOrigin;
            var res = -b / a;
            if (res <= 0) return new HitResult {Hit = false};
            var hitPoint = ray.Origin + res * ray.Direction;
            var maybeNorm = Vector3.Normalize(VerticalVector);

            return new HitResult
            {
                Hit = true,
                HitPoint = hitPoint,
                HitNorm = Vector3.Dot(maybeNorm, ray.Direction) < 0 ? maybeNorm : -maybeNorm
            };
        }
    }
}