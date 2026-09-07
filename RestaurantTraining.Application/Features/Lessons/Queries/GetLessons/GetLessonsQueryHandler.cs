using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Lessons.Queries.GetLessons
{
    public class GetLessonsQueryHandler
        : IRequestHandler<GetLessonsQuery, List<LessonDto>>
    {
        private readonly ILessonRepository _lessonRepository;

        public GetLessonsQueryHandler(
            ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<List<LessonDto>> Handle(
            GetLessonsQuery request,
            CancellationToken cancellationToken)
        {
            var lessons = await _lessonRepository.GetAllLessonsAsync(
                cancellationToken);

            return lessons.Select(x => new LessonDto
            {
                LessonId = x.LessonId,
                ModuleId = x.ModuleId,
                LessonTitle = x.LessonTitle,
                Description = x.Description,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive
            }).ToList();
        }
    }
}