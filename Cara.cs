using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace proyectoPG
{
    public class Cara
    {
        public Vector3 color;
        public List<Vertice> vertices;

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
            return vertices.Remove(vertice); // elimina si lo encuentra
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

        public uint[] GetIndices(int startIndex)
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
    }
} 