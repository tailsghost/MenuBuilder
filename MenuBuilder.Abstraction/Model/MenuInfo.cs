using Newtonsoft.Json;

namespace MenuBuilder.Abstraction.Model;

public abstract class MenuInfo
{
    public string Path { get; } = string.Empty;
    public string Name { get; } = string.Empty;

    protected MenuInfo(string path)
    {
        Path = path;
        Name = System.IO.Path.GetFileName(path);
    }
}
