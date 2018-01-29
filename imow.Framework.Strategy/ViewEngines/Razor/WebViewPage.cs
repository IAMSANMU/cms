using System.IO;
using System.Web.Mvc;
using imow.Core.config;
using imow.Framework.Interface;
using Imow.Framework.Engine;

namespace imow.Framework.Strategy.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public IWorkContext WorkContext
        {
            get { return ImowEngineContext.Current.Resolve<IWorkContext>(); }
        }
     

        public override string Layout
        {
            get
            {
                var layout = base.Layout;

                if (!string.IsNullOrEmpty(layout))
                {
                    var filename = Path.GetFileNameWithoutExtension(layout);
                    ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

                    if (viewResult.View != null && viewResult.View is RazorView)
                    {
                        layout = (viewResult.View as RazorView).ViewPath;
                    }
                }

                return layout;
            }
            set
            {
                base.Layout = value;
            }
        }
        
        private bool _isRelease;
        public bool IsRelease
        {
            get
            {
                _isRelease = true;
#if DEBUG
                _isRelease=false;
#endif
                return _isRelease;
            }
        }

        
        public string Image(string imageUrl)
        {
            var uploadImageUrl = ImowEngineContext.Current.ResolveConfig<ImowConfig>().ImageUrl;
            return string.Concat(uploadImageUrl, imageUrl);
        }

      

    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}