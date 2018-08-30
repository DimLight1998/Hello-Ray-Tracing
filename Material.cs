using System.Drawing;

namespace HelloRayTracing
{
    public class Material
    {
        public Material(Color ambient, Color diffuse, Color specular, Color emission, float shininess,
            float reflectivity)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Emission = emission;
            Shininess = shininess;
            Reflectivity = reflectivity;
        }

        public Color Ambient { get; }
        public Color Diffuse { get; }
        public Color Specular { get; }
        public Color Emission { get; }
        public float Shininess { get; }
        public float Reflectivity { get; }
    }
}