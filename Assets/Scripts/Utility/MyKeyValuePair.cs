[System.Serializable]
public class MyKeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public MyKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
