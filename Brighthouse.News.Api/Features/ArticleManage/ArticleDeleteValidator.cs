using Brighthouse.News.Api.Application.Dto;
using FluentValidation;

namespace Brighthouse.News.Api.Features.ArticleManage
{
    public class ArticleDeleteValidator : AbstractValidator<ArticleDetailInputDto>
    {
        public ArticleDeleteValidator()
        {
            RuleFor(r => r.ArticleId).GreaterThan(0).WithMessage("Please specify a valid article id");
        }
    }
}
