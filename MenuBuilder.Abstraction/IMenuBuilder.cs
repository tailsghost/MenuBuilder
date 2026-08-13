using MenuBuilder.Abstraction.Model;

namespace MenuBuilder.Abstraction;

public interface IMenuBuilder<T, J> : IDisposable
{
    MenuDirectoryInfo CreateMenuItem(bool isNodes = true);
    void CreateMenu(IList<T> parentList, T? topList, J owner);
    bool AddMenuDirectory(MenuInfo parent, string name);

    public bool AddMenuItem(MenuInfo parent, string saveItem, string name);

    public bool Remove(MenuInfo item);

    public bool RemoveMenuDirectory(MenuDirectoryInfo directory);

    public bool RemoveMenuItem(MenuFileInfo file);

    public MenuInfo? FindItem(string name);
    bool RenameItem(MenuInfo item, string newTitle);

    void AddId(MenuInfo info, object element, string id);
}
