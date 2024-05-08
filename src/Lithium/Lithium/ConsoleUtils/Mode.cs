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

    private static bool isNoColor;
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

    public static bool IsColorEnabled() {
        return !isNoColor && isColored;
    }

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

    public static void DisableColor() {
        isColored = false;
    }
}
