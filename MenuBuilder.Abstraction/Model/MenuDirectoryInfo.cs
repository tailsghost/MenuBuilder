namespace MenuBuilder.Abstraction.Model;

public class MenuDirectoryInfo : MenuInfo
{
    public List<MenuInfo> Children = [];
    public MenuDirectoryInfo(string path) : base(path)
    {
    }
}
