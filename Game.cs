using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using proyectoPG;

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
        
        // Objeto computadora usando la nueva estructura
        private Objeto _computadora;

        public CubeWindow()
            : base(
                  GameWindowSettings.Default,
                  new NativeWindowSettings
                  {
                      ClientSize = new Vector2i(800, 600),
                      Title = "OpenTK - Computadora 3D - Estructurado",
                  })
        { 
            _camera = new Camera(new Vector3(0.0f, 10.0f, 30.0f));
            _computadora = Computadora.CrearComputadora();
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
            
            // Obtener datos de vértices de la computadora
            var vertices = _computadora.GetVertexData();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // EBO
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            
            // Obtener índices de la computadora
            var indices = _computadora.GetIndices(0);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

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
            var indices = _computadora.GetIndices(0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

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
