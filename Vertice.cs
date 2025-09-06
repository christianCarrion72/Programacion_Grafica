using System;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace proyectoPG
{
    [JsonObject]
    public class Vertice
    {
        [JsonProperty("x")]
        public float X { get; set; }
        [JsonProperty("y")]
        public float Y { get; set; }
        [JsonProperty("z")]
        public float Z { get; set; }

        public Vertice()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        public Vertice(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public static Vertice operator +(Vertice a, Vertice b)
        {
            return new Vertice(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vertice operator -(Vertice a, Vertice b)
        {
            return new Vertice(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
    }
} 