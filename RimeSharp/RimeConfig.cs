using System.Runtime.InteropServices;

namespace RimeSharp;

[StructLayout(LayoutKind.Sequential)]
public struct RimeConfig : IDisposable
{
    private readonly UIntPtr _ptr;

    public bool? GetBool(string key)
    {
        var api = Rime.Instance();
        return api.ConfigGetBool(ref this, key);
    }

    public int? GetInt(string key)
    {
        var api = Rime.Instance();
        return api.ConfigGetInt(ref this, key);
    }

    public double? GetDouble(string key)
    {
        var api = Rime.Instance();
        return api.ConfigGetDouble(ref this, key);
    }

    public string? GetString(string key)
    {
        var api = Rime.Instance();
        return api.ConfigGetString(ref this, key);
    }

    public T[] GetList<T>(string key, Func<RimeConfig, string, T?> getFunc)
    {
        var api = Rime.Instance();
        var iter = new RimeConfigIterator();
        if (!api.ConfigBeginList(ref iter, ref this, key))
            return [];
        var size = api.ConfigListSize(ref this, key);
        var result = new T[size];
        var i = 0;
        while (api.ConfigNext(ref iter))
        {
            var value = getFunc(this, iter.Path);
            if (value != null) result[i++] = value;
        }

        api.ConfigEnd(ref iter);
        return result;
    }

    public Dictionary<string, T> GetMap<T>(string key, Func<RimeConfig, string, T?> getFunc)
    {
        var api = Rime.Instance();
        var iter = new RimeConfigIterator();
        if (!api.ConfigBeginMap(ref iter, ref this, key))
            return [];
        var result = new Dictionary<string, T>();
        while (api.ConfigNext(ref iter))
        {
            var value = getFunc(this, iter.Path);
            if (value != null) result.Add(iter.Key, value);
        }

        api.ConfigEnd(ref iter);
        return result;
    }

    public void Dispose()
    {
        Rime.Instance().ConfigClose(ref this);
    }
}

[StructLayout(LayoutKind.Sequential)]
internal struct RimeConfigIterator
{
    private readonly UIntPtr _list;
    private readonly UIntPtr _map;
    public int Index;
    private readonly IntPtr _key;
    private readonly IntPtr _path;

    public string Key => UTF8Marshal.PtrToStringUTF8(_key);
    public string Path => UTF8Marshal.PtrToStringUTF8(_path);
}