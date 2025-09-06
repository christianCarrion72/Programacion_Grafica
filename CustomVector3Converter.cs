using System;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace proyectoPG
{
    public class CustomVector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("X");
            writer.WriteValue(value.X);
            writer.WritePropertyName("Y");
            writer.WriteValue(value.Y);
            writer.WritePropertyName("Z");
            writer.WriteValue(value.Z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            float x = 0, y = 0, z = 0;
            
            if (reader.TokenType == JsonToken.StartObject)
            {
                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = reader.Value?.ToString() ?? string.Empty;
                        reader.Read();
                        
                        switch (propertyName)
                        {
                            case "X":
                                x = Convert.ToSingle(reader.Value);
                                break;
                            case "Y":
                                y = Convert.ToSingle(reader.Value);
                                break;
                            case "Z":
                                z = Convert.ToSingle(reader.Value);
                                break;
                        }
                    }
                }
            }
            
            return new Vector3(x, y, z);
        }
    }
}