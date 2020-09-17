// -----------------------------------------------------------------------
//  <copyright file="SolutionDbContext.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-07-01 2:02</last-date>
// -----------------------------------------------------------------------

using OSharp.Data.Entity;


namespace Solution.Core.Data
{
    public class SolutionDbContext : DbContextBase<SolutionDbContext>
    {
        public SolutionDbContext()
        {
            //var objectContext = (this as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
            //objectContext.CommandTimeout = 120;

        }

    }
}