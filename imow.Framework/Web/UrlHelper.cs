#region Summary

//===================================================================
//
// Author:    刘冰 jex536@163.com
//
// Project:	  EC.WebSite
//
// File:      UrlHelper.cs
//	
// Contents:
//			  url 帮助类
//
// Version:   1.0.0.0	
//
// Copyright (C) Aodaa Corporation. All rights reserved.
//
// ===================================================================
//
// History:	    2014/5/19 16:49:55 Created
// ===================================================================

#endregion

using System;
using System.Web;

using EC.Common.Extensions;

namespace EC.Web.Util
{
	/// <summary>
	/// url 相关类
	/// </summary>
	public static class UrlHelper
	{

		/// <summary>
		/// 获取相对Admin目录的url地址
		/// </summary>
		/// <param name="includeParameters">是否包含url参数</param>
		/// <param name="includePathInfo">是否包含pathinfo</param>
		/// <returns></returns>
		public static string GetAdminRelPath(bool includeParameters, bool includePathInfo = false)
		{
			var url = "";
			//获取页面地址
			var request = HttpContext.Current.Request;
			//去除第一级的Admin目录
			url = request.Url.AbsolutePath.ToLower().Replace(GetApplicationPath().Contact("admin/"), "");

			if (!includePathInfo)
			{
				if (!string.IsNullOrEmpty(request.PathInfo))
				{
					var index = url.LastIndexOf(request.PathInfo);
					url = url.Substring(0, index);
				}
			}
			if (includeParameters)
			{
				url += request.Url.PathAndQuery;
			}
			return url;
		}

		/// <summary>
		/// 得到http主机头(如：http://www.xxx.com)
		/// </summary>
		/// <returns></returns>
		public static string GetHostUrl()
		{
			var securePort = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
			var protocol = securePort == null || securePort == "0" ? "http" : "https";
			return string.Format("{0}://{1}", protocol, HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);
		}
		/// <summary>
		/// 得到站点的url(如：http://www.xxx.com/虚拟目录)
		/// </summary>
		public static string GetSiteUrl()
		{
			string path = HttpContext.Current.Request.ApplicationPath;
			if (path.EndsWith("/") && path.Length == 1)
			{
				return GetHostUrl();
			}
			else
			{
				return GetHostUrl() + path.ToLower();
			}
		}
		/// <summary>
		/// 得到站点的url(如：http://www.xxxx.com/虚拟目录/)
		/// 肯定以/结尾
		/// </summary>
		public static String GetSiteUrlEndWidthBackslash()
		{
			String siteUrl = UrlHelper.GetSiteUrl();
			if (!siteUrl.EndsWith("/"))
			{
				return String.Concat(siteUrl, "/");
			}
			else
			{
				return siteUrl;
			}
		}
		/// <summary>
		/// 得到虚拟应用程序根路径（如：/chuangxin/ 或者 /）
		/// </summary>
		public static String GetApplicationPath()
		{
			string path = HttpContext.Current.Request.ApplicationPath.ToLower();
			if (!path.EndsWith("/"))
			{
				return path + "/";
			}
			return path;
		}

		public static String GetCartUrl()
		{
			return String.Concat(GetApplicationPath(), "cart.aspx");
		}

		/// <summary>
		/// 注销url
		/// </summary>
		public static String GetLogoutUrl()
		{
			return String.Concat(GetApplicationPath(), "widgets/passport/logout.aspx");
		}

		/// <summary>
		/// 搜索
		/// </summary>
		public static String GetSearchUrl()
		{
			return String.Concat(GetApplicationPath(), "search.aspx");
		}

		/// <summary>
		/// 会员中心url
		/// </summary>
		public static String GetMemberUrl()
		{
            //return String.Concat(GetApplicationPath(), "member/index.aspx");
            return String.Concat(GetApplicationPath(), "member/baseinfo.aspx");
		}

