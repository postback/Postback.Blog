using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cassette.Configuration;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Postback.Blog
{
    /// <summary>
    /// Configures the Cassette asset modules for the web application.
    /// </summary>
    public class CassetteConfiguration : ICassetteConfiguration
    {
        public void Configure(BundleCollection bundles, CassetteSettings settings)
        {
            bundles.Add<StylesheetBundle>("css");
            bundles.Add<ScriptBundle>("js");
        }
    }
}