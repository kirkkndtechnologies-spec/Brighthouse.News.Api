using Brighthouse.News.Api.Application.Dto;
using FluentValidation;

namespace Brighthouse.News.Api.Features.ArticleDisplay
{
    public class ArticleGetValidator : AbstractValidator<ArticleDetailInputDto>
    {
        public ArticleGetValidator()
        {
            RuleFor(r => r.ArticleId).GreaterThan(0).WithMessage("Please specify a valid article id");
        }
    }
}
