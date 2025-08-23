using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKCube
{
    public class CubeWindow : GameWindow
    {
        // Buffers / Shaders
        private int _vao;
        private int _vbo;
        private int _ebo;
        private int _shaderProgram;

        // Uniforms
        private int _uModel;
        private int _uView;
        private int _uProjection;

        //Camara
        private Camera _camera;

        // Rotación simple
        private float _angle = 0f;
        private readonly float[] _vertices =
        {
            // ===== MONITOR =====
            // Pantalla (frente) - Negro
            -8f, 2f, 2f,     0f, 0f, 0f,    // 0
            8f, 2f, 2f,      0f, 0f, 0f,    // 1
            8f, 12f, 2f,     0f, 0f, 0f,    // 2
            -8f, 12f, 2f,    0f, 0f, 0f,    // 3
            
            // Marco del monitor (trasero) - Gris oscuro
            -5.5f, 4.5f, -3f,   0.3f, 0.3f, 0.3f,  // 4
            5.5f, 4.5f, -3f,    0.3f, 0.3f, 0.3f,  // 5
            5.5f, 10.5f, -3f,   0.3f, 0.3f, 0.3f,  // 6
            -5.5f, 10.5f, -3f,  0.3f, 0.3f, 0.3f,  // 7
            
            // Base del monitor - Gris
            -2f, 0f, -1f,    0.5f, 0.5f, 0.5f,    // 8
            2f, 0f, -1f,     0.5f, 0.5f, 0.5f,    // 9
            2f, 4f, -1f,     0.5f, 0.5f, 0.5f,    // 10
            -2f, 4f, -1f,    0.5f, 0.5f, 0.5f,    // 11
            
            -2f, 0f, 3f,     0.5f, 0.5f, 0.5f,    // 12
            2f, 0f, 3f,      0.5f, 0.5f, 0.5f,    // 13
            2f, 1.5f, 3f,      0.5f, 0.5f, 0.5f,    // 14
            -2f, 1.5f, 3f,     0.5f, 0.5f, 0.5f,    // 15

            // ===== TECLADO =====
            // Teclado principal - Blanco/Gris claro
            -6f, 0.2f, 4f,   0.9f, 0.9f, 0.9f,    // 16
            6f, 0.2f, 4f,    0.9f, 0.9f, 0.9f,    // 17
            6f, 0.2f, 8f,    0.9f, 0.9f, 0.9f,    // 18
            -6f, 0.2f, 8f,   0.9f, 0.9f, 0.9f,    // 19
            
            -6f, 0f, 4f,     0.8f, 0.8f, 0.8f,    // 20
            6f, 0f, 4f,      0.8f, 0.8f, 0.8f,    // 21
            6f, 0f, 8f,      0.8f, 0.8f, 0.8f,    // 22
            -6f, 0f, 8f,     0.8f, 0.8f, 0.8f,    // 23

            // ===== CPU =====
            // Frente de la CPU - Negro/Gris oscuro
            12f, 0f, -3f,    0.2f, 0.2f, 0.2f,    // 24
            16f, 0f, -3f,    0.2f, 0.2f, 0.2f,    // 25
            16f, 12f, -3f,   0.2f, 0.2f, 0.2f,    // 26
            12f, 12f, -3f,   0.2f, 0.2f, 0.2f,    // 27
            
            // Trasero de la CPU
            12f, 0f, 5f,     0.15f, 0.15f, 0.15f, // 28
            16f, 0f, 5f,     0.15f, 0.15f, 0.15f, // 29
            16f, 12f, 5f,    0.15f, 0.15f, 0.15f, // 30
            12f, 12f, 5f,    0.15f, 0.15f, 0.15f, // 31

            // ===== MESA =====
            // Mesa - Marrón claro
            -20f, -0.5f, -10f,  0.6f, 0.4f, 0.2f,  // 32
            20f, -0.5f, -10f,   0.6f, 0.4f, 0.2f,  // 33
            20f, -0.5f, 15f,    0.6f, 0.4f, 0.2f,  // 34
            -20f, -0.5f, 15f,   0.6f, 0.4f, 0.2f,  // 35
            
            -20f, -1f, -10f,    0.5f, 0.3f, 0.15f, // 36
            20f, -1f, -10f,     0.5f, 0.3f, 0.15f, // 37
            20f, -1f, 15f,      0.5f, 0.3f, 0.15f, // 38
            -20f, -1f, 15f,     0.5f, 0.3f, 0.15f, // 39
        };

        private readonly uint[] _indices =
        {
            // MONITOR
            // Pantalla (frente)
            0, 1, 2,    2, 3, 0,
            // Marco trasero
            4, 5, 6,    6, 7, 4,
            // Laterales del monitor
            0, 4, 7,    7, 3, 0,    // Izquierda
            1, 5, 6,    6, 2, 1,    // Derecha
            0, 1, 5,    5, 4, 0,    // Inferior
            3, 2, 6,    6, 7, 3,    // Superior
            
            // Base del monitor
            // Cara superior
            11, 10, 14,  14, 15, 11,
            // Cara inferior
            8, 9, 13,    13, 12, 8,
            // Laterales
            8, 11, 15,   15, 12, 8,    // Izquierda
            9, 10, 14,   14, 13, 9,    // Derecha
            8, 9, 10,    10, 11, 8,    // Frente
            12, 13, 14,  14, 15, 12,   // Atrás

            // TECLADO
            // Cara superior
            16, 17, 18,  18, 19, 16,
            // Cara inferior
            20, 23, 22,  22, 21, 20,
            // Laterales
            16, 20, 23,  23, 19, 16,   // Izquierda
            17, 21, 22,  22, 18, 17,   // Derecha
            16, 17, 21,  21, 20, 16,   // Frente
            19, 18, 22,  22, 23, 19,   // Atrás

            // CPU
            // Frente
            24, 25, 26,  26, 27, 24,
            // Atrás
            28, 31, 30,  30, 29, 28,
            // Laterales
            24, 28, 31,  31, 27, 24,   // Izquierda
            25, 29, 30,  30, 26, 25,   // Derecha
            24, 25, 29,  29, 28, 24,   // Inferior
            27, 26, 30,  30, 31, 27,   // Superior

            // MESA
            // Cara superior
            32, 33, 34,  34, 35, 32,
            // Cara inferior
            36, 39, 38,  38, 37, 36,
            // Laterales
            32, 36, 39,  39, 35, 32,   // Izquierda
            33, 37, 38,  38, 34, 33,   // Derecha
            32, 33, 37,  37, 36, 32,   // Frente
            35, 34, 38,  38, 39, 35,   // Atrás
        };

        public CubeWindow()
            : base(
                  GameWindowSettings.Default,
                  new NativeWindowSettings
                  {
                      ClientSize = new Vector2i(800, 600),
                      Title = "OpenTK - Computadora 3D",
                  })
        { 
            _camera = new Camera(new Vector3(0.0f, 10.0f, 30.0f));
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.1f, 0.15f, 1f);
            GL.Enable(EnableCap.DepthTest);

            // VAO
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            // VBO
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // EBO
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Compilar shaders y crear programa
            string vertexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shaders", "shader.vert");
            string fragmentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shaders", "shader.frag");

            int vs = CreateShader(ShaderType.VertexShader, LoadShaderSource(vertexPath));
            int fs = CreateShader(ShaderType.FragmentShader, LoadShaderSource(fragmentPath));
            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vs);
            GL.AttachShader(_shaderProgram, fs);
            GL.LinkProgram(_shaderProgram);
            GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out int linked);
            if (linked == 0)
            {
                var log = GL.GetProgramInfoLog(_shaderProgram);
                throw new Exception($"Error al linkear programa: {log}");
            }
            GL.DetachShader(_shaderProgram, vs);
            GL.DetachShader(_shaderProgram, fs);
            GL.DeleteShader(vs);
            GL.DeleteShader(fs);

            GL.UseProgram(_shaderProgram);

            // Atributos de vértice: posición (loc=0) y color (loc=1)
            int stride = (3 + 3) * sizeof(float);

            // Posición
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            // Color (offset 3 floats)
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Uniforms
            _uModel = GL.GetUniformLocation(_shaderProgram, "model");
            _uView = GL.GetUniformLocation(_shaderProgram, "view");
            _uProjection = GL.GetUniformLocation(_shaderProgram, "projection");

            // Inicializar la cámara
            _camera = new Camera(new Vector3(0.0f, 10.0f, 30.0f));

            // Configurar la vista inicial
            var view = _camera.GetViewMatrix();
            GL.UniformMatrix4(_uView, false, ref view);

            // Matriz Projection
            var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f),
                                                                  Size.X / (float)Size.Y,
                                                                  0.1f, 100f);
            GL.UniformMatrix4(_uProjection, false, ref projection);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);

            // Actualizar proyección al redimensionar
            var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f),
                                                                  Size.X / (float)Size.Y,
                                                                  0.1f, 100f);
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(_uProjection, false, ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Tecla ESC para salir
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            // Actualizar la cámara
            _camera.HandleInput(KeyboardState, (float)args.Time);

            // Actualizar la matriz de vista
            var view = _camera.GetViewMatrix();
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(_uView, false, ref view);

            // Rotación automática
            _angle += (float)args.Time * 60f; 
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shaderProgram);
            GL.BindVertexArray(_vao);

            var model = Matrix4.CreateTranslation(0f, 0f, 0f);
            GL.UniformMatrix4(_uModel, false, ref model);

            // Dibujar
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteVertexArray(_vao);
            GL.DeleteProgram(_shaderProgram);
        }

        private static int CreateShader(ShaderType type, string source)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status == 0)
                throw new Exception($"Error compilando {type}: {GL.GetShaderInfoLog(shader)}");
            return shader;
        }

        private static string LoadShaderSource(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
