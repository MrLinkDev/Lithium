/// This file is part of Lithium.
/// Copyright (c) 2024 Alexander Gorbunov <sasha2000.gorbunov@gmail.com>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Lithium.Enums;

namespace Lithium;

/// <summary>
/// Класс логгера
/// </summary>
public static class Log {
    #region Paths
    
    /// Путь до папки с данными приложения
    private static readonly string appDataPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        AppDomain.CurrentDomain.FriendlyName);
    
    /// Путь до папки для логов
    private static readonly string logsDirPath = Path.Join(appDataPath, Defaults.FOLDER_NAME);

    #endregion

    #region LogFields

    /// <summary>
    /// Выбранный уровень логирования
    /// </summary>
    private static Level level;
    
    /// <summary>
    /// Коллекция сообщений, которые ждут отправки
    /// </summary>
    private static readonly Queue<string> messageQueue = new ();

    /// <summary>
    /// Сообщение, которое логируется
    /// </summary>
    private static string? selectedMessage;

    #endregion

    #region FileRegion

    /// <summary>
    /// Порядковый номер файла, который изменяется в пределах одной сессии
    /// </summary>
    private static int logFileNumber = 0;

    /// <summary>
    /// Объект, предоставляющий информацию о файле
    /// </summary>
    private static FileInfo fileInfo;
    
    /// <summary>
    /// Объект потока для записи в файл
    /// </summary>
    private static StreamWriter fileWriter;

    #endregion

    #region Threading

    /// <summary>
    /// Поток для логгера
    /// </summary>
    private static Thread thread;
    
    /// <summary>
    /// Источник токена отмены
    /// </summary>
    private static readonly CancellationTokenSource tokenSource = new();
    
    /// <summary>
    /// Токен отмены
    /// </summary>
    private static CancellationToken token;
    
    /// <summary>
    /// Мьютекс для ограничения доступа к очереди сообщений
    /// </summary>
    private static readonly object queueMutex = new();

    /// <summary>
    /// Объект события для организации ожидания заполнения очереди сообщений
    /// </summary>
    private static readonly ManualResetEvent resetEvent = new(false);

    #endregion
    
    /// <summary>
    /// Инициализация логгера
    /// </summary>
    /// <param name="logLevel">
    /// Уровень логирования. Сообщения, которые имеют уровень ниже установленного, игнорируются
    /// </param>
    public static void Start(Level logLevel) {
        ConsoleUtils.Mode.EnableColor();
        
        level = logLevel;
        
        token = tokenSource.Token;
        
        PrepareFile();

        thread = new Thread(Loop);
        thread.Start();
    }

    /// <summary>
    /// Остановка логгера
    /// </summary>
    public static void Stop() { 
        tokenSource.Cancel();
        resetEvent.Set();
        
        thread.Join();
        
        fileWriter.Flush();
        fileWriter.Close();
    }

    #region PreDefinedLogMethods

    /// <summary>
    /// Отправка сообщения с уровнем Error
    /// </summary>
    /// <param name="message">Отправляемое сообщение</param>
    public static void E(string message) {
        Write(Level.ERROR, message);
    }
    
    /// <summary>
    /// Отправка сообщения с уровнем Warning
    /// </summary>
    /// <param name="message">Отправляемое сообщение</param>
    public static void W(string message) {
        Write(Level.WARNING, message);
    }
    
    /// <summary>
    /// Отправка сообщения с уровнем Info
    /// </summary>
    /// <param name="message">Отправляемое сообщение</param>
    public static void I(string message) {
        Write(Level.INFO, message);
    }
    
    /// <summary>
    /// Отправка сообщения с уровнем Debug
    /// </summary>
    /// <param name="message">Отправляемое сообщение</param>
    public static void D(string message) {
        Write(Level.DEBUG, message);
    }
    
    /// <summary>
    /// Отправка сообщения с уровнем Trace
    /// </summary>
    /// <param name="message">Отправляемое сообщение</param>
    public static void T(string message) {
        Write(Level.TRACE, message);
    }

    #endregion

    #region WriteRegion

    /// <summary>
    /// Логирование сообщения с установленным уровнем логирования
    /// </summary>
    /// <param name="logLevel">Уровень логирования</param>
    /// <param name="message">Сообщение</param>
    private static void Write(Level logLevel, string message) {
        if (logLevel < level) {
            return;
        }
        
        string time = GetCurrentTime(TimeFormat.CONSOLE);
        string tag = GetTag(logLevel);

        string formattedMessage = $"{time} [{tag}] {message}";
        
        lock (queueMutex) {
            messageQueue.Enqueue(formattedMessage);
            resetEvent.Set();
        } 
    }
    
    private static void Loop() {
        while (!token.IsCancellationRequested) {
            resetEvent.WaitOne();

            while (messageQueue.TryDequeue(out selectedMessage)) {
                lock (queueMutex) {
                    WriteIntoConsole(selectedMessage);
                    WriteIntoFile(selectedMessage);
                }
            }

            if (token.IsCancellationRequested) {
                return;
            }

            resetEvent.Reset();
        }
    }
    
    /// <summary>
    /// Отправка сообщения в консоль
    /// </summary>
    /// <param name="message">Отправляемое сообщение</param>
    private static void WriteIntoConsole(string message) {
        Console.Out.WriteLine(message);
    }

    /// <summary>
    /// Запись сообщения в файл
    /// </summary>
    /// <param name="message">Записываемое сообщение</param>
    private static void WriteIntoFile(string message) {
        try {
            fileInfo.Refresh();
            if (fileInfo.Length >= Defaults.FILE_MAX_SIZE) {
                fileWriter.Close();
                PrepareFile();
            }
            
            fileWriter.WriteLine(message);
            fileWriter.Flush();
        } catch (Exception) {
            PrepareFile();
        }
    }

    #endregion

    #region Utilities
    
    /// <summary>
    /// Инициализация части логгера, которая работает с файлом
    /// </summary>
    private static void PrepareFile() {
        if (!Directory.Exists(logsDirPath)) {
            Directory.CreateDirectory(logsDirPath);
        }
        
        CheckFileCountInDirectory();

        var logFileName = Path.Join(
            logsDirPath, 
            String.Concat(GetCurrentTime(TimeFormat.FILE), $"_{logFileNumber++,0:D3}", Defaults.FILE_EXTENSION));

        fileWriter = new StreamWriter(logFileName);
        fileInfo = new FileInfo(logFileName);
    }

    /// <summary>
    /// Проверка количества файлов в папке с логами. Если больше, чем максимально
    /// допустимое, то удаляет старые файлы до тех пор, пока количество файлов
    /// не будет равно максимальное-1
    /// </summary>
    private static void CheckFileCountInDirectory() {
        string[] filenameList = Directory.GetFiles(logsDirPath, "*", SearchOption.TopDirectoryOnly);
        if (filenameList.Length > Defaults.FILE_MAX_COUNT - 1) {
            filenameList = filenameList.OrderBy(filename => filename).ToArray();

            int pos = 0;
            
            while (filenameList.Length - pos > Defaults.FILE_MAX_COUNT - 1) {
                File.Delete(Path.Combine(logsDirPath, filenameList[pos]));
                ++pos;
            }
        }
    }
    
    /// <summary>
    /// Получение текущего времени в требуемом формате
    /// </summary>
    /// <param name="format">Тип требуемого формата</param>
    /// <returns>Строка в требуемом формате</returns>
    private static string GetCurrentTime(TimeFormat format) {
        DateTime now = DateTime.Now;
        
        return format switch {
            TimeFormat.CONSOLE => now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            TimeFormat.FILE => now.ToString("yyyyMMdd_HHmmss"),
            
            _ => now.ToString("yyyy_MM_dd_HH_mm_ss")
        };
    }
    
    /// <summary>
    /// Получение тега уровня сообщения
    /// </summary>
    /// <param name="logLevel">Уровень сообщения</param>
    /// <returns>Тег уровня сообщения</returns>
    private static string GetTag(Level logLevel) {
        return logLevel switch {
            Level.ERROR => "ERR",
            Level.WARNING => "WRN",
            Level.INFO => "INF",
            Level.DEBUG => "DBG",
            Level.TRACE => "TRC",
            
            _ => "???"
        };
    }
    
    #endregion
}
