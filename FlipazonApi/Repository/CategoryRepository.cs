using FlipazonApi.DatabaseContext;
using FlipazonApi.Models;
using FlipazonApi.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlipazonApi.Repository
{
    public class CategoryRepository(FlipazonContext context) : ICategoryRepository
    {
        private readonly FlipazonContext _context = context;

        public Task<List<Category>> GetAllCategories()
        {
            return _context.Categories.ToListAsync();
        }

        public Task<Category?> GetProductsByCategoryIdAsync(int categoryId)
        {
            return _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

    }
}
