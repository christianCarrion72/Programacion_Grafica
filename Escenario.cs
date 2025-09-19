using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace proyectoPG
{
    [JsonObject]
    public class Escenario
    {
        private Dictionary<string, Objeto> objetos;

        public Escenario()
        {
            objetos = new Dictionary<string, Objeto>();
        }

        public void AddObjeto(string name, Objeto objeto)
        {
            objetos.Add(name, objeto);
        }

        public bool RemoveObjeto(string name)
        {
            return objetos.Remove(name);
        }

        public void ClearObjetos()
        {
            objetos.Clear();
        }

        public bool ContainsObjeto(string name)
        {
            return objetos.ContainsKey(name);
        }

        public Objeto GetObjeto(string name)
        {
            if (objetos.TryGetValue(name, out var objeto))
                return objeto;

            throw new KeyNotFoundException($"El objeto con nombre '{name}' no existe en el escenario.");
        }

        public void Dibujar(int vao)
        {
            foreach (var objeto in objetos.Values)
            {
                objeto.Dibujar(vao);
            }
        }

        public void DibujarObjeto(string nombreObjeto, int vao)
        {
            if (objetos.TryGetValue(nombreObjeto, out var objeto))
            {
                objeto.Dibujar(vao);
            }
        }

        public void LiberarRecursos()
        {
            foreach (var objeto in objetos.Values)
            {
                objeto.LiberarRecursos();
            }
        }

        public void Rotar(Matrix4 matrizRotacion)
        {
            foreach (var objeto in objetos.Values)
                objeto.Rotar(matrizRotacion);
        }

        public void Trasladar(Vector3 traslacion)
        {
            foreach (var objeto in objetos.Values)
                objeto.Trasladar(traslacion);
        }

        public void Escalar(Vector3 escala)
        {
            foreach (var objeto in objetos.Values)
                objeto.Escalar(escala);
        }

        public void Reflejar(Vector3 eje)
        {
            foreach (var objeto in objetos.Values)
                objeto.Reflejar(eje);
        }

        public int CountObjetos()
        {
            return objetos.Count;
        }

        public List<string> GetObjetoNames()
        {
            return new List<string>(objetos.Keys);
        }

        public List<Objeto> GetAllObjetos()
        {
            return new List<Objeto>(objetos.Values);
        }
    }
}