        /// <summary>
        /// 会员中心url
        /// </summary>
        public static String GetUserOrderUrl()
        {
            return String.Concat(GetApplicationPath(), "member/orders.aspx");
        }
		/// <summary>
		/// 账户余额页面url
		/// </summary>
		public static String GetPredepositUrl(String condition)
		{
			return String.Format("{0}member/pre/{1}.aspx", GetApplicationPath(), condition);
		}

        /// <summary>
        /// 账户余额页面url
        /// </summary>
        public static String GetPredepositUrl()
        {
            return String.Format("{0}member/pre.aspx", GetApplicationPath());
        }


        /// <summary>
        /// 用户汇款单管理
        /// </summary>
        public static String GetPaymentLogUrl()
        {
            return String.Format("{0}member/paymentlog.aspx", GetApplicationPath());
        }

        /// <summary>
        /// 用户汇款单管理
        /// </summary>
        public static String GetPaymentLogUrl(String condition)
        {
            return String.Format("{0}member/paymentlog/{1}.aspx", GetApplicationPath(), condition);
        }

        /// <summary>
        /// 用户汇款单详细
        /// </summary>
        public static String GetPaymentLogDetailUrl(string condition)
        {
            return String.Format("{0}member/logdetail/{1}.aspx", GetApplicationPath(), condition);
        }

		/// <summary>
		/// 用户红包页面url
		/// </summary>
		public static String GetUserCouponUrl(String condition)
		{
			return String.Format("{0}member/coupon/{1}.aspx", GetApplicationPath(), condition);
		}
        /// <summary>
        /// 用户阿母币页面url
        /// </summary>
        public static String GetUserCouponImowUrl(String condition)
        {
            return String.Format("{0}member/couponimow/{1}.aspx", GetApplicationPath(), condition);
        }
        /// <summary>
        /// 用户积分页面url
        /// </summary>
        public static String GetUserScoreUrl(String condition)
        {
            return String.Format("{0}member/score/{1}.aspx", GetApplicationPath(), condition);
        }

        /// <summary>
        /// 用户积分页面url
        /// </summary>
        public static String GetUserScoreUrl()
        {
            return String.Format("{0}member/score.aspx", GetApplicationPath());
        }
		/// <summary>
		/// 用户投诉页面url
		/// </summary>
		public static String GetUserComplaintUrl(String condition)
		{
			return String.Format("{0}member/complainthistory/{1}.aspx", GetApplicationPath(), condition);
		}

        /// <summary>
        /// 用户退款列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static String GetUserServiceApplyUrl(String condition)
        {
            return String.Format("{0}member/serviceapply/{1}.aspx", GetApplicationPath(), condition);
        }

        public static String GetUserServiceApplyUrl()
        {
            return String.Format("{0}member/serviceapply.aspx", GetApplicationPath());
        }

        /// <summary>
        /// sku列表列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static String GetSkuSearchUrl(String condition)
        {
            return string.Format("{0}member/skusearch.aspx{1}", GetApplicationPath(), condition);
        }

        public static String GetSkuSearchUrl()
        {
            return String.Format("{0}member/skusearch.aspx", GetApplicationPath());
        }
		/// <summary>
		/// 注册页面url
		/// </summary>
		public static String GetRegisterUrl()
		{
			return String.Concat(GetApplicationPath(), "passport/register.html");
		}

		public static String GetForgotUrl()
		{
			return String.Concat(GetApplicationPath(), "findpwd/step1.aspx");
		}

		public static String GetForgotStep2Url()
		{
			return String.Concat(GetApplicationPath(), "findpwd/step2.aspx");
		}

		public static String GetForgotStep3Url()
		{
			return String.Concat(GetApplicationPath(), "findpwd/reset.aspx");
		}

		public static String GetForgotStep4Url()
		{
			return String.Concat(GetApplicationPath(), "findpwd/success.aspx");
		}

		public static String GetVerifyAccountUrl()
		{
			return String.Concat("findpwd/verify.aspx");
		}

		public static String GetPopEditOrderPriceUrl()
		{
			return String.Concat(GetApplicationPath(), "pop/editorderprice.aspx");
		}

