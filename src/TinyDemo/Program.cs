using System;
using System.IO;
using System.Numerics;
using Veldrid.Graphics;
using Veldrid.Platform;
using Veldrid.StartupUtilities;

namespace SampleExe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WindowCreateInfo windowCI = new WindowCreateInfo()
            {
                X = 100, Y = 100,
                WindowWidth = 960, WindowHeight = 540,
                WindowTitle = "Veldrid TinyDemo",
            };
            RenderContextCreateInfo contextCI = new RenderContextCreateInfo();
            VeldridStartup.CreateWindowAndRenderContext(ref windowCI, ref contextCI, out var window, out RenderContext rc);

            VertexBuffer vb = rc.ResourceFactory.CreateVertexBuffer(Cube.Vertices, new VertexDescriptor(VertexPositionColor.SizeInBytes, 2), false);
            IndexBuffer ib = rc.ResourceFactory.CreateIndexBuffer(Cube.Indices, false);

            string folder = rc.BackendType == GraphicsBackend.Direct3D11 ? "HLSL" : "GLSL";
            string extension = rc.BackendType == GraphicsBackend.Direct3D11 ? "hlsl" : "glsl";

            VertexInputLayout inputLayout = rc.ResourceFactory.CreateInputLayout(new VertexInputDescription[]
            {
                new VertexInputDescription(
                    new VertexInputElement("Position", VertexSemanticType.Position, VertexElementFormat.Float3),
                    new VertexInputElement("Color", VertexSemanticType.Color, VertexElementFormat.Float4))
            });

            string vsPath = Path.Combine(AppContext.BaseDirectory, folder, $"vertex.{extension}");
            string fsPath = Path.Combine(AppContext.BaseDirectory, folder, $"fragment.{extension}");

            Shader vs = rc.ResourceFactory.CreateShader(ShaderStages.Vertex, File.ReadAllText(vsPath));
            Shader fs = rc.ResourceFactory.CreateShader(ShaderStages.Fragment, File.ReadAllText(fsPath));

            ShaderSet shaderSet = rc.ResourceFactory.CreateShaderSet(inputLayout, vs, fs);
            ShaderResourceBindingSlots bindingSlots = rc.ResourceFactory.CreateShaderResourceBindingSlots(
                shaderSet,
                new ShaderResourceDescription("ViewProjectionMatrix", ShaderConstantType.Matrix4x4));
            ConstantBuffer viewProjectionBuffer = rc.ResourceFactory.CreateConstantBuffer(ShaderConstantType.Matrix4x4);

            while (window.Exists)
            {
                InputSnapshot snapshot = window.PumpEvents();
                rc.ClearBuffer();

                rc.SetViewport(0, 0, window.Width, window.Height);
                float timeFactor = Environment.TickCount / 1000f;
                viewProjectionBuffer.SetData(
                    Matrix4x4.CreateLookAt(
                        new Vector3(2 * (float)Math.Sin(timeFactor), (float)Math.Sin(timeFactor), 2 * (float)Math.Cos(timeFactor)),
                        Vector3.Zero,
                        Vector3.UnitY)
                        * Matrix4x4.CreatePerspectiveFieldOfView(1.05f, (float)window.Width / window.Height, .5f, 10f));
                rc.SetVertexBuffer(0, vb);
                rc.IndexBuffer = ib;
                rc.ShaderSet = shaderSet;
                rc.ShaderResourceBindingSlots = bindingSlots;
                rc.SetConstantBuffer(0, viewProjectionBuffer);
                rc.DrawIndexedPrimitives(Cube.Indices.Length);

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

            public static readonly ushort[] Indices =
                { 0, 1, 2, 0, 2, 3, 4, 6, 5, 4, 7, 6, 8, 9, 10, 8, 10, 11, 12, 14, 13, 12, 15, 14, 16, 17, 18, 16, 18, 19, 20, 21, 22, 20, 22, 23 };
        }
    }
}