using MenuBuilder.Abstraction;
using MenuBuilder.Abstraction.Model;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization.Metadata;
using FBDEditor.Abstractions.Model;
using FluentTreeMenu.ViewModels;
using Wpf.Ui.Controls;
using File = System.IO.File;

namespace MenuBuilder;

public class MenuBuilder : IMenuBuilder<FluentTreeMenuList, FluentTreeMenuViewModel>
{
    private readonly string _mainMenuPath;
    private MenuDirectoryInfo _directory;
    private MenuItemBase _items;
    private readonly Dictionary<Guid, object> _nodesById = [];

    private readonly JsonSerializerOptions _settings = new()
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver = JsonTypeInfoResolver.Combine(
            new DefaultJsonTypeInfoResolver()
        )
    };

    public MenuBuilder(string mainMenuPath)
    {
        _mainMenuPath = mainMenuPath;
    }

    public MenuDirectoryInfo CreateMenuItem(bool isNodes = true)
    {
        Dispose();
        DirectoryHelper.Add += BuilderHelper.Add;
        _directory = DirectoryHelper.GetDirectoryChild(_mainMenuPath);
        DirectoryHelper.Add -= BuilderHelper.Add;
        var menuItem = new MenuItemDirectory()
        {
            Name = _directory.Name
        };
        foreach (var child in _directory.Children)
        {
            if (!isNodes)
            {
                var item = GetMenuCompositeItem(child, null);
                if (item == null)
                {
                    continue;
                }
                menuItem.Fields.Add(item);
            }
            else
            {
                var item = GetMenuItem(child, null);
                if (item == null)
                {
                    continue;
                }
                menuItem.Fields.Add(item);
            }
        }

        _items = menuItem;
        return _directory;
    }

    public void CreateMenu(IList<FluentTreeMenuList> parentList, FluentTreeMenuList? topList, FluentTreeMenuViewModel owner)
    {
        var topMenu = new FluentTreeMenuList(_items.Name, SymbolRegular.Collections24);
        topMenu.TopParent = topList;
        topMenu.Owner = owner;

        if (topList == null)
        {
            parentList.Add(topMenu);
        }
        else
        {
            topList.AddChildrenList(topMenu);
        }
        if (_items is MenuItemDto dto)
        {
            topMenu.Description = dto.Description;
            topMenu.Id = Guid.Parse(dto.Id);
            _nodesById[topMenu.Id] = topMenu;
        }
        CreateNode(_items.Fields, parentList, topMenu);
    }

    private void CreateNode(List<MenuItemBase> dtos, IList<FluentTreeMenuList> parentList, FluentTreeMenuList? parent = null)
    {
        foreach (var dto in dtos)
        {
            if (dto is MenuItemDto itemDto)
            {
                if (itemDto.Fields.Count == 0)
                {
                    var item = new FluentTreeMenuItem(!string.IsNullOrEmpty(itemDto.AltName) ? itemDto.AltName : dto.Name, SymbolRegular.ItemCompare24, itemDto.AddingInclude, itemDto.IsUnique);
                    item.Parent = parent;
                    item.Id = Guid.Parse(itemDto.Id);
                    item.Description = itemDto.Description;
                    item.TopParent = parent?.TopParent;
                    parent?.AddChildrenItem(item);
                    _nodesById[item.Id] = item;
                }
                else
                {
                    var menu = new FluentTreeMenuList(!string.IsNullOrEmpty(itemDto.AltName) ? itemDto.AltName : dto.Name, SymbolRegular.Collections24, itemDto.AddingInclude, itemDto.IsUnique);
                    menu.TopParent = parent?.TopParent;
                    menu.Owner = parent?.Owner;
                    menu.Id = Guid.Parse(itemDto.Id);
                    menu.Description = itemDto.Description;
                    parent?.AddChildrenList(menu);
                    _nodesById[menu.Id] = menu;
                    CreateNode(dto.Fields, parentList, menu);
                }
            }
            else
            {
                var menu = new FluentTreeMenuList(dto.Name, SymbolRegular.Collections24, false, false);
                menu.TopParent = parent?.TopParent;
                menu.Owner = parent?.Owner;
                if (!string.IsNullOrEmpty(dto.Id))
                {
                    menu.Id = Guid.Parse(dto.Id);
                    _nodesById[menu.Id] = menu;
                }
                parent?.AddChildrenList(menu);
                CreateNode(dto.Fields, parentList, menu);
            }
        }
    }


    protected MenuItemBase GetMenuCompositeItem(MenuInfo info, MenuItemBase? parent)
    {
        if (info is MenuDirectoryInfo dir)
        {
            MenuItemBase current;
            MenuItemBase currentDir = default;
            var nodeFile = FindNodeInfo(dir);

            if (nodeFile != null)
            {
                var json = System.IO.File.ReadAllText(nodeFile.Path);
                current = JsonSerializer.Deserialize<MenuItemDto>(json, new JsonSerializerOptions()
                          {
                              PropertyNameCaseInsensitive = true
                          })
                          ?? new MenuItemDto();
                var name = Path.GetFileNameWithoutExtension(nodeFile.Name);
                current.Name = name;
                current.Title = name;

                nodeFile.Id = current.Id;

                if (parent != null)
                {
                    parent.Fields.Add(current);
                }
                else
                {
                    currentDir = new MenuItemDirectory()
                    {
                        Title = info.Name,
                        Name = info.Name,
                        Fields = []
                    };
                    currentDir.Fields.Add(current);
                }
            }

            if (parent != null && currentDir == null)
            {
                currentDir = parent;
            }
            if (currentDir == null && parent == null)
            {
                currentDir = new MenuItemDirectory()
                {
                    Title = info.Name,
                    Name = info.Name,
                    Fields = []
                };
            }

            foreach (var child in dir.Children)
            {
                if (child is MenuDirectoryInfo item)
                {
                    var itemDirectory = new MenuItemDirectory()
                    {
                        Title = item.Name,
                        Name = item.Name,
                        Fields = []
                    };
                    currentDir.Fields.Add(itemDirectory);
                    GetMenuCompositeItem(item, itemDirectory);
                }
            }

            return currentDir;
        }
        else
        {
            MenuItemBase current;
            MenuItemBase currentDir = default;
            var json = System.IO.File.ReadAllText(info.Path);
            current = System.Text.Json.JsonSerializer.Deserialize<MenuItemDto>(json)
                      ?? new MenuItemDto();
            var name = Path.GetFileNameWithoutExtension(info.Name);
            current.Name = name;
            current.Title = name;

            info.Id = current.Id;

            if (parent != null)
            {
                parent.Fields.Add(current);
            }

            return current;
        }
    }

    public bool RenameItem(MenuInfo item, string newTitle)
    {
        if (!item.Update(newTitle))
            return false;

        if (!string.IsNullOrEmpty(item.Id) && Guid.TryParse(item.Id, out var guid)
            && _nodesById.TryGetValue(guid, out var node))
        {
            switch (node)
            {
                case FluentTreeMenuItem treeItem:
                    treeItem.Rename(newTitle);
                    break;
                case FluentTreeMenuList treeList:
                    treeList.Rename(newTitle);
                    break;
            }
        }

        return true;
    }

    public void AddId(MenuInfo info, object element , string id)
    {
        if (!string.IsNullOrEmpty(info.Id))
        {
            if (!_nodesById.ContainsKey(Guid.Parse(info.Id)))
            {
                _nodesById[Guid.Parse(info.Id)] = element;
            }
        }
        else
        {

            if (!_nodesById.ContainsKey(Guid.Parse(id)))
            {
                if (element is FluentTreeMenuBase menu)
                {
                    _nodesById[Guid.Parse(id)] = element;
                }
                info.Id = id;
            }
        }
    }


    protected MenuItemBase GetMenuItem(MenuInfo info, MenuItemBase? parent)
    {
        if (info is not MenuDirectoryInfo dir)
            throw new InvalidOperationException("GetMenuItem expects directory");

        var nodeFile = FindNodeInfo(dir);

        MenuItemBase current;

        if (nodeFile != null)
        {
            var json = System.IO.File.ReadAllText(nodeFile.Path);
            current = System.Text.Json.JsonSerializer.Deserialize<MenuItemDto>(json, _settings)
                      ?? new MenuItemDto();
            var name = Path.GetFileNameWithoutExtension(nodeFile.Name);
            current.Name = name;
            current.Title = name;

            nodeFile.Id = current.Id;

            parent?.Fields.Add(current);

            foreach (var child in dir.Children)
            {
                if (child is MenuDirectoryInfo subDir)
                {
                    GetMenuItem(subDir, current);
                }
            }

            return current;
        }

        var currentDir = new MenuItemDirectory()
        {
            Title = info.Name,
            Name = info.Name,
            Fields = []
        };

        foreach (var child in dir.Children)
        {
            if (child is MenuDirectoryInfo item)
            {
                var result = GetMenuItem(item, currentDir);
                if (result is MenuItemDto) continue;
                currentDir.Fields.Add(result);
            }
        }

        return currentDir;
    }

    private MenuFileInfo? FindNodeInfo(MenuDirectoryInfo directory)
    {
        foreach (var child in directory.Children)
        {
            if (child is MenuFileInfo info)
            {
                if (Path.GetExtension(info.Name) == ".json") return info;
            }
        }

        return null;
    }

    public void SaveNode()
    {

    }

    public void AddNode()
    {

    }

    public void RemoveNode()
    {

    }

    public void SaveMenuItem()
    {

    }

    public bool AddMenuDirectory(string name)
    {
        try
        {
            if (!Directory.Exists(_directory.Path))
                Directory.CreateDirectory(_directory.Path);
            var path = Path.Combine(_directory.Path, name);
            Directory.CreateDirectory(path);
            var menuDir = new MenuDirectoryInfo(path);
            _directory.Children.Add(menuDir);
            menuDir.Parent = _directory;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AddMenuDirectory(MenuInfo parent, string name)
    {
        if (parent is not MenuDirectoryInfo directory) return false;

        try
        {
            if (!Directory.Exists(parent.Path))
                Directory.CreateDirectory(parent.Path);
            var path = Path.Combine(parent.Path, name);
            Directory.CreateDirectory(path);
            var menuDir = new MenuDirectoryInfo(path);
            directory.Children.Add(menuDir);
            menuDir.Parent = directory;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AddMenuItem(string saveItem, string name)
    {
        try
        {
            var dir = Path.Combine(_directory.Path, $"{name}.json");
            if (!Directory.Exists(_directory.Path))
            {
                Directory.CreateDirectory(_directory.Path);
            }

            var file = new MenuFileInfo(dir);
            _directory.Children.Add(file);
            file.Parent = _directory;
            File.WriteAllText(dir, saveItem);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool AddMenuItem(MenuInfo parent, string saveItem, string name)
    {
        if (parent is not MenuDirectoryInfo directory) return false;
        try
        {
            var dir = Path.Combine(parent.Path, $"{name}.json");
            if (!Directory.Exists(parent.Path))
            {
                Directory.CreateDirectory(parent.Path);
            }

            var file = new MenuFileInfo(dir);
            directory.Children.Add(new MenuFileInfo(dir));
            file.Parent = directory;
            File.WriteAllText(dir, saveItem);
            return true;
        }
        catch
        {
            return false;
        }

    }

    public bool Remove(MenuInfo item)
    {
        if (item is MenuDirectoryInfo directory)
        {
            return RemoveMenuDirectory(directory);
        }
        else if (item is MenuFileInfo file)
        {
            return RemoveMenuItem(file);
        }

        return false;
    }

    public bool RemoveMenuDirectory(MenuDirectoryInfo directory)
    {
        try
        {
            if (Directory.Exists(directory.Path))
                Directory.Delete(directory.Path, true);
            directory.Parent?.Children.Remove(directory);
            directory.Parent = null;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveMenuItem(MenuFileInfo file)
    {
        try
        {
            if (File.Exists(file.Path))
                File.Delete(file.Path);

            file.Parent?.Children.Remove(file);
            file.Parent = null;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public MenuInfo? FindItem(string name)
    {
        foreach (var info in _directory.Children)
        {
            var result = FindMenuInfo(name, info);
            if (result != null) return result;
        }

        return null;
    }

    private MenuInfo? FindMenuInfo(string name, MenuInfo info)
    {
        if (info is MenuDirectoryInfo directory)
        {
            if (directory.Name == name) return info;
            foreach (var child in directory.Children)
            {
                var result = FindMenuInfo(name, child);
                if (result != null) return result;
            }

            return null;
        }
        else
        {
            if (Path.GetFileNameWithoutExtension(info.Name) == name) return info;
        }

        return null;
    }

    public void Dispose()
    {
        _items?.Dispose();
        _directory?.Dispose();
        _directory = null;
        _items = null;
        _nodesById.Clear();
    }
}