		public static String GetPopEditOrderExpressUrl()
		{
			return String.Concat(GetApplicationPath(), "pop/editorderexpress.aspx");
		}

		public static String GetPopSelectProductUrl(string queryString)
		{
			return String.Concat(GetApplicationPath(), string.Format("pop/selectproduct.aspx{0}", queryString));
		}

		public static String GetPopSelectMenuUrl()
		{
			return String.Concat(GetApplicationPath(), "pop/selectmenu.aspx");
		}
        /// <summary>
        /// 用户信用还款跳转页面
        /// </summary>
        /// <returns></returns>
        public static String GetCreditDefaultUrl()
        {
            //暂时用
            return String.Format("{0}member/Repayment.aspx", GetApplicationPath());
        }

		/// <summary>
		/// 检测是否是前台页面
		/// </summary>
		public static Boolean IsRewriteUrl()
		{
			if (HttpContext.Current != null)
			{
				Object objRewriteUrl = HttpContext.Current.Items["IsRewriteUrl"];
				if (objRewriteUrl == null)
				{
					String appRelativeURL = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower(System.Globalization.CultureInfo.CurrentCulture);
					Boolean isRewriteUrl = !appRelativeURL.StartsWith("~/admin", StringComparison.OrdinalIgnoreCase)
									&& !appRelativeURL.StartsWith("~/plugins", StringComparison.OrdinalIgnoreCase)
									&& !appRelativeURL.StartsWith("~/widgets", StringComparison.OrdinalIgnoreCase)
									&& !appRelativeURL.StartsWith("~/pop", StringComparison.OrdinalIgnoreCase)
									&& !appRelativeURL.StartsWith("~/ajax", StringComparison.OrdinalIgnoreCase)
									&& !appRelativeURL.StartsWith("~/error.aspx", StringComparison.OrdinalIgnoreCase);
					HttpContext.Current.Items["IsRewriteUrl"] = isRewriteUrl;
					return isRewriteUrl;
				}
				else
				{
					return (Boolean)objRewriteUrl;
				}
			}
			return true;
		}
		/// <summary>
		/// 得到商品展示列表的url
		/// </summary>
        public static String GetProductListUrl(string classCode)
        {
            return String.Format("{0}list.html?c={1}", GetApplicationPath(), classCode);

        }

		/// <summary>
		/// 得到商品搜索展示列表的url
		/// </summary>
		public static String GetProductListUrl1(String condition)
		{
			return String.Format("{0}search.aspx{1}", GetApplicationPath(), condition);
		}
		/// <summary>
		/// 得到商品的url
		/// </summary>
		public static String GetProductUrl(String productId)
		{
			return String.Format("{0}product/{1}.html", GetApplicationPath(), productId);

		}
        
		/// <summary>
		/// 得到商品品牌的url
		/// </summary>
		public static String GetBrandUrl(String brandId)
		{
			return String.Format("{0}brand/{1}.aspx", GetApplicationPath(), brandId);
		}

		/// <summary>
		/// 得到订单明细url
		/// </summary>
		public static String GetOrderDetailUrl(String orderId)
		{
			return String.Format("{0}member/orderdetail/{1}.aspx", GetApplicationPath(), orderId);
		}

        /// <summary>
        /// 得到合伙人查看订单url
        /// </summary>
        public static String GetPartnerOrderDetailUrl(String orderId)
        {
            return String.Format("{0}member/partnerorderdetail/{1}.aspx", GetApplicationPath(), orderId);
        }

        /// <summary>
        /// 得到合伙人查看订单url
        /// </summary>
        public static String GetUserDetailUrl(String userId)
        {
            return String.Format("{0}member/userdetail/{1}.aspx", GetApplicationPath(), userId);
        }

		/// <summary>
		/// 订单成功页面
		/// </summary>
		public static String GetOrderSucessUrl(long orderId)
		{
			return String.Format("{0}order/success/{1}.aspx", GetApplicationPath(), orderId);
		}

