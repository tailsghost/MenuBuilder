using MenuBuilder.Abstraction.Model;
using System.IO;

namespace MenuBuilder.Abstraction;

public static class DirectoryHelper
{

    public static event Action<MenuInfo> Add;

    public static MenuDirectoryInfo GetDirectoryChild(string pathDirectory)
    {
        var menuDirectoryInfo = new MenuDirectoryInfo(pathDirectory);
        return (MenuDirectoryInfo)GetChild(menuDirectoryInfo);
    }

    private static MenuInfo GetChild(MenuDirectoryInfo parent)
    {
        var directories = Directory.GetDirectories(parent.Path);
        for (int i = 0; i < directories.Length; i++)
        {
            var dirPath = directories[i];

            var childDir = new MenuDirectoryInfo(dirPath);
            parent.Children.Add(childDir);
            Add?.Invoke(childDir);
            GetChild(childDir);
        }

        var files = Directory.GetFiles(parent.Path);
        for (int i = 0; i < files.Length; i++)
        {
            var filePath = files[i];
            var childFile = new MenuFileInfo(filePath);
            parent.Children.Add(childFile);
            Add?.Invoke(childFile);
        }

        return parent;
    }
}
