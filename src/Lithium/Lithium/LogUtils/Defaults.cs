/// This file is part of Lithium.
/// Copyright (c) 2024 Alexander Gorbunov <sasha2000.gorbunov@gmail.com>

namespace Lithium.LogUtils;

/// <summary>
/// Класс, в котором находятся значения по умолчанию
/// </summary>
public static class Defaults {
    /// Название папки, где находятся логи
    public const string FOLDER_NAME = "Logs";
    /// Расшираение файла лога
    public const string FILE_EXTENSION = ".log";
    /// Максимальное количество файлов логов, которое может находится в папке логов
    public const int FILE_MAX_COUNT = 10;
    /// Максимальный размер файла лога
    public const long FILE_MAX_SIZE = 1 * 1024 * 1024;

    public const string MESSAGE_MASK = "{0} {1} {2}";

    public const byte ERROR_COLOR = 196;
    public const byte WARNING_COLOR = 214;
    public const byte INFO_COLOR = 78;
    public const byte DEBUG_COLOR = 153;
    public const byte TRACE_COLOR = 250;
}
