using exmaple_identity.Models;
using exmaple_identity.Repositories.Interface;

namespace exmaple_identity.Repositories.Implematation
{
    public class IdentityRepository : IIdentityRepository
    {
        public Task<LoginModel> GetUserName()
        {
            throw new NotImplementedException();
        }
    }
}
