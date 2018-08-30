using System;
using System.Numerics;

namespace HelloRayTracing
{
    public class SphereEntity : Entity
    {
        public SphereEntity(Material entityMaterial, Vector3 center, float radius) : base(entityMaterial)
        {
            Center = center;
            Radius = radius;
        }

        public Vector3 Center { get; }
        public float Radius { get; }

        public override HitResult GetHitResult(Ray ray)
        {
            var a = ray.Direction.LengthSquared();
            var b = 2 * Vector3.Dot(ray.Origin - Center, ray.Direction);
            var c = (ray.Origin - Center).LengthSquared() - Math.Pow(Radius, 2);

            var delta = b * b - 4 * a * c;

            if (delta <= 0) return new HitResult {Hit = false};
            var root1 = (float) (-b - Math.Sqrt(delta)) / (2 * a);
            var root2 = (float) (-b + Math.Sqrt(delta)) / (2 * a);

            var hitPoint1 = ray.Origin + root1 * ray.Direction;
            var hitPoint2 = ray.Origin + root2 * ray.Direction;
            if (root1 > 0)
                return new HitResult
                {
                    Hit = true,
                    HitPoint = hitPoint1,
                    HitNorm = Vector3.Normalize(hitPoint1 - Center)
                };

            if (root2 > 0)
                return new HitResult
                {
                    Hit = true,
                    HitPoint = hitPoint2,
                    HitNorm = Vector3.Normalize(hitPoint2 - Center)
                };

            return new HitResult {Hit = false};
        }
    }
}