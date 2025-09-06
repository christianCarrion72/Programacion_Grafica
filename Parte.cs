using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace proyectoPG
{
    [JsonObject]
    public class Parte
    {
        [JsonProperty("centro")]
        public Vertice centro;

        [JsonProperty("caras")]
        public List<Cara> caras;

        public Parte()
        {
            centro = new Vertice(0, 0, 0);
            caras = new List<Cara>();
        }

        public Parte(float x, float y, float z)
        {
            centro = new Vertice(x, y, z);
            caras = new List<Cara>();
        }

        public Parte(Vertice centro)
        {
            this.centro = centro;
            caras = new List<Cara>();
        }

        public void AddCara(Cara cara)
        {
            // Crear una copia de la cara con vértices relativos al centro
            var caraRelativa = new Cara(cara.color);
            foreach (var vertice in cara.vertices)
            {
                var verticeRelativo = new Vertice(
                    vertice.X + centro.X,
                    vertice.Y + centro.Y,
                    vertice.Z + centro.Z
                );
                caraRelativa.AddVertice(verticeRelativo);
            }
            caras.Add(caraRelativa);
        }

        public void AddCara(float r, float g, float b, params Vertice[] vertices)
        {
            var cara = new Cara(r, g, b);
            foreach (var vertice in vertices)
            {
                var verticeRelativo = new Vertice(
                    vertice.X + centro.X,
                    vertice.Y + centro.Y,
                    vertice.Z + centro.Z
                );
                cara.AddVertice(verticeRelativo);
            }
            caras.Add(cara);
        }

        public bool RemoveCara(Cara cara)
        {
            return caras.Remove(cara);
        }

        public void RemoveCaraAt(int index)
        {
            if (index >= 0 && index < caras.Count)
                caras.RemoveAt(index);
        }

        public void ClearCaras()
        {
            caras.Clear();
        }

        public bool ContainsCara(Cara cara)
        {
            return caras.Contains(cara);
        }

        public Cara GetCaraAt(int index)
        {
            if (index >= 0 && index < caras.Count)
                return caras[index];
            throw new ArgumentOutOfRangeException(nameof(index), "Índice fuera de rango en la lista de caras.");
        }

        public void InsertCaraAt(int index, Cara cara)
        {
            if (index >= 0 && index <= caras.Count)
                caras.Insert(index, cara);
            else
                throw new ArgumentOutOfRangeException(nameof(index), "Índice inválido para insertar cara.");
        }

        public int CountCaras()
        {
            return caras.Count;
        }


        public float[] GetVertexData()
        {
            var data = new List<float>();
            foreach (var cara in caras)
            {
                data.AddRange(cara.GetVertexData());
            }
            return data.ToArray();
        }

        public uint[] GetIndices(int startIndex)
        {
            var indices = new List<uint>();
            int currentIndex = startIndex;

            foreach (var cara in caras)
            {
                var caraIndices = cara.GetIndices(currentIndex);
                indices.AddRange(caraIndices);
                currentIndex += cara.GetVertexCount();
            }

            return indices.ToArray();
        }

        public int GetTotalVertexCount()
        {
            int count = 0;
            foreach (var cara in caras)
            {
                count += cara.GetVertexCount();
            }
            return count;
        }

        public void Dibujar(int vao)
        {
            foreach (var cara in caras)
            {
                cara.Dibujar(vao);
            }
        }

        public void LiberarRecursos()
        {
            foreach (var cara in caras)
            {
                cara.LiberarRecursos();
            }
        }
    }
} 