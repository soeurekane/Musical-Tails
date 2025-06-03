using System;
using System.IO;
using System.Xml.Serialization;

namespace Model.Data
{
    public class XmlGameSerializer : SerializationBase<GameData>
    {
        public XmlGameSerializer() : base("highscores.xml") { }

        public override GameData Deserialize(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(GameData));
                using var reader = new StreamReader(filePath);
                var result = (GameData)serializer.Deserialize(reader);
                return result ?? new GameData { HighScores = Array.Empty<HighScore>() };
            }
            catch
            {
                return new GameData { HighScores = Array.Empty<HighScore>() };
            }
        }

        public override void Serialize(GameData data, string filePath)
        {
            var serializer = new XmlSerializer(typeof(GameData));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, data);
        }
    }
}