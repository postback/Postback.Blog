using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

public static class StringExtensions
{
    public static string Localize(this string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return key;
        }
        return key;// ObjectFactory.GetInstance<ISiteContext>().GetCurrentSite().Localize(key);
    }

    public static string Localize(this MvcHtmlString key)
    {
        if (key == null)
        {
            return string.Empty;
        }
        return key.ToHtmlString().Localize();
    }

    public static string ToUri(this string text)
    {
        byte[] b = Encoding.GetEncoding(1251).GetBytes(text); // 8 bit characters
        string slug = Encoding.ASCII.GetString(b);
        var whitespace = new Regex("\\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        slug = whitespace.Replace(slug, "-");

        var nonword = new Regex("[^a-zA-Z0-9_-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        slug = nonword.Replace(slug, "");

        var underscore = new Regex("_+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        slug = underscore.Replace(slug, "-");
        slug = slug.ToLower();

        var dash = new Regex("-+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        slug = dash.Replace(slug, "-");

        var charsToTrim = new [] {'-'};
        slug = slug.TrimEnd(charsToTrim).TrimStart(charsToTrim);

        return slug;
    }

    public static string ToSeparatedWords(this string value)
    {
        return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
    }
}