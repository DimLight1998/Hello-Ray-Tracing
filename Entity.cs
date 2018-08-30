using System.Numerics;

namespace HelloRayTracing
{
    public abstract class Entity
    {
        public Entity(Material entityMaterial)
        {
            EntityMaterial = entityMaterial;
        }

        public Material EntityMaterial { get; }

        public abstract HitResult GetHitResult(Ray ray);

        public class HitResult
        {
            public HitResult()
            {
                Hit = false;
            }

            public HitResult(Vector3 hitPoint, Vector3 hitNorm)
            {
                Hit = true;
                HitPoint = hitPoint;
                HitNorm = hitNorm;
            }

            public bool Hit { get; protected internal set; }
            public Vector3 HitPoint { get; protected internal set; }
            public Vector3 HitNorm { get; protected internal set; }
        }
    }
}