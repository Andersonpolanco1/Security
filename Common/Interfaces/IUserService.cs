﻿using Common.Models;

namespace Common.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserCreateModel userCreate);
        Task<bool> VerifyPasswordAsync(string username, string password);
        Task<User?> GetUserByCredentialsAsync(string username, string password);
        User GetUserById(string id);
    }
}