using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Presistance
{
    static class SpeceficationEvaluator
    {
        // take dbcontext.set<t>() == is the input query
        public static IQueryable<T> GetQuery<T>(IQueryable< T> inputQuery, Specefication<T>specefication)
            where T : class
        {

            var query = inputQuery; //dbcontext.set<product> مثلا بعتت من نوع دا
            // criteria == .where (p=>p.id)
            if (specefication.Criteria is not null) //p=> p.id==1 
            {
                query = query.Where(specefication.Criteria); // it is like dbcontext.set<product>.where(p=> p.id==1 )
            }

            //// add include p=> p.productbrand and producttype
            //foreach (var item in specefication.IncludeExp)
            //    query = query.Include(item); //dbcontext.set<product>.where(p=> p.id==1 ).include(productBrand).include(productType)  كانني ضفت دا
            

            //OR           
            query = specefication.IncludeExp.Aggregate(query, (currentQuery, includeExp) => currentQuery.Include(includeExp)); // take current qury and add expression with it

            if (specefication.OrderBy is not null)
                query = query.OrderBy(specefication.OrderBy); // add after include the orderby exp OrderBy(p=>p.name)

            else if (specefication.OrderByDecending is not null)
                query = query.OrderByDescending(specefication.OrderByDecending);

            if (specefication.IsPaginated)
            {
                query = query.Skip(specefication.Skip).Take(specefication.Take);
            }
            return query;


        }

        
    }
}
