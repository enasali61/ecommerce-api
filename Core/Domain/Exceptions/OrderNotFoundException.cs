namespace Domain.Exceptions
{
    public class OrderNotFoundException(Guid id) : NotFoundException($"no order with id {id} was found")
    {
    }
}
