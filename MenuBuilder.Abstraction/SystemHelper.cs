using System.IO;

namespace MenuBuilder.Abstraction;

public static class SystemHelper
{
    public static void AddDirectory(string directory)
    {
        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
    
    public static void RemoveDirectory(string directory)
    {
        if(Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }
    }

    public static void AddFile(string file)
    {
        if (!File.Exists(file))
        {
            File.Create(file);
        }
    }

    public static void AddFile(string data, string path)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, data);
        }
        else
        {
            if(new FileInfo(path).Length>0)
            {
                RemoveFile(path);
                AddFile(path);
                AddFile(data, path);
            }
        }
    }

    public static void RemoveFile(string file)
    {
        if (File.Exists(file))
        {
            File.Delete(file);
        }
    }
}
