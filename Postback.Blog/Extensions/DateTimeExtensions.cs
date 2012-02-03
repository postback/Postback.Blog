using System;


public static class DateTimeExtensions
{
    public static string FormatToSmartTimeSpan(this DateTime dateTime)
    {
        var timestamp = DateTime.Now.Subtract(dateTime);
        if(timestamp.Days == 1)
        {
            return "yesterday, " + dateTime.FormatToTime();
        }
        else if(timestamp.Days > 1)
        {
            return dateTime.FormatToDateAndTime();
        }

        return timestamp.ToFormattedString();
    }

    public static string FormatToSmartTimeSpan(this DateTime? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.FormatToSmartTimeSpan() : string.Empty;
    }

    public static string FormatToDateAndTime(this DateTime dateTime)
    {
        return string.Format("{0} {1}", dateTime.FormatToDate(), dateTime.FormatToTime());
    }

    public static string FormatToDateAndTime(this DateTime? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.FormatToDateAndTime() : string.Empty;
    }

    public static string FormatToDate(this DateTime date)
    {
        return date.ToString("dd/MM/yyyy");
    }

    public static string FormatToTime(this DateTime date)
    {
        return date.ToString("HH:mm");
    }

    public static string Date(DateTime? date)
    {
        return date == null
                ? string.Empty
                : date.Value.FormatToDate();
    }
}