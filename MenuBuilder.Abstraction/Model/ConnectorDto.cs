using System.Drawing;

namespace MenuBuilder.Abstraction.Model;

public class ConnectorDto
{
    public Color Color { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string AltName { get; set; }
    public string ConnectorType { get; set; }
    public int Cell { get; set; }
    public double Width { get; set; }
    public bool IsTemp { get; set; }
    public double Height { get; set; }

    public ConnectorDto()
    {
    }
}
