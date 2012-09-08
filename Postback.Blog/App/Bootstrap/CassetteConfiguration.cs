using Cassette;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Postback.Blog
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection configurable)
        {
            configurable.Add<StylesheetBundle>("css");
            configurable.Add<ScriptBundle>("js");
        }
    }
}