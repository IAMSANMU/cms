//using System.Collections.Generic;
//using System.Configuration;
//using System.Web;
//using System.Web.Mvc;
//using EC.Common.Extensions;
//using imow.AdminEtl.Models;
//using IModelBinder = System.Web.Mvc.IModelBinder;
//using ModelBindingContext = System.Web.Mvc.ModelBindingContext;

//namespace imow.AdminEtl.Binder
//{
//    public class ListSearchModelModelBinder : IModelBinder
//    {
//        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            HttpRequestBase httpRequestBase = controllerContext.HttpContext.Request;
//            ListSearchModel model = new ListSearchModel();
//            model.ParameterDictionary = new Dictionary<string, string>();
//            foreach (string key in httpRequestBase.Form)
//            {
//                string value = httpRequestBase.Form[key];
//                switch (key)
//                {
//                    case "draw":
//                        model.Draw = value.ToInt();
//                        break;
//                    case "start":
//                        model.PageIndex = value.ToInt();
//                        break;
//                    case "length":
//                        model.PageSize = value.ToInt();
//                        break;
//                    case "order":
//                        model.Order = value;
//                        break;
//                    case "ordertype":
//                        model.OrderType = value;
//                        break;
//                    default:
//                        model.ParameterDictionary.Add(key,value);
//                        break;
//                }
//            }
//            return model;
//        }
//    }
//}