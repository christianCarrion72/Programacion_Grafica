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
        
        // Escenario cargado desde JSON
        private Escenario _escenario = new Escenario();

        // Ruta del archivo JSON
        private string _rutaJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "escenario.json");

        public CubeWindow()
            : base(
                  GameWindowSettings.Default,
                  new NativeWindowSettings
                  {
                      ClientSize = new Vector2i(800, 600),
                      Title = "OpenTK - Escenario 3D desde JSON",
                  })
        {
            _camera = new Camera(new Vector3(0.0f, 10.0f, 30.0f));
            CargarEscenarioDesdeJson();
        }

        private void CargarEscenarioDesdeJson()
        {
            _escenario = Serializer.CargarEscenarioConFallback(_rutaJson, () => Computadora.CrearEscenarioComputadora());
            
            Console.WriteLine($"Escenario 3D cargado desde: {_rutaJson}");
            Console.WriteLine($"El escenario contiene {_escenario.CountObjetos()} objetos");
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

            // F5: Recargar escenario desde JSON
            if (KeyboardState.IsKeyPressed(Keys.F5))
            {
                Console.WriteLine("Recargando escenario 3D desde JSON...");
                _escenario?.LiberarRecursos();
                CargarEscenarioDesdeJson();
            }

            // F2: Guardar el escenario actual en JSON
            if (KeyboardState.IsKeyPressed(Keys.F2))
            {
                try
                {
                    if (_escenario != null)
                    {
                        Serializer.Guardar(_escenario, _rutaJson);
                        Console.WriteLine($"Escenario 3D guardado en: {_rutaJson}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar: {ex.Message}");
                }
            }

            // F3: Crear un nuevo escenario y guardarlo
            if (KeyboardState.IsKeyPressed(Keys.F3))
            {
                try
                {
                    var nuevoEscenario = Computadora.CrearEscenarioComputadora();
                    Serializer.Guardar(nuevoEscenario, _rutaJson);
                    Console.WriteLine("Nuevo escenario creado y guardado");
                    
                    _escenario?.LiberarRecursos();
                    CargarEscenarioDesdeJson();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear nuevo escenario: {ex.Message}");
                }
            }
            var kb = KeyboardState;
            // Trasladar con flechas
            if (kb.IsKeyDown(Keys.KeyPad6))
                _escenario?.Trasladar(new Vector3(0.05f, 0f, 0f));
            if (kb.IsKeyDown(Keys.KeyPad4))
                _escenario?.Trasladar(new Vector3(-0.05f, 0f, 0f));
            if (kb.IsKeyDown(Keys.KeyPad8))
                _escenario?.Trasladar(new Vector3(0f, 0.05f, 0f));
            if (kb.IsKeyDown(Keys.KeyPad2))
                _escenario?.Trasladar(new Vector3(0f, -0.05f, 0f));

            // Escalar con teclado numérico + y -
            if (kb.IsKeyDown(Keys.KeyPadAdd))
                _escenario?.Escalar(new Vector3(1.01f, 1.01f, 1.01f));
            if (kb.IsKeyDown(Keys.KeyPadSubtract))
                _escenario?.Escalar(new Vector3(0.99f, 0.99f, 0.99f));

            // Rotar con R
            if (kb.IsKeyDown(Keys.R))
            {
                Matrix4 rotY = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(1f));
                _escenario?.Rotar(rotY);
            }

            // Reflejar con F
            if (kb.IsKeyPressed(Keys.F))
                _escenario?.Reflejar(new Vector3(-1f, 1f, 1f));

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

            _escenario?.Dibujar(_vao);

            SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            // Liberar recursos del escenario
            _escenario?.LiberarRecursos();

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