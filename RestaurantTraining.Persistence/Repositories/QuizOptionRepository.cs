using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class QuizOptionRepository : IQuizOptionRepository
    {
        private readonly AppDbContext _context;

        public QuizOptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddQuizOptionAsync(
            QuizOption option,
            CancellationToken cancellationToken)
        {
            await _context.QuizOptions.AddAsync(
                option,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return option.OptionId;
        }

        public async Task<List<QuizOption>> GetAllQuizOptionsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.QuizOptions
                .ToListAsync(cancellationToken);
        }

        public async Task<QuizOption?> GetQuizOptionByIdAsync(
            int optionId,
            CancellationToken cancellationToken)
        {
            return await _context.QuizOptions
                .FirstOrDefaultAsync(
                    x => x.OptionId == optionId,
                    cancellationToken);
        }

        public async Task UpdateQuizOptionAsync(
            QuizOption option,
            CancellationToken cancellationToken)
        {
            _context.QuizOptions.Update(option);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        public async Task DeleteQuizOptionAsync(
            QuizOption option,
            CancellationToken cancellationToken)
        {
            _context.QuizOptions.Remove(option);

            await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}