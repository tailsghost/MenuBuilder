namespace MenuBuilder.Abstraction.Model;

public class MenuItemDto
{
    public string Id { get; set; } = string.Empty;
    public string ParentId { get; set; } = string.Empty;
    public string Title { get;set;  } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionShort { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsOpenChildren { get; set; }
    public List<MenuItemDto> Fields { get; set; } = [];
}
