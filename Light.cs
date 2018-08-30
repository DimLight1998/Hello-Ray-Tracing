using System;
using System.Drawing;
using System.Numerics;

namespace HelloRayTracing
{
    public class Light
    {
        protected Light(Color diffuse, Color specular)
        {
            Diffuse = diffuse;
            Specular = specular;
        }

        public Color Diffuse { get; }
        public Color Specular { get; }

        public virtual bool HasLightOn(Vector3 point, World world)
        {
            throw new NotImplementedException();
        }

        public virtual float GetAttenuation(Vector3 point)
        {
            throw new NotImplementedException();
        }

        public virtual Vector3 GetLightVector(Vector3 point)
        {
            throw new NotImplementedException();
        }
    }
}