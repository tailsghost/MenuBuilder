using System.IO;
using System.Text.Json.Serialization;

namespace MenuBuilder.Abstraction.Model;

public abstract class MenuInfo
{
    public string Path { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    [JsonIgnore]
    public MenuDirectoryInfo Parent { get; set; }

    public string? Id { get; set; }

    public bool Update(string title)
    {
        try
        {
            var newName = this is MenuFileInfo ? $"{title}.json" : title;

            if (Name == newName)
                return false;

            var directory = System.IO.Path.GetDirectoryName(Path)!;
            var newPath = System.IO.Path.Combine(directory, newName);

            if (this is MenuDirectoryInfo)
            {
                Directory.Move(Path, newPath);
            }
            else if (this is MenuFileInfo)
            {
                System.IO.File.Move(Path, newPath);
            }

            Path = newPath;
            Name = newName;

            return true;
        }
        catch
        {
            return false;
        }
    }

    protected MenuInfo(string path)
    {
        Path = path;
        Name = System.IO.Path.GetFileName(path);
    }
}
