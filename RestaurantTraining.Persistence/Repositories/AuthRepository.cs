using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(
                    x => x.Email == email,
                cancellationToken);
        }

        public Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }

        public Task<Role?> GetRoleByIdAsync(
            int roleId,
            CancellationToken cancellationToken)
        {
            return _context.Roles.FirstOrDefaultAsync(
                x => x.RoleId == roleId && x.IsActive,
                cancellationToken);
        }

        public Task<List<Role>> GetSelfRegistrationRolesAsync(
            CancellationToken cancellationToken)
        {
            return _context.Roles
                .Where(x => x.IsActive && x.RoleName != "Content Creator")
                .OrderBy(x => x.RoleName)
                .ToListAsync(cancellationToken);
        }

        public async Task AddUserAsync(
            User user,
            CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(
            User user,
            CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<User?> GetUserByIdAsync(
    int userId,
    CancellationToken cancellationToken)
        {
            return _context.Users.FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);
        }

        public async Task DeleteAccountAsync(
            int userId,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (user is null)
                return;

            // The foreign keys don't cascade, so remove every child row first,
            // then the user. One SaveChanges = one transaction (all or nothing).
            var lessonProgress = _context.LessonProgress.Where(x => x.UserId == userId);
            var moduleProgress = _context.ModuleProgress.Where(x => x.UserId == userId);
            var quizAttempts = _context.QuizAttempts.Where(x => x.UserId == userId);
            var scenarioAttempts = _context.ScenarioAttempts.Where(x => x.UserId == userId);
            var certificates = _context.Certificates.Where(x => x.UserId == userId);

            _context.LessonProgress.RemoveRange(lessonProgress);
            _context.ModuleProgress.RemoveRange(moduleProgress);
            _context.QuizAttempts.RemoveRange(quizAttempts);
            _context.ScenarioAttempts.RemoveRange(scenarioAttempts);
            _context.Certificates.RemoveRange(certificates);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
