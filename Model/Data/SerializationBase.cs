using System.IO;

namespace Model.Data
{
    public abstract class SerializationBase<T>
    {
        public string FilePath { get; protected set; }

        protected SerializationBase(string fileName)
        {
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            EnsureDirectoryExists();
        }

        private void EnsureDirectoryExists()
        {
            var directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public abstract void Serialize(T data, string filePath);
        public abstract T Deserialize(string filePath);
    }
}