		/// <summary>
		/// 订单支付成功页面
		/// </summary>
		public static String GetPaySucessUrl(string orderId)
		{
			return String.Format("{0}order/paysuccess/{1}.aspx", GetApplicationPath(), orderId);
		}

		/// <summary>
		/// 订单确认页面
		/// </summary>
		public static String GetOrderConfirmUrl()
		{
			return String.Format("{0}order/confirm.aspx", GetApplicationPath());
		}

		/// <summary>
		/// 得到新闻明细url
		/// </summary>
		public static String GetArticleDtlUrl(String id)
		{
			return String.Format("{0}article/{1}.aspx", GetApplicationPath(), id);
		}

		/// <summary>
		/// 得到帮助页面url
		/// </summary>
		/// <param name="id"></param>
		public static String GetHelpUrl(String id)
		{
			return String.Format("{0}help/{1}.aspx", GetApplicationPath(), id);
		}
		/// <summary>
		/// 得到新闻列表url
		/// </summary>
		public static String GetArticleListUrl(String typeId)
		{
			return String.Format("{0}articles/{1}.aspx", GetApplicationPath(), typeId);
		}
		/// <summary>
		/// 得到新闻列表url
		/// </summary>
		/// <param name="typeId">类型</param>
		/// <param name="currentIndex">当前页</param>
		public static String GetArticleListPageUrl(String typeId, String currentIndex)
		{
			return String.Format("{0}articlelist/{1}/{2}.aspx", GetApplicationPath(), typeId, currentIndex);
		}
		/// <summary>
		/// 从request中得到参数为returnurl的值
		/// </summary>
		/// <param name="canFromRef">如果当前request中无returnurl，则是否允许从urlReferrer中取</param>
		public static String GetReturnUrl(Boolean canFromRef)
		{
			HttpRequest request = HttpContext.Current.Request;
			var returnUrl = request.QueryString["returnurl"];
			if (String.IsNullOrEmpty(returnUrl) && canFromRef)
			{
				if (request.UrlReferrer != null)
				{
					String search = request.UrlReferrer.PathAndQuery.ToLower();
					String param = "returnurl";
					Int32 i = search.IndexOf(param + "=");
					if (i > -1)
					{
						i = i + param.Length;
						Int32 j = search.IndexOf("&", i);
						if (j == -1)
						{
							j = search.Length;
						}
						returnUrl = search.Substring(i + 1, j - i - 1);
					}
					if (!String.IsNullOrEmpty(returnUrl))
					{
						returnUrl = HttpContext.Current.Server.UrlDecode(returnUrl);
					}
				}
			}

			return returnUrl;
		}

		/// <summary>
		/// 是否首页
		/// </summary>
		public static bool IsHome()
		{
			var rawurl = HttpContext.Current.Request.RawUrl;
			return GetApplicationPath().EqualOrdinalIgnoreCase(rawurl) || rawurl.IndexOf(GetApplicationPath().Contact("default.aspx"), StringComparison.OrdinalIgnoreCase) >= 0;
		}

		/// <summary>
		/// 网站logo的URL地址
		/// </summary>

		/// <summary>
		/// 团购详情
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public static String GetGroupDetailUrl(object promotionID, object productID)
		{
			return String.Format("{0}group/{1}-{2}.aspx", GetApplicationPath(), promotionID.ToString(), productID.ToString());
		}

		/// <summary>
		/// 秒杀详情
		/// </summary>
		public static String GetSecKillDetailUrl(object promotionID, object productID)
		{
			return String.Format("{0}seckill/{1}-{2}.aspx", GetApplicationPath(), promotionID.ToString(), productID.ToString());
		}

		/// <summary>
		/// 跳转到404页面
		/// </summary>
		public static void RedirectTo404Page()
		{
			HttpContext.Current.Server.Transfer("~/404.aspx");
		}

		/// <summary>
		/// 商品搜索
		/// </summary>
		public static string GetProductSearchUrl(string keywords)
		{
			return string.Format("{0}search.aspx?keywords={1}", GetApplicationPath(), Uri.EscapeDataString(keywords));
		}


