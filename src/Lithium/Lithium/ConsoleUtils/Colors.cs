/// This file is part of Lithium.
/// Copyright (c) 2024 Alexander Gorbunov <sasha2000.gorbunov@gmail.com>

namespace Lithium.ConsoleUtils;

/// <summary>
/// Класс, в котором описаны цвета по умолчанию
/// </summary>
public static class Colors {

    #region DefaultRgbColors
    
    public static readonly Color Black = new (12, 12, 12);
    public static readonly Color Red = new (197, 15, 32);
    public static readonly Color Green = new (19, 161, 14);
    public static readonly Color Yellow = new (193, 156, 0);
    public static readonly Color Blue = new (0, 55, 218);
    public static readonly Color Magenta = new (136, 23, 152);
    public static readonly Color Cyan = new (58, 150, 221);
    public static readonly Color White = new (204, 204, 204);

    #endregion
    
    #region BrightRgbColors
    
    public static readonly Color BrightBlack = new (118, 118, 118);
    public static readonly Color BrightRed = new (231, 72, 86);
    public static readonly Color BrightGreen = new (22, 198, 12);
    public static readonly Color BrightYellow = new (249, 241, 165);
    public static readonly Color BrightBlue = new (59, 120, 255);
    public static readonly Color BrightMagenta = new (180, 0, 158);
    public static readonly Color BrightCyan = new (97, 214, 214);
    public static readonly Color BrightWhite = new (242, 242, 242);

    #endregion
}
