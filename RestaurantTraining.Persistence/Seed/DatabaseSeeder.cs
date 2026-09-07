using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // 1. Seed Roles
            var roleNames = new Dictionary<string, string>
            {
                { "Content Creator", "Creates and manages training content." },
                { "HR", "Human Resources training role." },
                { "Admin", "Administrative training role." },
                { "Accounts", "Accounts training role." },
                { "Managers", "Management training role." },
                { "Chefs", "Chef training role." },
                { "Stewards", "Steward training role." },
                { "Housekeeping", "Housekeeping training role." },
                { "Order Takers", "Order taking training role." },
                { "Cashier", "Cashier training role." },
                { "Commis 1", "Commis 1 training role." },
                { "Commis 2", "Commis 2 training role." },
                { "Commis 3", "Commis 3 training role." },
                { "Storekeeper", "Storekeeper training role." },
                { "Utility", "Utility training role." },
                { "Maintenance", "Maintenance training role." },
                { "Security", "Security training role." }
            };

            foreach (var roleData in roleNames)
            {
                var roleExists = await context.Roles
                    .AnyAsync(x => x.RoleName == roleData.Key);

                if (!roleExists)
                {
                    var role = new Role
                    {
                        RoleName = roleData.Key,
                        Description = roleData.Value,
                        IsActive = true
                    };

                    await context.Roles.AddAsync(role);
                }
            }

            await context.SaveChangesAsync();

            // 2. Find the Content Creator role
            var contentCreatorRole = await context.Roles
                .FirstAsync(x => x.RoleName == "Content Creator");

            // 3. Seed initial Content Creator user
            var contentCreator = await context.Users
                .FirstOrDefaultAsync(x => x.Email == "creator@restaurant.com");

            if (contentCreator == null)
            {
                var passwordHasher = new PasswordHasher<User>();

                contentCreator = new User
                {
                    FullName = "Khushi S",
                    Email = "creator@restaurant.com",
                    PhoneNumber = "0000000000",
                    RoleId = contentCreatorRole.RoleId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                contentCreator.PasswordHash =
                    passwordHasher.HashPassword(
                        contentCreator,
                        "Creator@123");

                await context.Users.AddAsync(contentCreator);
                await context.SaveChangesAsync();
            }
            else if (contentCreator.RoleId != contentCreatorRole.RoleId)
            {
                // Ensure an existing seeded user points at the Content Creator role
                contentCreator.RoleId = contentCreatorRole.RoleId;
                await context.SaveChangesAsync();
            }
        }
    }
}