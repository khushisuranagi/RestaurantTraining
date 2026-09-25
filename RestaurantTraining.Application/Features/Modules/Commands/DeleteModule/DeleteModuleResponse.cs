using RestaurantTraining.Application.Common.Responses;

namespace RestaurantTraining.Application.Features.Modules.Commands.DeleteModule
{
    public class DeleteModuleResponse : BaseResponse
    {
        public bool RequiresConfirmation { get; set; }   // "has lessons — ask first"
        public int LessonCount { get; set; }
    }
}