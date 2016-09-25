using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid.Graphics;
using Veldrid.Graphics.Direct3D;
using Veldrid.Graphics.OpenGL;
using Veldrid.Platform;

namespace SampleExe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            OpenTKWindow window = isWindows ? (OpenTKWindow)new DedicatedThreadWindow() : new SameThreadWindow();
            RenderContext rc = isWindows && !args.Contains("opengl")
                ? (RenderContext)new D3DRenderContext(window)
                : new OpenGLRenderContext(window);
            window.Title = "Veldrid TinyDemo";

            VertexBuffer vb = rc.ResourceFactory.CreateVertexBuffer(Cube.Vertices, new VertexDescriptor(VertexPositionColor.SizeInBytes, 2), false);
            IndexBuffer ib = rc.ResourceFactory.CreateIndexBuffer(Cube.Indices, false);

            DynamicDataProvider<Matrix4x4> viewProjection = new DynamicDataProvider<Matrix4x4>();
            Material material = rc.ResourceFactory.CreateMaterial(
                rc, "vertex", "fragment",
                new MaterialVertexInput(VertexPositionColor.SizeInBytes,
                    new MaterialVertexInputElement("Position", VertexSemanticType.Position, VertexElementFormat.Float3),
                    new MaterialVertexInputElement("Color", VertexSemanticType.Color, VertexElementFormat.Float4)),
                new MaterialInputs<MaterialGlobalInputElement>(
                    new MaterialGlobalInputElement("ViewProjectionMatrix", MaterialInputType.Matrix4x4, viewProjection)),
                MaterialInputs<MaterialPerObjectInputElement>.Empty,
                MaterialTextureInputs.Empty);

            while (window.Exists)
            {
                InputSnapshot snapshot = window.GetInputSnapshot();
                rc.ClearBuffer();

                rc.SetViewport(0, 0, window.Width, window.Height);
                float timeFactor = Environment.TickCount / 1000f;
                viewProjection.Data =
                    Matrix4x4.CreateLookAt(
                        new Vector3(2 * (float)Math.Sin(timeFactor), (float)Math.Sin(timeFactor), 2 * (float)Math.Cos(timeFactor)),
                        Vector3.Zero,
                        Vector3.UnitY)
                        * Matrix4x4.CreatePerspectiveFieldOfView(1.05f, (float)window.Width / window.Height, .5f, 10f);
                rc.SetVertexBuffer(vb);
                rc.SetIndexBuffer(ib);
                rc.SetMaterial(material);
                rc.DrawIndexedPrimitives(Cube.Indices.Length, 0);

                rc.SwapBuffers();
            }
        }

        public static class Cube
        {
            public static readonly VertexPositionColor[] Vertices =
            {
                // Front & Back
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), RgbaFloat.Red), new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), RgbaFloat.Red), new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), RgbaFloat.Red),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), RgbaFloat.Orange), new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), RgbaFloat.Orange),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), RgbaFloat.Orange), new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), RgbaFloat.Orange),

                // Top & Bottom
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), RgbaFloat.Yellow), new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), RgbaFloat.Yellow),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), RgbaFloat.Yellow), new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), RgbaFloat.Yellow),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), RgbaFloat.Green), new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), RgbaFloat.Green),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), RgbaFloat.Green), new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), RgbaFloat.Green),

                // Left & Right
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), RgbaFloat.Blue), new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), RgbaFloat.Blue), new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), RgbaFloat.Pink), new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), RgbaFloat.Pink),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), RgbaFloat.Pink), new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), RgbaFloat.Pink)
            };

            public static readonly int[] Indices =
                { 0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 8, 9, 10, 8, 10, 11, 12, 14, 13, 12, 15, 14, 16, 17, 18, 16, 18, 19, 20, 21, 22, 20, 22, 23 };
        }
    }
}