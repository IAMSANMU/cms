using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Imow.Framework.Engine;
using Imow.Framework.UI;

namespace WebImow.Common.Framework.Strategy.UI
{
    public partial class PageHeadBuilder : IPageHeadBuilder
    {
        #region Fields

        private static readonly object s_lock = new object();

        private readonly List<string> _titleParts;
        private readonly Dictionary<ResourceLocation, List<ScriptReferenceMeta>> _scriptParts;
        private readonly Dictionary<ResourceLocation, List<string>> _cssParts;
        private readonly List<string> _canonicalUrlParts;
        private readonly List<string> _headCustomParts;

        #endregion 

        #region Ctor

        public PageHeadBuilder()
        {
            this._titleParts = new List<string>();
            this._scriptParts = new Dictionary<ResourceLocation, List<ScriptReferenceMeta>>();
            this._cssParts = new Dictionary<ResourceLocation, List<string>>();
            this._canonicalUrlParts = new List<string>();
            this._headCustomParts = new List<string>();
        }

        #endregion

        #region Utilities

      


        protected virtual MetaModel Uris2Meta(string[] uris)
        {
            var meta = new MetaModel();
            foreach (string item in uris)
            {
                var parts = item.Replace("://", "__").Split('/');
                var m = meta;
                foreach (var part in parts)
                {
                    if (!m.Keys.Contains(part))
                    {
                        var itemM = new MetaModel();
                        m.InnerModel.Add(part, itemM);
                        m.Keys.Add(part);
                    }
                    m = m.InnerModel[part];
                }
            }
            return meta;
        }


        protected virtual Dictionary<string, List<string>> Meta2Path(MetaModel meta)
        {
            var paths = new Dictionary<string, List<string>>();
            var __KEYS = meta.Keys;

            foreach (var part in __KEYS)
            {
                var root = part;
                var m = meta.InnerModel[part];
                var KEYS = m.Keys;
                while (KEYS.Count == 1)
                {
                    root += "/" + KEYS[0];
                    m = m.InnerModel[KEYS[0]];
                    KEYS = m.Keys;
                }
                if (KEYS.Count > 0)
                {
                    paths.Add(root.Replace("__", "://"), Meta2Arr(m));
                }
            }
            return paths;
        }


        protected virtual List<string> Meta2Arr(MetaModel meta)
        {
            var arr = new List<string>();
            var __KEYS = meta.Keys;

            foreach (var key in __KEYS)
            {
                var r = Meta2Arr(meta.InnerModel[key]);
                var m = r.Count;
                if (m > 0)
                {
                    for (var j = 0; j < m; j++)
                    {
                        arr.Add(key + "/" + r[j]);
                    }
                }
                else
                {
                    arr.Add(key);
                }
            }
            return arr;
        }


        public virtual string PartsToBundleToUrl(string[] partsToBundle)
        {
            var dic = Meta2Path(Uris2Meta(partsToBundle));
            string url = "";

            foreach (KeyValuePair<string, List<string>> keyValuePair in dic)
            {
                url = string.Concat(keyValuePair.Key,"/$$$",string.Join(",", keyValuePair.Value))  ;
            }
            return url;
        }
        #endregion

        #region Methods

        public virtual void AddTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Add(part);
        }
        public virtual void AppendTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Insert(0, part);
        }
        public virtual string GenerateTitle(bool addDefaultTitle)
        {
            var specificTitle = string.Join(",", _titleParts.AsEnumerable().Reverse().ToArray());
            return specificTitle;
        }

      
        public virtual void AddScriptParts(ResourceLocation location, string part, bool excludeFromBundle)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;

            _scriptParts[location].Add(new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Part = part
            });
        }
        public virtual void AppendScriptParts(ResourceLocation location, string part, bool excludeFromBundle)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;

            _scriptParts[location].Insert(0, new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Part = part
            });
        }
        public virtual string GenerateScripts(UrlHelper urlHelper, ResourceLocation location, bool bundleFiles)
        {
            if (!_scriptParts.ContainsKey(location) || _scriptParts[location] == null)
                return "";

            if (_scriptParts.Count == 0)
                return "";

            if (bundleFiles)
            {
                var partsToBundle = _scriptParts[location]
                    .Where(x => !x.ExcludeFromBundle)
                    .Select(x => x.Part)
                    .Distinct()
                    .ToArray();
                var partsToDontBundle = _scriptParts[location]
                    .Where(x => x.ExcludeFromBundle)
                    .Select(x => x.Part)
                    .Distinct()
                    .ToArray();


                var result = new StringBuilder();

                if (partsToBundle.Length > 0)
                {
                    string url = PartsToBundleToUrl(partsToBundle);
                    result.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", urlHelper.Content(url));
                    result.Append(Environment.NewLine);
                }

                foreach (var path in partsToDontBundle)
                {
                    result.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", urlHelper.Content(path));
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
            else
            {
                var result = new StringBuilder();
                foreach (var path in _scriptParts[location].Select(x => x.Part).Distinct())
                {
                    var url = path;
                  
                    result.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", urlHelper.Content(url));
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
        }


        public virtual void AddCssFileParts(ResourceLocation location, string part)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(part))
                return;

            _cssParts[location].Add(part);
        }
        public virtual void AppendCssFileParts(ResourceLocation location, string part)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<string>());

            if (string.IsNullOrEmpty(part))
                return;

            _cssParts[location].Insert(0, part);
        }
        public virtual string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location, bool bundleFiles)
        {
            if (!_cssParts.ContainsKey(location) || _cssParts[location] == null)
                return "";

            var distinctParts = _cssParts[location].Distinct().ToList();
            if (distinctParts.Count == 0)
                return "";

            //合并文件
            if (bundleFiles)
            {
                var result = new StringBuilder();

                var partsToBundle = distinctParts.ToArray();
                if (partsToBundle.Length > 0)
                {
                    string url = PartsToBundleToUrl(distinctParts.ToArray());
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", url);
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
            else
            {
                var result = new StringBuilder();
                foreach (var path in distinctParts)
                {
                    var url = path;
                 
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", urlHelper.Content(url));
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
        }


        public virtual void AddCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _canonicalUrlParts.Add(part);
        }
        public virtual void AppendCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _canonicalUrlParts.Insert(0, part);
        }
        public virtual string GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        public virtual void AddHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Add(part);
        }
        public virtual void AppendHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Insert(0, part);
        }
        public virtual string GenerateHeadCustom()
        {
            //use only distinct rows
            var distinctParts = _headCustomParts.Distinct().ToList();
            if (distinctParts.Count == 0)
                return "";

            var result = new StringBuilder();
            foreach (var path in distinctParts)
            {
                result.Append(path);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

     
        #endregion

        #region Nested classes

        private class ScriptReferenceMeta
        {
            public bool ExcludeFromBundle { get; set; }

            public string Part { get; set; }
        }

        #endregion
    }
}
