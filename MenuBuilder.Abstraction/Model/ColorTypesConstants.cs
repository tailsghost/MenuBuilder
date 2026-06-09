using System.Drawing;

namespace MenuBuilder.Abstraction.Model;

public class ColorTypesConstants
{
    public static readonly Color ANY = Color.FromArgb(0xEF, 0x44, 0x44);
    public static readonly Color BOOL = Color.FromArgb(0x3B, 0x82, 0xF6);
    public static readonly Color BYTE = Color.FromArgb(0x10, 0xB9, 0x81);
    public static readonly Color SBYTE = Color.FromArgb(0x2F, 0xC5, 0x8F);
    public static readonly Color INT = Color.FromArgb(0xF5, 0x9E, 0x0B);
    public static readonly Color UINT = Color.FromArgb(0xF6, 0xAE, 0x2C);
    public static readonly Color DINT = Color.FromArgb(0x8B, 0x5C, 0xF6);
    public static readonly Color UDINT = Color.FromArgb(0x9D, 0x74, 0xF7);
    public static readonly Color REAL = Color.FromArgb(0xF1, 0x61, 0x61);
    public static readonly Color DATE = Color.FromArgb(0x06, 0xB6, 0xD4);
    public static readonly Color STRING = Color.FromArgb(0x28, 0xC4, 0xDE);
    public static readonly Color DATETIME = Color.FromArgb(0xEC, 0x48, 0x99);
    public static readonly Color TIME = Color.FromArgb(0xDB, 0x27, 0x77);
    public static readonly Color NONE = Color.FromArgb(0x9C, 0xA3, 0xAF);

    public static Dictionary<string, Color> ColorsConvert = new()
    {
        { AllowedTypeConstants.ANY, ANY },
        { AllowedTypeConstants.BOOL, BOOL },
        { AllowedTypeConstants.SHORT, INT },
        { AllowedTypeConstants.USHORT, UINT },
        { AllowedTypeConstants.INT16, INT },
        { AllowedTypeConstants.INT, INT },
        { AllowedTypeConstants.UINT32, UINT },
        { AllowedTypeConstants.UINT, UINT },
        { AllowedTypeConstants.REAL, REAL },
        { AllowedTypeConstants.FLOAT, REAL },
        { AllowedTypeConstants.DATE, DATE },
         { AllowedTypeConstants.BYTE, BYTE },
         { AllowedTypeConstants.SBYTE, SBYTE },
         { AllowedTypeConstants.DATETIME, DATETIME },
         { AllowedTypeConstants.DT, DATETIME },
         { AllowedTypeConstants.TIME, TIME },
         { AllowedTypeConstants.DINT, DINT },
         { AllowedTypeConstants.UDINT, UDINT },
         { AllowedTypeConstants.STRING, STRING },
         {AllowedTypeConstants.NONE, NONE}
    };
}
