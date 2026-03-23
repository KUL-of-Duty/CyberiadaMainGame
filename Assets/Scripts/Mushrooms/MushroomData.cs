using Unity.Netcode;

public struct MushroomData : INetworkSerializable
{
    public int id;
    public int power;
    public float size;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref id);
        serializer.SerializeValue(ref power);
        serializer.SerializeValue(ref size);
    }
}