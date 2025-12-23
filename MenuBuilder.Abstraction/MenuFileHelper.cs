using MenuBuilder.Abstraction.Model;
using System.IO;

namespace MenuBuilder.Abstraction;

public static class MenuFileHelper
{
    public static Model.File CreateFile(MenuFileInfo info)
    {
        return Path.GetExtension(info.Name) switch
        {
            "h" => new HFile(info),
            "c" => new CFile(info),
            "json" => new JsonFIle(info),
            _ => throw new NotImplementedException()
        };
    }

    public static bool IsJsonFile(MenuInfo info)
    {
        var extension = Path.GetExtension(info.Name);
        return info is MenuFileInfo && extension == ".json"; 
    }
}
