using FluentValidation.Results;

namespace Brighthouse.News.Api.Models
{
    public class BrighthouseResponse
    {
        public string Message { get; set; } = default!;

        public bool Success { get; set; } = default!;

        public Object? Data { get; set; }

        public ValidationResult ValidationResult { get; set; } = default!;
    }
}
