using System;
using Postback.Blog;


public static class DateTimeExtensions
{
    public static string FormatToSmartTimeSpan(this DateTime dateTime)
    {
        var now = SystemTime.Now();
        TimeSpan timestamp;
        if (dateTime.Date == now.Date)
        {
            return "today, " + dateTime.FormatToTime();
        }
        else if(dateTime < now)
        {
            timestamp = now.Date.Subtract(dateTime.Date);
            if(timestamp.Days == 1)
            {
                return "yesterday, " + dateTime.FormatToTime();
            }
            if(timestamp.Days > 1)
            {
                return dateTime.FormatToDateAndTime();
            }
        }
        else
        {
            timestamp = dateTime.Date.Subtract(now.Date);
            if (timestamp.Days == 1)
            {
                return "tomorrow, " + dateTime.FormatToTime();
            }
            if (timestamp.Days < -1)
            {
                return dateTime.FormatToDateAndTime();
            }
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
        return date.ToString("dd MMM yyyy");
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