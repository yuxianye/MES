using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Utility.Filter;
using OSharp.Web.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace Solution.Web.App.Areas.Service
{
    /// <summary>
    /// 控制器抽象基类，封装分页查询数据
    /// </summary>
    [Authorize]
    public abstract class ServiceBaseApiController : BaseApiController
    {
        protected ServiceBaseApiController()
        {
        }

        protected PageResult<TResult> GetPageResult<TEntity, TResult>(IQueryable<TEntity> source,
            Expression<Func<TEntity, TResult>> selector,
            GridRequest request = null)
        {
            if (request == null)
            {
                request = new GridRequest(new PageRepuestParams());
            }
            Expression<Func<TEntity, bool>> predicate = FilterHelper.GetExpression<TEntity>(request.FilterGroup);
            return source.ToPage(predicate, request.PageCondition, selector);
        }
    }

    /// <summary>
    /// Grid查询请求
    /// </summary>
    public class GridRequest
    {
        /// <summary>
        /// 初始化一个<see cref="GridRequest"/>类型的新实例
        /// </summary>
        public GridRequest(PageRepuestParams request)
        {
            var jsonWhere = request.FilterGroup;
            FilterGroup = jsonWhere != null ? jsonWhere : new FilterGroup();

            int pageIndex = request.PageIndex;
            int pageSize = request.PageSize;
            PageCondition = new PageCondition(pageIndex, pageSize);
            string sortField = request.SortField;
            string sortOrder = request.SortOrder;
            if (!sortField.IsNullOrEmpty() && !sortOrder.IsNullOrEmpty())
            {
                string[] fields = sortField.Split(",", true);
                string[] orders = sortOrder.Split(",", true);
                if (fields.Length != orders.Length)
                {
                    throw new ArgumentException("查询列表的排序参数个数不一致。");
                }
                List<SortCondition> sortConditions = new List<SortCondition>();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (orders[i].Length > 0)
                    {
                        ListSortDirection direction = orders[i].ToLower() == "desc"
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending;
                        sortConditions.Add(new SortCondition(fields[i], direction));
                    }
                }
                PageCondition.SortConditions = sortConditions.ToArray();
            }
            else
            {
                PageCondition.SortConditions = new SortCondition[] { };
            }
        }

        /// <summary>
        /// 获取 查询条件组
        /// </summary>
        public FilterGroup FilterGroup { get; private set; }

        /// <summary>
        /// 获取 分页查询条件信息
        /// </summary>
        public PageCondition PageCondition { get; private set; }

        /// <summary>
        /// 添加默认排序条件，只有排序条件为空时有效
        /// </summary>
        /// <param name="conditions"></param>
        public void AddDefaultSortCondition(params SortCondition[] conditions)
        {
            if (PageCondition.SortConditions.Length == 0)
            {
                PageCondition.SortConditions = conditions;
            }
        }

    }

    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageRepuestParams
    {
        /// <summary>
        /// 判断条件
        /// </summary>
        public FilterGroup FilterGroup { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 查询列表(,分割组成字符串)
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序参数(,分割组成字符串)
        /// </summary>
        public string SortOrder { get; set; }

    }
}