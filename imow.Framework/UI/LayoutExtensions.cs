using System.Web.Mvc;
using Imow.Framework.Engine;
using Imow.Framework.UI;

namespace WebImow.Common.Framework.Strategy.UI
{
    public static class LayoutExtensions
    {

        /// <summary>
        /// 添加一个title信息
        /// </summary>
        public static void AddTitleParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder  = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddTitleParts(part);
        }
        /// <summary>
        /// 添加一个title信息
        /// </summary>
        public static void AppendTitleParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendTitleParts(part);
        }
        /// <summary>
        /// 输出全部的title
        /// </summary>
        public static MvcHtmlString FlushTitle(this HtmlHelper html, bool addDefaultTitle, string part = "")
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            html.AppendTitleParts(part);
            return MvcHtmlString.Create(html.Encode(pageHeadBuilder.GenerateTitle(addDefaultTitle)));
        }


      


        /// <summary>
        /// 添加一个script
        /// </summary>
        public static void AddScriptParts(this HtmlHelper html, string part, bool excludeFromBundle = false)
        {
            AddScriptParts(html, ResourceLocation.Head, part, excludeFromBundle);
        }
        /// <summary>
        ///添加一个script
        /// </summary>
        public static void AddScriptParts(this HtmlHelper html, ResourceLocation location, string part, bool excludeFromBundle = false)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddScriptParts(location, part, excludeFromBundle);
        }
        /// <summary>
        /// 添加一个script
        /// </summary>
        public static void AppendScriptParts(this HtmlHelper html, string part, bool excludeFromBundle = false)
        {
            AppendScriptParts(html, ResourceLocation.Head, part, excludeFromBundle);
        }
        /// <summary>
        ///  添加一个script
        /// </summary>
        public static void AppendScriptParts(this HtmlHelper html, ResourceLocation location, string part, bool excludeFromBundle = false)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendScriptParts(location, part, excludeFromBundle);
        }
        /// <summary>
        /// 输出全部的script
        /// </summary>
        public static MvcHtmlString FlushScripts(this HtmlHelper html, UrlHelper urlHelper, 
            ResourceLocation location, bool bundleFiles = false)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GenerateScripts(urlHelper, location, bundleFiles));
        }



        /// <summary>
        /// 添加一个css
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">CSS part</param>
        public static void AddCssFileParts(this HtmlHelper html, string part)
        {
            AddCssFileParts(html, ResourceLocation.Head, part);
        }
        /// <summary>
        /// 添加一个css
        /// </summary>
        public static void AddCssFileParts(this HtmlHelper html, ResourceLocation location, string part)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddCssFileParts(location, part);
        }
        /// <summary>
        /// 添加一个css
        /// </summary>
        public static void AppendCssFileParts(this HtmlHelper html, string part)
        {
            AppendCssFileParts(html, ResourceLocation.Head, part);
        }
        /// <summary>
        /// 添加一个css
        /// </summary>
        public static void AppendCssFileParts(this HtmlHelper html, ResourceLocation location, string part)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendCssFileParts(location, part);
        }
        /// <summary>
        /// 输出全部css
        /// </summary>
        public static MvcHtmlString FlushCssFiles(this HtmlHelper html, UrlHelper urlHelper,
            ResourceLocation location, bool bundleFiles = false)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            return MvcHtmlString.Create(pageHeadBuilder.GenerateCssFiles(urlHelper, location, bundleFiles));
        }


        /// <summary>
        /// 添加CanonicalUrl
        /// </summary>
        public static void AddCanonicalUrlParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AddCanonicalUrlParts(part);
        }
        /// <summary>
        ///添加CanonicalUrl
        /// </summary>
        public static void AppendCanonicalUrlParts(this HtmlHelper html, string part)
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            pageHeadBuilder.AppendCanonicalUrlParts(part);
        }
        /// <summary>
        /// 输出CanonicalUrl
        /// </summary>
        public static MvcHtmlString FlushCanonicalUrls(this HtmlHelper html, string part = "")
        {
            var pageHeadBuilder = ImowEngineContext.Current.Resolve<IPageHeadBuilder>();
            html.AppendCanonicalUrlParts(part);
            return MvcHtmlString.Create(pageHeadBuilder.GenerateCanonicalUrls());
        }

   
    }
}
