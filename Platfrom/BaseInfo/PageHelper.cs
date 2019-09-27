using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Gsafety.PTMS.BaseInfo
{
    public static class PageHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="source"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAddOrderBy">是否添加排序，是否使用后边的orderby参数排序</param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T, Tkey>(this IQueryable<T> source, out int totalCount, int pageIndex = 1, int pageSize = 10, bool isAddOrderBy = true, Expression<Func<T, Tkey>> orderBy = null, bool ascending = false)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            totalCount = source.Count();

            CheckPageInfo(totalCount, ref pageIndex, ref pageSize);

            if (isAddOrderBy)
            {
                source = OrderBy(source, orderBy, ascending);
            }

            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAddOrderBy">是否添加排序，添加的排序是按照“”排序</param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> source, out int totalCount, int pageIndex = 1, int pageSize = 10, bool isAddOrderBy = true)
        {
            return Page<T, string>(source, out totalCount, pageIndex, pageSize, isAddOrderBy);
        }

        private static void CheckPageInfo(int totalCount, ref int pageIndex, ref int pageSize)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = totalCount;
            }

            if ((pageIndex - 1) * pageSize > totalCount)
            {
                var left = totalCount % pageSize;
                var div = totalCount / pageSize;
                if (left > 0)
                {
                    pageIndex = div + 1;
                }
                else
                {
                    pageIndex = div;
                }
            }
        }

        /// <summary>
        /// paixu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        private static IQueryable<T> OrderBy<T, Tkey>(IQueryable<T> source, Expression<Func<T, Tkey>> orderBy = null, bool ascending = false)
        {
            if (orderBy == null)
            {
                source = source.OrderBy(t => "");
            }
            else
            {
                if (ascending)
                {
                    source = source.OrderBy(orderBy);
                }
                else
                {
                    source = source.OrderByDescending(orderBy);
                }
            }

            return source;
        }
    }
}
