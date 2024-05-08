/// This file is part of Lithium.
/// Copyright (c) 2024 Alexander Gorbunov <sasha2000.gorbunov@gmail.com>

using System.Text;

namespace Lithium.ConsoleUtils;

/// <summary>
/// Класс, в котором реализованы методы для изменения цвета текста/фона в консоли
/// </summary>
public static class ColorTextBuilder {
    /// Сброс 
    private const string RESET = "\x1b[0m";
    
    /// Маска для цвета текста в формате RGB
    private const string FG_COLOR_RGB_MASK = "\x1b[38;2;{0};{1};{2}m";
    /// Маска для цвета фона в формате RGB
    private const string BG_COLOR_RGB_MASK = "\x1b[48;2;{0};{1};{2}m";
    
    /// Маска для цвета текста в виде числового кода
    private const string FG_COLOR_NUM_MASK = "\x1b[38;5;{0}m";
    /// Маска для цвета фона в виде числового кода
    private const string BG_COLOR_NUM_MASK = "\x1b[48;5;{0}m";

    #region ColorTarget

    /// <summary>
    /// Изменение цвет текста/фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов/фона</param>
    /// <param name="color">Цвет в формате RGB</param>
    /// <param name="target">Объект, на который направлено изменение цвета</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, Color color, ColorTarget target = ColorTarget.FOREGROUND) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(
            target == ColorTarget.FOREGROUND ? FG_COLOR_RGB_MASK : BG_COLOR_RGB_MASK, 
            color.R, color.G, color.B);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }
    
    /// <summary>
    /// Изменение цвет текста/фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов/фона</param>
    /// <param name="r">Красная составляющая цвета</param>
    /// <param name="g">Зелёная составляющая цвета</param>
    /// <param name="b">Синяя составляющая цвета</param>
    /// <param name="target">Объект, на который направлено изменение цвета</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, byte r, byte g, byte b, ColorTarget target = ColorTarget.FOREGROUND) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(
            target == ColorTarget.FOREGROUND ? FG_COLOR_RGB_MASK : BG_COLOR_RGB_MASK, 
            r, g, b);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }

    /// <summary>
    /// Изменение цвет текста/фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов/фона</param>
    /// <param name="colorNum">Номер цвета <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code#8-bit"/></param>
    /// <param name="target">Объект, на который направлено изменение цвета</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, short colorNum, ColorTarget target = ColorTarget.FOREGROUND) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(
            target == ColorTarget.FOREGROUND ? FG_COLOR_NUM_MASK : BG_COLOR_NUM_MASK, 
            colorNum);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }

    #endregion

    #region ColorAll

    /// <summary>
    /// Изменяет цвет текста и фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов и фона</param>
    /// <param name="rF">Красная составляющая цвета символов</param>
    /// <param name="gF">Зелёная составляющая цвета символов</param>
    /// <param name="bF">Синяя составляющая цвета символов</param>
    /// <param name="rB">Красная составляющая цвета фона</param>
    /// <param name="gB">Зелёная составляющая цвета фона</param>
    /// <param name="bB">Синяя составляющая цвета фона</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, byte rF, byte gF, byte bF, byte rB, byte gB, byte bB) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(FG_COLOR_RGB_MASK, rF, gF, bF);
        builder.AppendFormat(BG_COLOR_RGB_MASK, rB, gB, bB);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }

    /// <summary>
    /// Изменяет цвет текста и фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов и фона</param>
    /// <param name="foreground">Цвет символов в формате RGB</param>
    /// <param name="background">Цвет фона в формате RGB</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, Color foreground, Color background) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(
            FG_COLOR_RGB_MASK, 
            foreground.R, foreground.G, foreground.B);
        builder.AppendFormat(
            BG_COLOR_RGB_MASK, 
            background.R, background.G, background.B);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }
    
    /// <summary>
    /// Изменяет цвет текста и фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов и фона</param>
    /// <param name="foreground">Цвет символов в формате RGB</param>
    /// <param name="bgNum">Цвет фона в виде числового кода</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, Color foreground, byte bgNum) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(
            FG_COLOR_RGB_MASK, 
            foreground.R, foreground.G, foreground.B);
        builder.AppendFormat(
            BG_COLOR_NUM_MASK, 
            bgNum);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }
    
    /// <summary>
    /// Изменяет цвет текста и фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов и фона</param>
    /// <param name="fgNum">Цвет символов в виде числового кода</param>
    /// <param name="background">Цвет фона в формате RGB</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, byte fgNum, Color background) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(
            FG_COLOR_NUM_MASK, 
            fgNum);
        builder.AppendFormat(
            BG_COLOR_RGB_MASK, 
            background.R, background.G, background.B);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }
    
    /// <summary>
    /// Изменяет цвет текста и фона
    /// </summary>
    /// <param name="text">Текст, у которого будет изменён цвет символов и фона</param>
    /// <param name="fgNum">Цвет символов в виде числового кода</param>
    /// <param name="bgNum">Цвет фона в виде числового кода</param>
    /// <returns>Строка с необходимым цветом</returns>
    public static string Color(this string text, byte fgNum, byte bgNum) {
        if (!Mode.IsColorEnabled()) {
            return text;
        }
        
        StringBuilder builder = new StringBuilder();
        
        builder.AppendFormat(
            FG_COLOR_NUM_MASK, 
            fgNum);
        builder.AppendFormat(
            BG_COLOR_NUM_MASK, 
            bgNum);
        
        builder.Append(text);
        builder.Append(RESET);

        return builder.ToString();
    }

    #endregion
    
    
    
}

