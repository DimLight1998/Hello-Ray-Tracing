using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HelloRayTracing
{
    public class World
    {
        public const float Epsilon = 0.0001f;
        public List<Entity> Entities { get; } = new List<Entity>();
        public List<Light> Lights { get; } = new List<Light>();

        public Color GetColor(Ray ray, int maxDepth)
        {
            if (maxDepth == 0) return Color.Black;

            var minDistanceSquared = float.MaxValue;
            Entity.HitResult realHitResult = null;
            Entity realHitEntity = null;

            foreach (var entity in Entities)
            {
                var hitResult = entity.GetHitResult(ray);
                var distanceSquared = Vector3.DistanceSquared(hitResult.HitPoint, ray.Origin);
                if (!hitResult.Hit || !(distanceSquared < minDistanceSquared)) continue;
                minDistanceSquared = distanceSquared;
                realHitResult = hitResult;
                realHitEntity = entity;
            }

            if (realHitResult == null) return Color.Black;

            var baseColor = Color.Black;
            var reflectedColor = Color.Black;

            if (realHitEntity.EntityMaterial.Reflectivity <= 1 - float.Epsilon)
                baseColor = GetColorOnEntity(realHitResult, realHitEntity, ray);

            if (realHitEntity.EntityMaterial.Reflectivity >= float.Epsilon)
            {
                // in case of precision issue
                var direction = Vector3.Reflect(ray.Direction, realHitResult.HitNorm);
                var origin = realHitResult.HitPoint + Epsilon * Vector3.Normalize(realHitResult.HitNorm);
                var reflectedRay = new Ray(origin, direction);

                reflectedColor = GetColor(reflectedRay, maxDepth - 1);
            }

            return baseColor.Multiply(1 - realHitEntity.EntityMaterial.Reflectivity).Add(
                reflectedColor.Multiply(realHitEntity.EntityMaterial.Reflectivity)
            );
        }

        public Color GetColorOnEntity(Entity.HitResult hitResult, Entity entity, Ray ray)
        {
            var emission = entity.EntityMaterial.Emission;
            var ambient = entity.EntityMaterial.Ambient;
            var finalColor = emission.Add(ambient);

            // in case of precision issue
            var checkPoint = hitResult.HitPoint + Epsilon * hitResult.HitNorm;

            foreach (var light in Lights)
            {
                if (!light.HasLightOn(checkPoint, this)) continue;

                var lightVec = light.GetLightVector(hitResult.HitPoint);
                var lightReflect = Vector3.Reflect(lightVec, hitResult.HitNorm);

                var attenuation = light.GetAttenuation(hitResult.HitPoint);
                var diffuse = light.Diffuse.Moderate(entity.EntityMaterial.Diffuse);
                var specular = light.Specular.Moderate(entity.EntityMaterial.Specular);

                var diffuseCoef =
                    Math.Max(Vector3.Dot(-Vector3.Normalize(hitResult.HitNorm), Vector3.Normalize(lightVec)), 0);
                var specularCoef =
                    Math.Max(Vector3.Dot(Vector3.Normalize(lightReflect), -Vector3.Normalize(ray.Direction)), 0);
                specularCoef = (float) Math.Pow(specularCoef, entity.EntityMaterial.Shininess);
                var lightColor = diffuse.Multiply(diffuseCoef).Add(specular.Multiply(specularCoef));
                finalColor = finalColor.Add(lightColor.Multiply(attenuation));
            }

            return finalColor;
        }

        public bool IsBlockedByEntity(Ray ray)
        {
            foreach (var entity in Entities)
                if (entity.GetHitResult(ray).Hit)
                    return true;
            return false;
        }

        public bool IsBlockedByEntity(Vector3 pointA, Vector3 pointB)
        {
            var origin = pointA;
            var direction = pointB - pointA;
            var distanceSquared = Vector3.DistanceSquared(pointA, pointB);
            var ray = new Ray(origin, direction);

            foreach (var entity in Entities)
            {
                var hitResult = entity.GetHitResult(ray);
                if (hitResult.Hit && Vector3.DistanceSquared(hitResult.HitPoint, origin) < distanceSquared)
                    return true;
            }

            return false;
        }

        public Image<Rgba32> GetRenderedImage(
            Vector3 cameraPosition, Vector3 cameraDirection, Vector3 cameraUpside,
            float verticalFov, float aspect, int verticalResolution
        )
        {
            var screenCenterPosition = cameraPosition + cameraDirection;
            var verticalOffset =
                Vector3.Normalize(cameraUpside) * (float) Math.Tan(verticalFov / 2) * cameraDirection.Length();
            var horizontalOffset =
                Vector3.Normalize(Vector3.Cross(cameraDirection, cameraUpside)) * aspect * verticalOffset.Length();

            var unitVerticalOffset = verticalOffset / ((float) verticalResolution / 2);
            var unitHorizontalOffset = horizontalOffset / (verticalResolution * aspect / 2);

            var halfWidth = (int) (verticalResolution * aspect / 2);
            var halfHeight = verticalResolution / 2;

            var ret = new Image<Rgba32>(2 * halfWidth, 2 * halfHeight);

#if DEBUG
            for (var w = 0; w < ret.Width; w++)
            {
                Console.WriteLine(w);
                for (var h = 0; h < ret.Height; h++)
                {
                    var target = screenCenterPosition +
                                 (w - halfWidth) * unitHorizontalOffset + (halfHeight - h) * unitVerticalOffset;
                    var ray = new Ray(cameraPosition, target - cameraPosition);
                    var color = GetColor(ray, 5);
                    ret[w, h] = new Rgba32(color.R, color.G, color.B);
                }
            }
#else
            Parallel.For(0, ret.Width, w =>
            {
                for (var h = 0; h < ret.Height; h++)
                {
                    var target = screenCenterPosition +
                                 (w - halfWidth) * unitHorizontalOffset + (halfHeight - h) * unitVerticalOffset;
                    var ray = new Ray(cameraPosition, target - cameraPosition);
                    var color = GetColor(ray, 5);
                    ret[w, h] = new Rgba32(color.R, color.G, color.B);
                }
            });
#endif


            return ret;
        }
    }
}