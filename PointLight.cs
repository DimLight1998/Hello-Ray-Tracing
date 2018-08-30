using System.Drawing;
using System.Numerics;

namespace HelloRayTracing
{
    public class PointLight : Light
    {
        private readonly float _coef0;
        private readonly float _coef1;
        private readonly float _coef2;
        private readonly Vector3 _position;

        public PointLight(
            Color diffuse, Color specular, Vector3 position, float coef0, float coef1, float coef2
        ) : base(diffuse, specular)
        {
            _position = position;
            _coef0 = coef0;
            _coef1 = coef1;
            _coef2 = coef2;
        }

        public override bool HasLightOn(Vector3 point, World world)
        {
            return !world.IsBlockedByEntity(point, _position);
        }

        public override float GetAttenuation(Vector3 point)
        {
            var distance = Vector3.Distance(point, _position);
            return 1 / (_coef0 + _coef1 * distance + _coef2 * distance * distance);
        }

        public override Vector3 GetLightVector(Vector3 point)
        {
            return point - _position;
        }
    }
}