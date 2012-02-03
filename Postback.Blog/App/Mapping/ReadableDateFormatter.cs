using System;
using AutoMapper;

namespace Postback.Blog.App.Mapping
{
    public class ReadableDateFormatter : IValueFormatter
    {
        public string FormatValue(ResolutionContext context)
        {
            if (context.SourceValue == null)
                return null;

            if (!(context.SourceValue is DateTime))
                return context.SourceValue.ToNullSafeString();

            return ((DateTime)context.SourceValue).ToLocalTime().FormatToSmartTimeSpan();
        }
    }
}