using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKCube
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var window = new CubeWindow();
            window.Run();
        }
    }
}
