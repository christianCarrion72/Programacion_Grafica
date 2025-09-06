using System;
using System.IO;
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
        private int _shaderProgram;

        // Uniforms
        private int _uModel;
        private int _uView;
        private int _uProjection;

        // Camara
        private Camera _camera;

        // Rotación simple
        private float _angle = 0f;
        
        // Objeto 3D genérico cargado desde JSON
        private Objeto _objeto3D = new Objeto();

        // Ruta del archivo JSON
        private string _rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "objeto3d.json");

        public CubeWindow()
            : base(
                  GameWindowSettings.Default,
                  new NativeWindowSettings
                  {
                      ClientSize = new Vector2i(800, 600),
                      Title = "OpenTK - Objeto 3D desde JSON",
                  })
        { 
            _camera = new Camera(new Vector3(0.0f, 10.0f, 30.0f));
            CargarObjetoDesdeJson();
        }

        private void CargarObjetoDesdeJson()
        {
            _objeto3D = Serializer.CargarConFallback(_rutaJson, () => Computadora.CrearComputadora());
            
            Console.WriteLine($"Objeto 3D cargado desde: {_rutaJson}");
            //Console.WriteLine($"El objeto contiene {_objeto3D.CountPartes()} partes: {string.Join(", ", _objeto3D.GetParteNames())}");
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.1f, 0.15f, 1f);
            GL.Enable(EnableCap.DepthTest);

            // VAO
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

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

            // Uniforms
            _uModel = GL.GetUniformLocation(_shaderProgram, "model");
            _uView = GL.GetUniformLocation(_shaderProgram, "view");
            _uProjection = GL.GetUniformLocation(_shaderProgram, "projection");

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

            // F5: Recargar objeto desde JSON
            if (KeyboardState.IsKeyPressed(Keys.F5))
            {
                Console.WriteLine("Recargando objeto 3D desde JSON...");
                _objeto3D?.LiberarRecursos();
                CargarObjetoDesdeJson();
            }

            // F2: Guardar el objeto actual en JSON
            if (KeyboardState.IsKeyPressed(Keys.F2))
            {
                try
                {
                    if (_objeto3D != null)
                    {
                        Serializer.Guardar(_objeto3D, _rutaJson);
                        Console.WriteLine($"Objeto 3D guardado en: {_rutaJson}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar: {ex.Message}");
                }
            }

            // F3: Crear una nueva computadora y guardarla
            if (KeyboardState.IsKeyPressed(Keys.F3))
            {
                try
                {
                    var nuevaComputadora = Computadora.CrearComputadora();
                    Serializer.Guardar(nuevaComputadora, _rutaJson);
                    Console.WriteLine("Nueva computadora creada y guardada");
                    
                    _objeto3D?.LiberarRecursos();
                    CargarObjetoDesdeJson();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear nueva computadora: {ex.Message}");
                }
            }

            // F1: Mostrar ayuda
            if (KeyboardState.IsKeyPressed(Keys.F1))
            {
                Console.WriteLine("=== CONTROLES ===");
                Console.WriteLine("ESC: Salir");
                Console.WriteLine("F1: Mostrar esta ayuda");
                Console.WriteLine("F2: Guardar objeto actual");
                Console.WriteLine("F5: Recargar desde JSON");
                Console.WriteLine("F3: Crear nueva computadora");
                Console.WriteLine("WASD + Mouse: Controlar cámara");
            }

            // Actualizar la cámara
            _camera.HandleInput(KeyboardState, (float)args.Time);

            // Actualizar la matriz de vista
            var view = _camera.GetViewMatrix();
            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(_uView, false, ref view);

            // Rotación automática (opcional)
            _angle += (float)args.Time * 60f; 
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_shaderProgram);

            // Configurar matriz de modelo
            var model = Matrix4.CreateTranslation(0f, 0f, 0f);
            GL.UniformMatrix4(_uModel, false, ref model);

            // Dibujar el objeto 3D genérico
            _objeto3D?.Dibujar(_vao);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Liberar recursos del objeto
            _objeto3D?.LiberarRecursos();

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