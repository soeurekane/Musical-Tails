using System;
using System.IO;
using System.Text.Json;

namespace Model.Data
{
    public class JsonGameSerializer : SerializationBase<HighScore[]>
    {
        public JsonGameSerializer(string fileName) : base(fileName) { }

        public override HighScore[] Deserialize(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<HighScore[]>(json) ?? Array.Empty<HighScore>();
            }
            catch
            {
                return Array.Empty<HighScore>();
            }
        }

        public override void Serialize(HighScore[] data, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
    }
}