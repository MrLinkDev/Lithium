namespace Lithium;

/// <summary>
/// Класс, в котором находятся значения по умолчанию
/// </summary>
public static class Defaults {
    /// Название папки, где находятся логи
    public const string FOLDER_NAME = "Logs";
    /// Расшираение файла лога
    public const string FILE_EXTENSION = ".log";
    /// Максимальное количество файлов логов, которое может находится в папке логов
    public const int FILE_MAX_COUNT = 20;
    /// Максимальный размер файла лога
    public const long FILE_MAX_SIZE = 1 * 1024 * 1024;
}
