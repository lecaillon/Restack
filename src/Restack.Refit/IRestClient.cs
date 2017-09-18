namespace Restack.Refit
{
    public interface IRestClient<TClient>
    {
        TClient Client { get; set; }
    }
}
