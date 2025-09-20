using System;
using System.IO;
using Newtonsoft.Json;

namespace proyectoPG
{
    public static class Serializer
    {
        // Guardar cualquier objeto en JSON
        public static void Guardar<T>(T objeto, string rutaArchivo)
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

        // Cargar objeto desde JSON
        public static T Cargar<T>(string rutaArchivo) where T : new()
        {
            string json = File.ReadAllText(rutaArchivo);
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = { new CustomVector3Converter() }
            };
            return JsonConvert.DeserializeObject<T>(json, settings) ?? new T();
        }

        // Métodos específicos para Objeto (para mantener compatibilidad)
        public static Objeto CargarObjeto(string rutaArchivo)
        {
            return Cargar<Objeto>(rutaArchivo);
        }

        // Métodos específicos para Escenario
        public static Escenario CargarEscenario(string rutaArchivo)
        {
            return Cargar<Escenario>(rutaArchivo);
        }

        // Crear archivo con un objeto específico (genérico)
        public static void CrearArchivo<T>(T objeto, string rutaArchivo)
        {
            Guardar(objeto, rutaArchivo);
        }

        // Verificar si el archivo existe
        public static bool ArchivoExiste(string rutaArchivo)
        {
            return File.Exists(rutaArchivo);
        }

        // Crear archivo solo si no existe
        public static void CrearArchivoSiNoExiste<T>(T objeto, string rutaArchivo)
        {
            if (!ArchivoExiste(rutaArchivo))
            {
                CrearArchivo(objeto, rutaArchivo);
            }
        }

        // Método de conveniencia para crear archivos con escenarios predefinidos
        public static void CrearEscenarioPorDefecto(string rutaArchivo)
        {
            var escenario = Computadora.CrearEscenarioComputadora();
            CrearArchivo(escenario, rutaArchivo);
        }

        // Cargar con manejo de errores y fallback para Objeto
        public static Objeto CargarObjetoConFallback(string rutaArchivo, Func<Objeto> objetoPorDefecto)
        {
            try
            {
                if (ArchivoExiste(rutaArchivo))
                {
                    return CargarObjeto(rutaArchivo);
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

        // Cargar con manejo de errores y fallback para Escenario
        public static Escenario CargarEscenarioConFallback(string rutaArchivo, Func<Escenario> escenarioPorDefecto)
        {
            try
            {
                if (ArchivoExiste(rutaArchivo))
                {
                    return CargarEscenario(rutaArchivo);
                }
                else
                {
                    var escenario = escenarioPorDefecto();
                    CrearArchivo(escenario, rutaArchivo);
                    return escenario;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar escenario desde JSON: {ex.Message}");
                Console.WriteLine("Creando escenario por defecto...");
                return escenarioPorDefecto();
            }
        }
    }
}