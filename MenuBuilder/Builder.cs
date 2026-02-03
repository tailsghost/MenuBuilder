using MenuBuilder.Abstraction;
using MenuBuilder.Abstraction.Model;
using Newtonsoft.Json;
using RadTreeView;
using RadTreeView.Commands;
using System.IO;
using System.Windows.Media.Imaging;

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
        var menuItem = new MenuItemDto();
        menuItem.Name = _directory.Name;
        foreach (var child in _directory.Children)
        {
            var item = GetMenuItem(child, null);
            if (item == null) continue;
            menuItem.Fields.Add(item);
        }

        _items = menuItem;
    }

    public void CreateMenu(IList<RowViewModelList> parentList, Func<RowViewModel, IEnumerable<Content>> getContent, Func<RowViewModel, IEnumerable<RadTreeCommand>> getCommand)
    {
        var topMenu = new RowViewModelList(_items.Fields.Count, parentList);
        topMenu.TopParent = topMenu;
        topMenu.Title = _items.Name;
        topMenu.Description = _items.Description;
        topMenu.AddContents(getContent(topMenu));
        topMenu.Image = new BitmapImage(
            new Uri("pack://application:,,,/FBDEditor;component/Assets/Project_Property_Icon.png"));
        topMenu.Commands = getCommand(topMenu).ToList();
        parentList.Add(topMenu);
        CreateNode(_items.Fields, parentList,getCommand,getContent, topMenu);
    }


    private void CreateNode(List<MenuItemDto> dtos, IList<RowViewModelList> parentList, Func<RowViewModelList, IEnumerable<RadTreeCommand>> getCommand, Func<RowViewModel, IEnumerable<Content>> getContent, RowViewModelList? parent = null)
    {
        foreach (var dto in dtos)
        {
            if (dto.Fields.Count == 0)
            {
                var item = new RowViewModelItem(dtos.Count, parentList, parent);
                item.Title = dto.Name;
                item.Description = dto.Description;
                item.AddContents(getContent(item));
                item.TopParent = parent?.TopParent;
                item.Image = new BitmapImage(
                    new Uri("pack://application:,,,/FBDEditor;component/Assets/TagItem.png"));
                parent.AddChildrenItem(item);
            }
            else
            {
                var menu = new RowViewModelList(dtos.Count, parentList, parent);
                menu.TopParent = parent?.TopParent;
                menu.Title = dto.Name;
                menu.Description = dto.Description;
                menu.AddContents(getContent(menu));
                menu.Image = new BitmapImage(
                    new Uri("pack://application:,,,/FBDEditor;component/Assets/Project_Property_Icon.png"));
                menu.Commands = getCommand(menu).ToList();
                parent.AddChildrenList(menu);
                CreateNode(dto.Fields, parentList,getCommand,getContent, menu);
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
