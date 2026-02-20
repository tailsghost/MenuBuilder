using System.Drawing;

namespace MenuBuilder.Abstraction.Model;

public class ColorTypesConstants
{

    public static Color ANY = Color.White;
    public static Color BOOL = Color.AliceBlue;
    public static Color INT8 = Color.DeepSkyBlue;
    public static Color UINT8 = Color.LightSkyBlue;
    public static Color INT16 = Color.Cornsilk;
    public static Color UINT16 = Color.LightCoral;
    public static Color INT32 = Color.AntiqueWhite;
    public static Color UINT32 = Color.Aquamarine;
    public static Color INT64 = Color.Beige;
    public static Color UINT64 = Color.Aqua;
    public static Color TIME = Color.DarkTurquoise;
    public static Color STRING = Color.Moccasin;
    public static Color FLOAT = Color.CadetBlue;
    public static Color DATE = Color.DarkKhaki;
    public static Color NONE = Color.Red;
    public static Color ANY_NUM = Color.DarkSeaGreen;
    public static Color ANY_REAL = Color.GreenYellow;


    public static Dictionary<string, Color> ColorsConvert = new()
    {
        { AllowedTypeConstants.ANY, ANY },
        { AllowedTypeConstants.BOOL, BOOL },
        { AllowedTypeConstants.SHORT, INT16 },
        { AllowedTypeConstants.USHORT, INT16 },
        { AllowedTypeConstants.INT16, INT16 },
        { AllowedTypeConstants.INT32, INT32 },
        { AllowedTypeConstants.INT, INT32 },
        { AllowedTypeConstants.UINT32, UINT32 },
        { AllowedTypeConstants.UINT, UINT32 },
        { AllowedTypeConstants.INT64, INT64 },
        { AllowedTypeConstants.LONG, INT64 },
        { AllowedTypeConstants.UINT64, UINT64 },
        { AllowedTypeConstants.Ulong, UINT64 },
        { AllowedTypeConstants.TIME, TIME },
        {AllowedTypeConstants.STRING, STRING },
        { AllowedTypeConstants.FLOAT, FLOAT },
        { AllowedTypeConstants.DATE, DATE },
        { AllowedTypeConstants.NONE, NONE },
        { AllowedTypeConstants.ANY_NUM, ANY_NUM },
        { AllowedTypeConstants.ANY_REAL, ANY_REAL },
         { AllowedTypeConstants.BYTE, UINT8 },
        { AllowedTypeConstants.REAL, ANY_REAL },
    };
}
