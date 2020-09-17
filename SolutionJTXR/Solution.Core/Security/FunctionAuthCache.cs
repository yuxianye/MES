﻿// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCache.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-06-03 21:04</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Core.Security;
using Solution.Core.Models.Identity;
using Solution.Core.Models.Security;


namespace Solution.Core.Security
{
    public class FunctionAuthCache : FunctionAuthCacheBase<Function, Guid, Module, int, Role, int, User, int>
    { }
}