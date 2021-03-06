﻿using System.Web.Mvc;
using Imow.Framework.UI;

namespace WebImow.Common.Framework.Strategy.UI
{
    public partial interface IPageHeadBuilder
    {
        void AddTitleParts(string part);
        void AppendTitleParts(string part);
        string GenerateTitle(bool addDefaultTitle);

   

        void AddScriptParts(ResourceLocation location, string part, bool excludeFromBundle);
        void AppendScriptParts(ResourceLocation location, string part, bool excludeFromBundle);
        string GenerateScripts(UrlHelper urlHelper, ResourceLocation location, bool bundleFiles);

        void AddCssFileParts(ResourceLocation location, string part);
        void AppendCssFileParts(ResourceLocation location, string part);
        string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location, bool bundleFiles);


        void AddCanonicalUrlParts(string part);
        void AppendCanonicalUrlParts(string part);
        string GenerateCanonicalUrls();

        void AddHeadCustomParts(string part);
        void AppendHeadCustomParts(string part);
        string GenerateHeadCustom();
    }
}
