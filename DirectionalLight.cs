using System.Drawing;
using System.Numerics;

namespace HelloRayTracing
{
    public class DirectionalLight : Light
    {
        private readonly Vector3 _direction;

        public DirectionalLight(Color diffuse, Color specular, Vector3 direction)
            : base(diffuse, specular)
        {
            _direction = direction;
        }

        public override bool HasLightOn(Vector3 point, World world)
        {
            var testRay = new Ray(point, -_direction);
            return !world.IsBlockedByEntity(testRay);
        }

        public override float GetAttenuation(Vector3 point)
        {
            // directional light has no attenuation
            return 1;
        }

        public override Vector3 GetLightVector(Vector3 point)
        {
            return _direction;
        }
    }
}