using Newtonsoft.Json;

namespace MenuBuilder.Abstraction.Model;

public class NodeDto : MenuItemDto
{
    public string AltName { get; set; } = string.Empty;
    public bool IsUnique { get; set; } = false;
    public List<ConnectorDto> Inputs { get; set; } = [];
    public List<ConnectorDto> Outputs { get; set; } = [];
    public string NodeViewModelType { get; set; }
    public double Width { get; set; } = 0.0;
    public double Height { get; set; } = 0.0;


    [JsonIgnore]
    public NodeDto? Parent { get; set; } = null;

    [JsonIgnore]
    public object NodeViewModel { get; set; }

    [JsonIgnore]
    public bool IsParentHInit { get; set; }
    [JsonIgnore]
    public bool IsParentCInit { get; set; }
    [JsonIgnore]
    public bool IsParentBodyInit { get; set; }
}
