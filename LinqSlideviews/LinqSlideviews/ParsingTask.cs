using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
            .Skip(1) // Пропускаем заголовок
            .Select(line => line.Split(';'))
            .Where(parts => parts.Length == 3 &&
                           int.TryParse(parts[0], out _) &&
                           Enum.TryParse<SlideType>(parts[1], true, out _))
            .ToDictionary(
                parts => int.Parse(parts[0]),
                parts => new SlideRecord(int.Parse(parts[0]), Enum.Parse<SlideType>(parts[1], true), parts[2]));
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1) // Пропускаем заголовок
            .Select(line =>
            {
                var parts = line.Split(';');
                if (parts.Length != 4 ||
                    !int.TryParse(parts[0], out var userId) ||
                    !int.TryParse(parts[1], out var slideId) ||
                    !slides.ContainsKey(slideId) ||
                    !DateTime.TryParseExact($"{parts[2]} {parts[3]}", "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                    throw new FormatException($"Wrong line [{line}]");

                return new VisitRecord(userId, slideId, dateTime, slides[slideId].SlideType);
            });
    }
}