// -----------------------------------------------------------------------
//  <copyright file="Role.cs" company="OSharp开源团队">
//      Copyright (c) 2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-01-08 0:24</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Core.Identity.Models;


namespace Solution.Core.Models.Identity
{
    /// <summary>
    /// 实体类——角色信息
    /// </summary>
    [Description("认证-角色信息")]
    public class Role : RoleBase<int>
    {

        /// <summary>
        /// 获取或设置 角色所属组织机构
        /// </summary>
        public virtual Organization Organization { get; set; }
    }
}