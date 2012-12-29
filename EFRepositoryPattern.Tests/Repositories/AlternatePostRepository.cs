using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using EFRepository.Queryable;
using EFRepositoryPattern.Tests.Models;

namespace EFRepositoryPattern.Tests.Repositories
{
    public class AlternatePostRepository : PostRepository
    {
        public AlternatePostRepository(BlogContext context) : base(context)
        {
        }

        /// <summary>
        /// Override of Retrieve that builds an explicit SQL query to do the work instead of have EF do it
        /// </summary>
        /// <param name="criteria">The post criteria</param>
        /// <param name="orderBy">The criteria for ordering the results</param>
        /// <returns>All of the Posts in the database which satisfy the criteria</returns>
        /// <remarks>
        /// This is an example to demonstrate replacing an EF-based method with a custom method. 
        /// Because the Order class and the custom criteria class provide enough information, 
        /// EF's generated query can be completely replaced with something else. Here it's a SQL 
        /// statement that we cobble together; it could just as easily be a stored procedure or a query
        /// against a NoSQL store of some kind. You're not locked into EF or SQL Server in any way.
        /// </remarks>
        public override IEnumerable<Post> Retrieve(PostCriteria criteria = null, params Order<Post>[] orderBy)
        {
            string whereClause = string.Empty;
            var parameters = new List<DbParameter>();

            string orderByClause = string.Empty;

            if(criteria != null)
            {
                if(!string.IsNullOrEmpty(criteria.Title))
                {
                    if(whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }

                    whereClause += " title = @title";
                    var param = new SqlParameter("title", criteria.Title);
                    parameters.Add(param);
                }

                if(criteria.AfterDate.HasValue)
                {
                    if(whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }

                    whereClause += " publishdate >= @afterDate";
                    var param = new SqlParameter("afterDate", criteria.AfterDate.Value);
                    parameters.Add(param);
                }

                if(criteria.BeforeDate.HasValue)
                {
                    if(whereClause.Length > 0)
                    {
                        whereClause += " and ";
                    }

                    whereClause += " publishdate < @beforeDate";
                    var param = new SqlParameter("beforeDate", criteria.BeforeDate.Value);
                    parameters.Add(param);
                }
            }

            foreach(var o in orderBy)
            {
                if(orderByClause.Length > 0)
                {
                    orderByClause += ", ";
                }
                else
                {
                    orderByClause += "order by ";
                }

                // For the love of all that's holy, if you implement something like this
                // in the real world make sure you sanitize this 'order by' clause that you're
                // building to avoid SQL injection
                orderByClause += o.PropertyName + (o.Descending ? " desc" : " asc");
            }

            if(whereClause.Length > 0)
            {
                whereClause = "where " + whereClause;
            }

            string command = string.Format("select * from posts {0} {1}", whereClause, orderByClause);

            return _context.Database.SqlQuery<Post>(command, parameters.Cast<object>().ToArray());
        }
    }
}