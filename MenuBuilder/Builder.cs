using MenuBuilder.Abstraction;
using MenuBuilder.Abstraction.Model;
using Newtonsoft.Json;
using System.IO;
using FluentTreeMenu.ViewModels;
using Wpf.Ui.Controls;

namespace MenuBuilder;

public class Builder
{
    private readonly string _mainMenuPath;
    private MenuDirectoryInfo _directory;
    private MenuItemDto _items;

    public Builder(string mainMenuPath)
    {
        _mainMenuPath = mainMenuPath;
        DirectoryHelper.Add += BuilderHelper.Add;
        _directory = DirectoryHelper.GetDirectoryChild(mainMenuPath);
        DirectoryHelper.Add -= BuilderHelper.Add;
    }

    public MenuDirectoryInfo GetDirectory() => _directory;

    public void CreateMenuItem()
    {
        var menuItem = new MenuItemDto
        {
            Name = _directory.Name
        };
        foreach (var child in _directory.Children)
        {
            var item = GetMenuItem(child, null);
            if (item == null) continue;
            menuItem.Fields.Add(item);
        }

        _items = menuItem;
    }

    public void CreateMenu(IList<FluentTreeMenuList> parentList, FluentTreeMenuList? topList, FluentTreeMenuViewModel owner)
    {
        var topMenu = new FluentTreeMenuList(_items.Name, SymbolRegular.Collections24);
        topMenu.TopParent = topMenu;
        topMenu.Owner = owner;
        topMenu.Description = _items.Description;
        if(topList == null)
        {
            parentList.Add(topMenu);
        }
        else
        {
            topList.AddChildrenList(topMenu);
        }
        CreateNode(_items.Fields, parentList, topMenu);
    }


    private void CreateNode(List<MenuItemDto> dtos, IList<FluentTreeMenuList> parentList, FluentTreeMenuList? parent = null)
    {
        foreach (var dto in dtos)
        {
            if (dto.Fields.Count == 0)
            {
                var item = new FluentTreeMenuItem(!string.IsNullOrEmpty(dto.AltName) ? dto.AltName : dto.Name, SymbolRegular.ItemCompare24, dto.AddingInclude, dto.IsUnique);
                item.Parent = parent;
                item.Description = dto.Description;
                item.TopParent = parent?.TopParent;
                parent?.AddChildrenItem(item);
            }
            else
            {
                var menu = new FluentTreeMenuList(!string.IsNullOrEmpty(dto.AltName) ? dto.AltName : dto.Name, SymbolRegular.Collections24, dto.AddingInclude, dto.IsUnique);
                menu.TopParent = parent?.TopParent;
                menu.Owner = parent?.Owner;
                menu.Description = dto.Description;
                parent?.AddChildrenList(menu);
                CreateNode(dto.Fields, parentList, menu);
            }
        }
    }

    

    private MenuItemDto GetMenuItem(MenuInfo info, MenuItemDto? parent)
    {
        if (info is not MenuDirectoryInfo dir)
            throw new InvalidOperationException("GetMenuItem expects directory");

        var nodeFile = FindNodeInfo(dir);

        MenuItemDto current;

        if (nodeFile != null)
        {
            var json = System.IO.File.ReadAllText(nodeFile.Path);
            current = JsonConvert.DeserializeObject<MenuItemDto>(json)
                      ?? new MenuItemDto();
            var name = Path.GetFileNameWithoutExtension(nodeFile.Name);
            current.Name = name;
            current.Title = name;

        }
        else
        {
            current = new MenuItemDto
            {
                Name = dir.Name,
                Title = dir.Name
            };
        }

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

    private MenuFileInfo? FindNodeInfo(MenuDirectoryInfo directory)
    {
        foreach (var child in directory.Children)
        {
            if (child is MenuFileInfo info)
            {
                if (System.IO.Path.GetExtension(info.Name) == ".json") return info;
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

    public void AddMenuItem()
    {

    }

    public void RemoveMenuItem()
    {

    }
}
