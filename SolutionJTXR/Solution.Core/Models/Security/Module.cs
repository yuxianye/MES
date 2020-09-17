// -----------------------------------------------------------------------
//  <copyright file="Module.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2016 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2016-03-13 2:37</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OSharp.Core.Security;
using OSharp.Core.Security.Models;
using Solution.Core.Models.Identity;


namespace Solution.Core.Models.Security
{
    /// <summary>
    /// 实体类——模块信息
    /// </summary>
    [Description("模块")]
    public class Module : ModuleBase<int, Module, Function, Guid, Role, int, User, int>
    {
        /// <summary>
        /// 图标
        /// </summary>
        [DisplayName("模块图标")]
        [StringLength(100)]
        public string Icon { get; set; }
    }
}