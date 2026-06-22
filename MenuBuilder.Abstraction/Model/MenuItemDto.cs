namespace MenuBuilder.Abstraction.Model;

public class MenuItemDto
{
    public bool IsUnique { get; set; } = false;
    public string ParentId { get; set; } = string.Empty;
    public string Title { get;set;  } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionShort { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsOpenChildren { get; set; }
    public bool AddingInclude { get; set; } = false;
    public string AltName { get; set; } = string.Empty;
    public List<MenuItemDto> Fields { get; set; } = [];
}
