using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// JSONシリアライズ可能なVector3
/// </summary>
[System.Serializable]
public class SerializableVector3
{
    [JsonProperty("x")]
    public float X { get; set; }
    
    [JsonProperty("y")]
    public float Y { get; set; }
    
    [JsonProperty("z")]
    public float Z { get; set; }

    public SerializableVector3() { }
    
    public SerializableVector3(Vector3 vector)
    {
        X = vector.x;
        Y = vector.y;
        Z = vector.z;
    }

    public SerializableVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }

    public static implicit operator Vector3(SerializableVector3 serializableVector3)
    {
        return serializableVector3?.ToVector3() ?? Vector3.zero;
    }

    public static implicit operator SerializableVector3(Vector3 vector)
    {
        return new SerializableVector3(vector);
    }
}