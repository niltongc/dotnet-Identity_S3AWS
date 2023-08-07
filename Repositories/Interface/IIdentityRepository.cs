using exmaple_identity.Models;

namespace exmaple_identity.Repositories.Interface
{
    public interface IIdentityRepository
    {
        Task<LoginModel> GetUserName();
    }
}
