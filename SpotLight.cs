using System;
using System.Drawing;
using System.Numerics;

namespace HelloRayTracing
{
    public class SpotLight : Light
    {
        private readonly float _coef0;
        private readonly float _coef1;
        private readonly float _coef2;
        private readonly Vector3 _direction;
        private readonly float _innerCutoffCosine;
        private readonly float _outerCutoffCosine;
        private readonly Vector3 _position;

        public SpotLight(
            Color diffuse, Color specular, Vector3 position, Vector3 direction,
            float innerCutoffCosine, float outerCutoffCosine, float coef0, float coef1, float coef2
        ) : base(diffuse, specular)
        {
            _position = position;
            _direction = direction;
            _innerCutoffCosine = innerCutoffCosine;
            _outerCutoffCosine = outerCutoffCosine;
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
            var normLightVec = Vector3.Normalize(GetLightVector(point));
            var cosine = Vector3.Dot(normLightVec, Vector3.Normalize(_direction));
            var baseAttenuation = Math.Clamp(
                (cosine - _outerCutoffCosine) / (_innerCutoffCosine - _outerCutoffCosine), 0, 1
            );

            var distance = Vector3.Distance(point, _position);
            var distanceAttenuation = 1 / (_coef0 + _coef1 * distance + _coef2 * distance * distance);
            return baseAttenuation * distanceAttenuation;
        }

        public override Vector3 GetLightVector(Vector3 point)
        {
            return point - _position;
        }
    }
}