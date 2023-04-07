using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        SlideType slideType = default;
        return lines
            .Where(x => x.Split(';').Length == 3)
            .Select(x => x.Split(';'))
            .Where(x => int.TryParse(x[0], out _) && Enum.TryParse(x[1], true, out slideType))
            .Select(x => x)
            .ToDictionary(x => int.Parse(x[0]),
                x => new SlideRecord(int.Parse(x[0]),  slideType, x[2]));
    }
	
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1)
            .Select(line => TryParseLine(slides, line));
    }

    private static VisitRecord TryParseLine(IDictionary<int, SlideRecord> slides, string line)
    {
        try
        {
            var data = line.Split(';');
            return new VisitRecord(
                int.Parse(data[0]),
                int.Parse(data[1]),
                DateTime.ParseExact(
                    $"{data[2]} {data[3]}",
                    "yyyy-MM-dd HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None), 
                slides[int.Parse(data[1])].SlideType);
        }
        catch (Exception e)
        {
            throw new FormatException($"Wrong line [{line}]", e);
        }
    }
}