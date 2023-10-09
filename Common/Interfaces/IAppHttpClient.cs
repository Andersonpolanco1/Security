using Common.Http;

namespace Auth.Services.Interfaces
{
    public interface IAppHttpClient
    {
        Task<Result<Tout>> HttpPostAsync<Tout, Tin>(Tin content);
    }
}
