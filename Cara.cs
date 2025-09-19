using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using Newtonsoft.Json;

namespace proyectoPG
{
    [JsonObject]
    public class Cara
    {
        [JsonProperty("color")]
        public Vector3 color;

        [JsonProperty("vertices")]
        public List<Vertice> vertices;

        [JsonIgnore]
        private int _vbo;
        [JsonIgnore]
        private int _ebo;
        [JsonIgnore]
        private bool _buffersInitialized = false;

        public Cara()
        {
            vertices = new List<Vertice>();
            color = new Vector3(0.5f, 0.5f, 0.5f); // Gris por defecto
        }

        public Cara(Vector3 color)
        {
            vertices = new List<Vertice>();
            this.color = color;
        }

        public Cara(float r, float g, float b)
        {
            vertices = new List<Vertice>();
            this.color = new Vector3(r, g, b);
        }

        public void AddVertice(float x, float y, float z)
        {
            vertices.Add(new Vertice(x, y, z));
        }

        public void AddVertice(Vertice vertice)
        {
            vertices.Add(vertice);
        }

        public bool RemoveVertice(Vertice vertice)
        {
            return vertices.Remove(vertice);
        }

        public void RemoveVerticeAt(int index)
        {
            if (index >= 0 && index < vertices.Count)
                vertices.RemoveAt(index);
        }

        public void ClearVertices()
        {
            vertices.Clear();
        }

        public bool ContainsVertice(Vertice vertice)
        {
            return vertices.Contains(vertice);
        }

        public Vertice GetVerticeAt(int index)
        {
            if (index >= 0 && index < vertices.Count)
                return vertices[index];
            throw new ArgumentOutOfRangeException(nameof(index), "Índice fuera de rango en la lista de vértices.");
        }

        public float[] GetVertexData()
        {
            var data = new List<float>();
            foreach (var vertice in vertices)
            {
                // Posición (X, Y, Z)
                data.Add(vertice.X);
                data.Add(vertice.Y);
                data.Add(vertice.Z);

                // Color (R, G, B)
                data.Add(color.X);
                data.Add(color.Y);
                data.Add(color.Z);
            }
            return data.ToArray();
        }

        public uint[] GetIndices(int startIndex = 0)
        {
            var indices = new List<uint>();

            // Para un polígono de 4 vértices, crear 2 triángulos
            if (vertices.Count == 4)
            {
                // Primer triángulo
                indices.Add((uint)startIndex);
                indices.Add((uint)(startIndex + 1));
                indices.Add((uint)(startIndex + 2));

                // Segundo triángulo
                indices.Add((uint)startIndex);
                indices.Add((uint)(startIndex + 2));
                indices.Add((uint)(startIndex + 3));
            }
            else if (vertices.Count == 3)
            {
                // Triángulo simple
                indices.Add((uint)startIndex);
                indices.Add((uint)(startIndex + 1));
                indices.Add((uint)(startIndex + 2));
            }

            return indices.ToArray();
        }

        public int GetVertexCount()
        {
            return vertices.Count;
        }

        private void InitializeBuffers()
        {
            if (_buffersInitialized || vertices.Count == 0)
                return;

            //Generar VBO
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            var vertexData = GetVertexData();
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StaticDraw);

            // Generar EBO
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            var indices = GetIndices();
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            _buffersInitialized = true;
        }

        public void Dibujar(int vao)
        {
            if (vertices.Count == 0)
                return;

            InitializeBuffers();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

            // Configurar atributos de vértice
            int stride = (3 + 3) * sizeof(float);

            // Posición (location = 0)
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            // Color (location = 1)
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Dibujar elementos
            var indices = GetIndices();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void LiberarRecursos()
        {
            if (_buffersInitialized)
            {
                GL.DeleteBuffer(_vbo);
                GL.DeleteBuffer(_ebo);
                _buffersInitialized = false;
            }
        }

        /*~Cara()
        {

        }*/
        public void Rotar(Matrix4 matrizRotacion)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i].ToVector3();
                v = Vector3.TransformPosition(v, matrizRotacion); 
                vertices[i] = new Vertice(v.X, v.Y, v.Z);
            }
            _buffersInitialized = false;
        }

        public void Trasladar(Vector3 traslacion)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = new Vertice(
                    vertices[i].X + traslacion.X,
                    vertices[i].Y + traslacion.Y,
                    vertices[i].Z + traslacion.Z
                );
            }
            _buffersInitialized = false;
        }

        public void Escalar(Vector3 escala)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = new Vertice(
                    vertices[i].X * escala.X,
                    vertices[i].Y * escala.Y,
                    vertices[i].Z * escala.Z
                );
            }
            _buffersInitialized = false;
        }

        public void Reflejar(Vector3 eje)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = new Vertice(
                    vertices[i].X * eje.X,
                    vertices[i].Y * eje.Y,
                    vertices[i].Z * eje.Z
                );
            }
            _buffersInitialized = false;
        }
    }
} 