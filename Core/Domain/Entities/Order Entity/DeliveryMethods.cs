namespace Domain.Entities.Order_Entity
{
    public class DeliveryMethods : BaseEntity<int>
    {
        public DeliveryMethods()
        {
            
        }
        public DeliveryMethods(string shortName, string description, string deliveryTime, decimal cost)
        {
            ShortName = shortName;
            Description = description;
            DeliveryTime = deliveryTime;
            Cost = cost;
        }

        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Cost { get; set; }
    }
}
