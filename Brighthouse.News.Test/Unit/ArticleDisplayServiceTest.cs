using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Domain;
using Brighthouse.News.Api.Features.ArticleDisplay;
using Brighthouse.News.Api.Infrastructure.Repositories;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;

namespace Brighthouse.News.Test.Unit
{
    public class ArticleDisplayServiceTest
    {
        private readonly Mock<INewsRepository> mockRepository;
        private readonly Mock<ILogger<ArticleDisplayService>> mockLogger;
        private readonly ArticleDisplayService service;
        private readonly ArticleGetValidator validator;

        public ArticleDisplayServiceTest()
        {
            // Initialise and arrange
            mockRepository = new Mock<INewsRepository>();
            mockLogger = new Mock<ILogger<ArticleDisplayService>>();
            validator = new ArticleGetValidator();

            service = new ArticleDisplayService(mockLogger.Object, mockRepository.Object, validator);
        }

        [Fact]
        public async Task GetArticlesAsync_ShouldReturnListOfArticles()
        {
            // Arrange expected result;
            var expectedArticles = new List<Article>
            {
                new Article { Id = 1, Title = "Test Article 1", Content = "Content 1" },
                new Article { Id = 2, Title = "Test Article 2", Content = "Content 2" }
            };

            mockRepository.Setup(repo => repo.GetArticlesAsync())
                .ReturnsAsync(expectedArticles);

            // Act
            var result = await service.GetArticlesAsync();

            // Assert
            Assert.NotNull(result);
            var articlesList = Assert.IsType<List<ArticleDto>>(result.Data);
            Assert.Equal(2, articlesList.Count);
        }

        [Fact]
        public async Task GetArticleAsync_ArticleExists_ReturnsArticle()
        {
            // Arrange expected result;
            var expectedArticle = new Article { Id = 1, Title = "Test Title", Content = "Test Content" };
            int articleIdToFetch = 1;

            mockRepository.Setup(repo => repo.GetArticleAsync(articleIdToFetch))
                .ReturnsAsync(expectedArticle);

            // Act
            var result = await service.GetArticleAsync(new ArticleDetailInputDto { ArticleId = articleIdToFetch });

            ArticleDetailDto response = result.Data as ArticleDetailDto;

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedArticle.Id, response.Id);
            Assert.Equal(expectedArticle.Title, response.Title);
            Assert.IsType<ArticleDetailDto>(response);
        }

        [Fact]
        public async Task GetArticleAsync_ArticleDoesNotExist_ReturnsValidationError()
        {
            // Arrange expected result;
            int articleIdDoesNotExist = 11001;
            string expectedMessage = "Article does not exist";

            mockRepository.Setup(repo => repo.GetArticleAsync(articleIdDoesNotExist))
                .ReturnsAsync((Article)null);

            // Act
            var result = await service.GetArticleAsync(new ArticleDetailInputDto { ArticleId = articleIdDoesNotExist });
            ValidationResult validationResult = result.ValidationResult;

            // Assert
            var error = validationResult.Errors.SingleOrDefault(e => e.PropertyName == "ArticleId" && e.ErrorMessage == expectedMessage);

            Assert.Null(result.Data);
            Assert.False(validationResult.IsValid);
            Assert.NotNull(error);
        }

    }
}
