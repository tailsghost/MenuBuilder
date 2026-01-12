using MenuBuilder.Abstraction;
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

    public static bool AnalysisDuplicates(out string error)
    {
        error = $"Обнаружены повторяющиеся файлы и папки!{Environment.NewLine}";
        var find = false;
        var count = 0;
        foreach (var item in GetDuplicates())
        {
            if (item.Value.Count > 1)
            {
                find = true;
                foreach (var it in item.Value)
                {
                    if (it is MenuDirectoryInfo dir)
                    {
                        error += $"Директория - {dir.Path + Environment.NewLine}";
                    }
                    else if (it is MenuFileInfo file)
                    {
                        error += $"Файл - {file.Path + Environment.NewLine}";
                    }
                    count++;
                }
            }
        }

        if (!find)
        {
            error = string.Empty;
        }
        else
        {
            error += $"Всего повторяющихся папок и файлов: {count}";
        }
        return find;
    }

    public static Dictionary<string, List<MenuInfo>>  GetDuplicates() => _duplicates;

    public static void Clear()
    {
        _duplicates.Clear();
    }
}
