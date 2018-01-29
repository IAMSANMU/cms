#region Summary

//===================================================================
//
// Author:    刘冰 jex536@163.com
//
// Project:	  EC
//
// File:      JsonResponse.cs
//	
// Contents:
//			  相关描述
//
// Version:   1.0.0.0	
//
// Copyright (C) Jex Corporation. All rights reserved.
//
// ===================================================================
//
// History:	    2011-8-18 14:32:51 Created
// ===================================================================

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace imow.Framework.Tool
{
    public sealed class JsonResponse
    {
        #region 字段



        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置返回的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 获取或者设置消息（比如错误消息）
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或者设置是否成功
        /// </summary>
        public bool Success { get; set; }

        #endregion

        #region ctor

        public JsonResponse()
        {
            this.Message = string.Empty;
            this.Success = false;
        }

        #endregion

        #region Public 方法

        public JsonResult ToJsonResult()
        {
            return new JsonResult { Data = this };
        }


        #endregion

        #region Private 方法



        #endregion
    }
}
