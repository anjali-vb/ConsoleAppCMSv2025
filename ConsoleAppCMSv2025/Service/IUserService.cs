using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public interface IUserService
    {
        Task<int> AuthenticateUserByRoleIdAsync(string userName, string password);

        public interface IUserService
        {
            Task<int> AuthenticateUserByRoleIdAsync(string username, string password);
        }
    }
}