namespace Megafon.Domain.Helpers;

public static class ColorToIntExtension
{
    public static int ToInt(this Color color)
    {
        return ((color.R & 0x0ff) << 16) | ((color.G & 0x0ff) << 8) | (color.B & 0x0ff);
    }
}
