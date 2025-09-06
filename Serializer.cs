using System;
using System.IO;
using Newtonsoft.Json;

namespace proyectoPG
{
    public static class Serializer
    {
        public static void Guardar(Objeto objeto, string rutaArchivo)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                Converters = { new CustomVector3Converter() }
            };
            string json = JsonConvert.SerializeObject(objeto, settings);
            File.WriteAllText(rutaArchivo, json);
        }

        public static Objeto Cargar(string rutaArchivo)
        {
            string json = File.ReadAllText(rutaArchivo);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = { new CustomVector3Converter() }
            };
            return JsonConvert.DeserializeObject<Objeto>(json, settings) ?? new Objeto();
        }

        public static void CrearArchivo(Objeto objeto, string rutaArchivo)
        {
            Guardar(objeto, rutaArchivo);
        }

        public static bool ArchivoExiste(string rutaArchivo)
        {
            return File.Exists(rutaArchivo);
        }

        public static void CrearArchivoSiNoExiste(Objeto objeto, string rutaArchivo)
        {
            if (!ArchivoExiste(rutaArchivo))
            {
                CrearArchivo(objeto, rutaArchivo);
            }
        }

        public static void CrearComputadoraPorDefecto(string rutaArchivo)
        {
            var computadora = Computadora.CrearComputadora();
            CrearArchivo(computadora, rutaArchivo);
        }

        public static Objeto CargarConFallback(string rutaArchivo, Func<Objeto> objetoPorDefecto)
        {
            try
            {
                if (ArchivoExiste(rutaArchivo))
                {
                    return Cargar(rutaArchivo);
                }
                else
                {
                    var objeto = objetoPorDefecto();
                    CrearArchivo(objeto, rutaArchivo);
                    return objeto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar desde JSON: {ex.Message}");
                Console.WriteLine("Creando objeto por defecto...");
                return objetoPorDefecto();
            }
        }
    }
}