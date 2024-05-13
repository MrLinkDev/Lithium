namespace Lithium.LogUtils;

/// <summary>
/// Структура, в которой содержатся строки сообщений для консоли и файла
/// </summary>
public struct LogMessage {
    /// Сообщение для консоли
    public string? Console;
    /// Сообщение для файла
    public string? File;

    public LogMessage(string? console, string? file) {
        Console = console;
        File = file;
    }
}
