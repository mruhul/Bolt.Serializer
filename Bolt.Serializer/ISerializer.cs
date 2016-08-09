using System.IO;

namespace Bolt.Serializer
{
    public interface ISerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string value);
        T Deserialize<T>(Stream value);

        bool IsSupported(string contentType);
    }
}
