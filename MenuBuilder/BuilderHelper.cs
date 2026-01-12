using MenuBuilder.Abstraction.Model;
using Newtonsoft.Json.Linq;

namespace MenuBuilder;

public static class BuilderHelper
{
    private static Dictionary<string, List<MenuInfo>> _duplicates = [];

    public static void Add(MenuInfo obj)
    {
        if (_duplicates.ContainsKey(obj.Name))
        {
            _duplicates[obj.Name].Add(obj);
        }
        else
        {
            _duplicates.Add(obj.Name, [obj]);
        }
    }

    public static Dictionary<string, List<MenuInfo>>  GetDuplicates() => _duplicates;

    public static void Clear()
    {
        _duplicates.Clear();
    }
}
