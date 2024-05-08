/// This file is part of Lithium.
/// Copyright (c) 2024 Alexander Gorbunov <sasha2000.gorbunov@gmail.com>

using System.Runtime.InteropServices;

namespace Lithium.ConsoleUtils;

/// <summary>
/// Класс, в котором описан режим работы консоли
/// </summary>
public static class Mode {

    #region DllImports
    
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(int nStdHandle);
    
    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
    
    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    
    #endregion

    #region Constants

    /// <summary>
    /// Название переменной окружения (https://no-color.org/)
    /// </summary>
    private const string NO_COLOR = "NO_COLOR";
    
    private const int STD_OUTPUT_HANDLE = -11;

    private const uint ENABLE_PROCESSED_OUTPUT = 0x0001;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    #endregion

    #region Variables

    /// Флаг, который показывает, что в переменных системы найдена переменная NO_COLOR
    private static bool isNoColor;
    /// Флаг, который показывает, что текст может быть цветным
    private static bool isColored;

    #endregion

    /// <summary>
    /// Проверяет, существует ли в системе переменная NO_COLOR и какое у неё значение
    /// </summary>
    /// <returns>Если переменная не существует, возвращает true. Если существует - возвращает её значение</returns>
    private static bool IsNoColorExists() {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return true;
        }
        
        string? value = null;
        bool noColor = false;

        // Проверка переменных окружения в Process (0), User (1) и Machine (2)
        for (byte target = 0; target < 3; ++target) {
            value = Environment.GetEnvironmentVariable(NO_COLOR, (EnvironmentVariableTarget)target);
            if (value == null) {
                noColor = false;
                continue;
            }

            noColor &= bool.Parse(value);
        }
        
        return noColor;
    }

    /// <summary>
    /// Проверяет, можно ли изменить цвет в консоли
    /// </summary>
    /// <returns>Если цвет можно изменить, возвращает true</returns>
    public static bool IsColorEnabled() {
        return !isNoColor && isColored;
    }

    /// <summary>
    /// Включает возможность выводить цвет в консоли
    /// </summary>
    public static void EnableColor() {
        isNoColor = IsNoColorExists();

        if (isNoColor) {
            return;
        }

        IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(stdHandle, out var mode);

        mode |= ENABLE_PROCESSED_OUTPUT | ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        isColored = SetConsoleMode(stdHandle, mode);
    }

    /// <summary>
    /// Выключает возможность выводить цвет в консоли
    /// </summary>
    public static void DisableColor() {
        isColored = false;
    }
}
