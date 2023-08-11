using Cbms.Authorization.Users;
using System;
using System.Threading.Tasks;

namespace Cbms.Kms.Domain.Users
{
    public interface IUserCreator
    {
        Task<User> CreateAsync(string roleName, string userName, string password, string name, string email, string mobilePhone, DateTime? birthday);
    }
}
