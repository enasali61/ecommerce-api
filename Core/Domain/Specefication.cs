using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class Specefication<T> where T : class
    {
        //property for each and every spec
        public Expression<Func<T, bool>> ?Criteria { get; set; } // null when i dun need condition mean return all products no condition
        public List<Expression<Func<T, object>>> IncludeExp { get; set; } = new();

        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDecending { get; private set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginated { get; set; }

        protected Specefication()
        {
            
        }
        protected Specefication(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }

        // methods for returning one product only (include)
        protected void AddInclude(Expression<Func<T, object>> Exp)
        {
            //add expression to the list
            IncludeExp.Add(Exp);
        }

        protected void SetOrderBy(Expression<Func<T, object>> orderExpression)
        {
            OrderBy = orderExpression; // orderBy(p=>p.name)
        }

        protected void SetOrderByDescending(Expression<Func<T, object>> orderExpression)
        {
            OrderByDecending = orderExpression; // orderByDesc(p=>p.name)
        }

        // devide products into pages each have same amount of products
        // page size = 5 products 
        // page index = 3 skip and get the 3rd page
        // means skip 1st 2*5 = 10 products 
        protected void ApplyPagination(int  pageIndex, int pageSize)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

    }
}