		#region pop 相关

		public static string GetPOPSellerIndex()
		{
			return string.Format("{0}seller/index.aspx", GetApplicationPath());
		}

		public static string GetPOPOrderList(string queryString)
		{
			return string.Format("{0}seller/orders.aspx{1}", GetApplicationPath(), queryString);
		}

		public static string GetPOPSellerProductListUrl(string queryString)
		{
			return string.Format("{0}seller/products.aspx{1}", GetApplicationPath(), queryString);
		}

		public static string GetPOPIndex(long shopID)
		{
			return string.Format("{0}shop/{1}.aspx", GetApplicationPath(), shopID);
		}


		public static string GetPOPProductListUrl(long popID, string queryString)
		{
			return string.Format("{0}shop/{1}/products.aspx{2}", GetApplicationPath(), popID, queryString);
		}


		#endregion

		#region 渠道相关
		public static string GetSaleChannelMyUsers(string queryString)
		{
			return string.Format("{0}member/myusers.aspx{1}", GetApplicationPath(), queryString);
		}

		public static string GetRegisterMyUser()
		{
			return String.Concat(GetApplicationPath(), "widgets/registermyuser.aspx");
		}

		public static string GetSaleChannelTransfer()
		{
			return string.Format("{0}member/sctransfer.aspx", GetApplicationPath());
		}

		public static string GetSelectMyUser()
		{
			return String.Concat(GetApplicationPath(), "widgets/selectmyuser.aspx");
		}

		public static string GetSelectMyCard()
		{
			return String.Concat(GetApplicationPath(), "widgets/selectmycard.aspx");
		}

		public static string GetSelectMyCard(string queryString)
		{
			return String.Concat(GetApplicationPath(), "widgets/selectmycard.aspx", queryString);
		}

		public static string GetSaleChannelMyUsedCard(string queryString)
		{
			return string.Format("{0}member/myusedcard.aspx{1}", GetApplicationPath(), queryString);
		}

		public static string GetSelectBuyer()
		{
			return String.Concat(GetApplicationPath(), "widgets/selectbuyer.aspx");
		}

		public static string GetSaleChannelMyEmptyCard(string queryString)
		{
			return string.Format("{0}member/myemptycard.aspx{1}", GetApplicationPath(), queryString);
		}
		public static string GetCardRecharge()
		{
			return String.Concat(GetApplicationPath(), "widgets/cardrecharge.aspx");
		}

		public static string GetSaleChannelMySaledCard(string queryString)
		{
			return string.Format("{0}member/mysaledcard.aspx{1}", GetApplicationPath(), queryString);
		}

		public static string GetSaleChannelMyDistributeCard(string queryString)
		{
			return string.Format("{0}member/mydistributecard.aspx{1}", GetApplicationPath(), queryString);
		}

        public static string GetPartnerOrder(string queryString)
        {
            return string.Format("{0}member/partenerorder.aspx{1}", GetApplicationPath(), queryString);
        }
        public static string GetSupplyPartnerOrder(string queryString)
        {
            return string.Format("{0}member/supplypartnerorder.aspx{1}", GetApplicationPath(), queryString);
        }
        public static string GetDayStatistics(string queryString)
        {
            return string.Format("{0}member/daystatistics.aspx{1}", GetApplicationPath(), queryString);
        }
        public static string GetYearStatistics(string queryString)
        {
            return string.Format("{0}member/yearstatistics.aspx{1}", GetApplicationPath(), queryString);
        }
        public static string GetMonthStatistics(string queryString)
        {
            return string.Format("{0}member/monthstatistics.aspx{1}", GetApplicationPath(), queryString);
        }

        public static string GetSaleCustomer(string queryString)
        {
            return string.Format("{0}member/salecustomer.aspx{1}", GetApplicationPath(), queryString);
        }
        public static string GetRankingList(string queryString)
        {
            return string.Format("{0}member/rankinglist.aspx{1}", GetApplicationPath(), queryString);
        }
        
		#endregion
	}
}
