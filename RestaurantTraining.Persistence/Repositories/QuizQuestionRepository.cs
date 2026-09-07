using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Persistence.Repositories
{
    public class QuizQuestionRepository : IQuizQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuizQuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddQuizQuestionAsync(
            QuizQuestion question,
            CancellationToken cancellationToken)
        {
            await _context.QuizQuestions.AddAsync(
                question,
                cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return question.QuestionId;
        }

        public async Task<List<QuizQuestion>> GetAllQuizQuestionsAsync(
            CancellationToken cancellationToken)
        {
            return await _context.QuizQuestions
                .ToListAsync(cancellationToken);
        }

        public async Task<QuizQuestion?> GetQuizQuestionByIdAsync(
            int questionId,
            CancellationToken cancellationToken)
        {
            return await _context.QuizQuestions
                .FirstOrDefaultAsync(
                    x => x.QuestionId == questionId,
                    cancellationToken);
        }

        public async Task UpdateQuizQuestionAsync(
            QuizQuestion question,
            CancellationToken cancellationToken)
        {
            _context.QuizQuestions.Update(question);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        public async Task DeleteQuizQuestionAsync(
            QuizQuestion question,
            CancellationToken cancellationToken)
        {
            // Remove the question's answer options first — they reference the
            // question via a foreign key with no cascade, so the question can't
            // be deleted while they still exist.
            var options = await _context.QuizOptions
                .Where(o => o.QuestionId == question.QuestionId)
                .ToListAsync(cancellationToken);

            _context.QuizOptions.RemoveRange(options);
            _context.QuizQuestions.Remove(question);

            await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}