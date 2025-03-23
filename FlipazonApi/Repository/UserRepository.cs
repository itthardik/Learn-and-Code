using FlipazonApi.DatabaseContext;
using FlipazonApi.Models;
using FlipazonApi.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlipazonApi.Repository
{
    public class UserRepository(FlipazonContext context) : IUserRepository
    {
        private readonly FlipazonContext _context = context;

        public Task<int> AddUser(string email, string password)
        {
            User newUser = new(email,password);
            _context.Users.Add(newUser);
            return _context.SaveChangesAsync();
        }

        public Task<User?> GetUser(string email)
        {
            var user = _context.Users.Where(u=> u.Email == email);
            return user.FirstOrDefaultAsync();
        }

    }
}
