namespace MenuBuilder.Abstraction.Model;

public class MenuDirectoryInfo : MenuInfo, IDisposable
{
    public List<MenuInfo> Children = [];
    public MenuDirectoryInfo(string path) : base(path)
    {
    }

    public void Dispose()
    {
        foreach(var child in Children)
        {
            if(child is MenuDirectoryInfo directory)
            {
                directory.Dispose();
            }
        }
        Children?.Clear();
    }
}
