using Brighthouse.News.Api.Application.Dto;
using FluentValidation;

namespace Brighthouse.News.Api.Features.ArticleManage
{
    public class ArticleAddValidator : AbstractValidator<ArticleAddDto>
    {
        public ArticleAddValidator()
        {

            RuleFor(r => r.AuthorId).GreaterThan(0).WithMessage("Please specify a valid author id");
            RuleFor(r => r.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(r => r.Summary).NotEmpty().WithMessage("Summary is required");
            RuleFor(r => r.Content).NotEmpty().WithMessage("Content is required");

        }
    }
}
