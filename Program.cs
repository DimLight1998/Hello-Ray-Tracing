using System;
using System.Drawing;
using System.Numerics;
using SixLabors.ImageSharp;

namespace HelloRayTracing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var blueMaterial = new Material(
                Color.DarkBlue.Multiply(0.5f),
                Color.DeepSkyBlue,
                Color.GhostWhite,
                Color.Black,
                1f, 0.05f
            );

            var blueRoughMaterial = new Material(
                Color.CornflowerBlue.Multiply(0.5f),
                Color.LightBlue,
                Color.White,
                Color.Black,
                1f, 0.08f
            );

            var greenRoughMaterial = new Material(
                Color.OliveDrab.Multiply(0.5f),
                Color.LightGreen,
                Color.OliveDrab,
                Color.Black,
                1f, 0.01f
            );

            var glassRedMaterial = new Material(
                Color.IndianRed.Multiply(0.5f),
                Color.OrangeRed,
                Color.Pink,
                Color.Black,
                32f, 0.20f
            );

            var yellowMaterial = new Material(
                Color.DarkOrange.Multiply(0.2f),
                Color.Yellow,
                Color.GhostWhite,
                Color.Black,
                1f, 0.05f
            );

            var silverMaterial = new Material(
                Color.DarkGray.Multiply(0.3f),
                Color.Silver,
                Color.White,
                Color.Black,
                128, 0.75f
            );

            var greyMaterial = new Material(
                Color.DimGray.Multiply(0.5f),
                Color.Azure,
                Color.White,
                Color.Black,
                1f, 0
            );

            var glassGreyMaterial = new Material(
                Color.DimGray.Multiply(0.5f),
                Color.LightGray,
                Color.White,
                Color.Black,
                1f, 0.7f
            );

            var leftWall = new PlaneEntity(blueMaterial, -Vector3.UnitX, 5);
            var rightWall = new PlaneEntity(yellowMaterial, Vector3.UnitX, 5);
            var ceiling = new PlaneEntity(greyMaterial, Vector3.UnitY, 10);
            var floor = new PlaneEntity(greyMaterial, -Vector3.UnitY, 0);
            var forwardWall = new PlaneEntity(glassGreyMaterial, new Vector3(0, 0.3f, -1), 12);
            var backwardWall = new PlaneEntity(greyMaterial, Vector3.UnitZ, 8);

            var ball1 = new SphereEntity(silverMaterial, new Vector3(-2, 2.5f, -5), 1);
            var ball2 = new SphereEntity(silverMaterial, new Vector3(-0.5f, 6, -4.7f), 0.8f);
            var ball3 = new SphereEntity(greenRoughMaterial, new Vector3(2.5f, 7, -6), 1.2f);
            var ball4 = new SphereEntity(glassRedMaterial, new Vector3(2f, 4, -3), 1.4f);
            var ball5 = new SphereEntity(blueRoughMaterial, new Vector3(-3, 5, -6), 1.33f);

            var pointLight2 =
                new PointLight(Color.Pink, Color.Pink, new Vector3(4f, 9f, -7f), 1f, 0.45f, 0.02f);
            var pointLight3 =
                new PointLight(Color.LightYellow, Color.YellowGreen, new Vector3(-3f, 7f, -6f), 1f, 0.3f, 0.04f);
            var spotLight1 = new SpotLight(
                Color.Azure, Color.White, new Vector3(0f, 10f, -4f), -Vector3.UnitY, 0.97f, 0.95f, 1, 0.3f, 0
            );

            var world = new World();
            world.Entities.AddRange(new Entity[]
            {
                leftWall, rightWall, ceiling, floor, forwardWall, backwardWall,
                ball1, ball2, ball3, ball4, ball5
            });
            world.Lights.AddRange(new Light[] {pointLight2, pointLight3, spotLight1});

            const int width = 1920 * 4;
            const int height = 1080 * 4;
            var image = world.GetRenderedImage(
                new Vector3(0, 5, 6.5f),
                new Vector3(0, 0, -1),
                Vector3.UnitY,
                (float) Math.PI / 3,
                (float) width / height,
                height
            );

            image.Save("image.png");
        }
    }
}