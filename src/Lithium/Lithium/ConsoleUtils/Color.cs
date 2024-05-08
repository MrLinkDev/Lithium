namespace Lithium.ConsoleUtils;

/// <summary>
/// Структура, которая описывает цвет в формате RGB
/// </summary>
public struct Color {
    /// Красная составляющая цвета
    public byte R;
    /// Зелёная составляющая цвета
    public byte G;
    /// Синяя составляющая цвета
    public byte B;

    public Color(byte r, byte g, byte b) {
        R = r;
        G = g;
        B = b;
    }
}
