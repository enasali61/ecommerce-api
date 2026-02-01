using Domain;
using Domain.Entities.Order_Entity;

namespace Services.Specefication
{
    internal class OrderWithIncludeSpec : Specefication<Order>
    {
        // get order by id return one object 
        public OrderWithIncludeSpec(Guid id) : base(
            o=> o.Id == id )
        {
            AddInclude(o => o.DeliveryMethods);
            AddInclude(o => o.OrderItems);

        }
        // get all orders by email return collection
        public OrderWithIncludeSpec(string email) : base(
            o => o.UserEmail == email)
        {
            AddInclude(o => o.DeliveryMethods);
            AddInclude(o => o.OrderItems);
            
            SetOrderBy(o => o.OrderDate);
        }
    }
}
