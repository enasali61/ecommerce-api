namespace Domain.Contracts
{
    public interface IDbIntializer
    {
        public Task IntializeAsync();
        public Task IntializeIdentityAsync();

    }
}
