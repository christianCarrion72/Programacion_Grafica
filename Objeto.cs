using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace proyectoPG
{
    [JsonObject]
    public class Objeto
    {
        [JsonProperty("centro")]
        public Vertice centro;

        [JsonProperty("partes")]
        public Dictionary<string, Parte> partes;

        public Objeto()
        {
            centro = new Vertice(0, 0, 0);
            partes = new Dictionary<string, Parte>();
        }

        public Objeto(float x, float y, float z)
        {
            centro = new Vertice(x, y, z);
            partes = new Dictionary<string, Parte>();
        }

        public Objeto(Vertice centro)
        {
            this.centro = centro;
            partes = new Dictionary<string, Parte>();
        }

        public void AddParte(string name, Parte parte)
        {
            // Crear una copia de la parte con centro relativo al objeto
            var parteRelativa = new Parte(
                parte.centro.X + centro.X,
                parte.centro.Y + centro.Y,
                parte.centro.Z + centro.Z
            );

            // Copiar todas las caras de la parte
            foreach (var cara in parte.caras)
            {
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
                parteRelativa.caras.Add(caraRelativa);
            }

            partes.Add(name, parteRelativa);
        }

        public bool RemoveParte(string name)
        {
            return partes.Remove(name);
        }

        public void ClearPartes()
        {
            partes.Clear();
        }

        public bool ContainsParte(string name)
        {
            return partes.ContainsKey(name);
        }

        public Parte GetParte(string name)
        {
            if (partes.TryGetValue(name, out var parte))
                return parte;

            throw new KeyNotFoundException($"La parte con nombre '{name}' no existe en el objeto.");
        }

        public int CountPartes()
        {
            return partes.Count;
        }

        public List<string> GetParteNames()
        {
            return new List<string>(partes.Keys);
        }

        public List<Parte> GetAllPartes()
        {
            return new List<Parte>(partes.Values);
        }

        public void Dibujar(int vao)
        {
            foreach (var parte in partes.Values)
            {
                parte.Dibujar(vao);
            }
        }

        public void DibujarParte(string nombreParte, int vao)
        {
            if (partes.TryGetValue(nombreParte, out var parte))
            {
                parte.Dibujar(vao);
            }
        }

        public void LiberarRecursos()
        {
            foreach (var parte in partes.Values)
            {
                parte.LiberarRecursos();
            }
        }
    }
} 