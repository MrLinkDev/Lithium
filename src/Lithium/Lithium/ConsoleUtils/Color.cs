/// This file is part of Lithium.
/// Copyright (c) 2024 Alexander Gorbunov <sasha2000.gorbunov@gmail.com>

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
