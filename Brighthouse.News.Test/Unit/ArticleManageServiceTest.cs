using Brighthouse.News.Api.Application.Dto;
using Brighthouse.News.Api.Domain;
using Brighthouse.News.Api.Features.ArticleDisplay;
using Brighthouse.News.Api.Features.ArticleManage;
using Brighthouse.News.Api.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Brighthouse.News.Test.Unit
{
    public class ArticleManageServiceTest
    {
        private readonly Mock<INewsRepository> mockRepository;
        private readonly Mock<ILogger<ArticleDisplayService>> mockDisplayLogger;
        private readonly Mock<ILogger<ArticleManageService>> mockManageLogger;
        private readonly ArticleDisplayService displayService;
        private readonly ArticleManageService manageService;
        private readonly ArticleGetValidator getValidator;
        private readonly ArticleAddValidator addValidator;
        private readonly ArticleUpdateValidator updateValidator;
        private readonly ArticleDeleteValidator deleteValidator;

        public ArticleManageServiceTest()
        {
            // Initialise and arrange
            mockRepository = new Mock<INewsRepository>();
            mockDisplayLogger = new Mock<ILogger<ArticleDisplayService>>();
            mockManageLogger = new Mock<ILogger<ArticleManageService>>();
            getValidator = new ArticleGetValidator();
            addValidator = new ArticleAddValidator();
            updateValidator = new ArticleUpdateValidator();
            deleteValidator = new ArticleDeleteValidator();
            
            displayService = new ArticleDisplayService(mockDisplayLogger.Object, mockRepository.Object, 
                getValidator);

            manageService = new ArticleManageService(mockManageLogger.Object, mockRepository.Object,
                addValidator, updateValidator, deleteValidator);

        }

        [Fact]
        public async Task AddArticleAsync_ReturnsNewId()
        {
            // Arrange expected result;
            var newArticle = new Article { AuthorId = 1, Title = "Test Title", Summary = "Test Summary", Content = "Test Content" };
            var newDto = new ArticleAddDto { AuthorId = 1, Title = "Test Title", Summary = "Test Summary", Content = "Test Content" };
            int expectedId = 101;

            mockRepository.Setup(repo => repo.AddArticleAsync(It.IsAny<Article>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await manageService.AddArticleAsync(newDto);

            var newId = Convert.ToInt32(result.Data);

            // Assert
            Assert.Equal(expectedId, newId);
        }

        [Fact]
        public async Task AddArticleAsync_AlreadyExists_ReturnsNewId()
        {
            // Arrange expected result;
            var newArticle = new Article { AuthorId = 1, Title = "Test Title", Summary = "Test Summary", Content = "Test Content" };
            var newDto = new ArticleAddDto { AuthorId = 1, Title = "Test Title", Summary = "Test Summary", Content = "Test Content" };
            int expectedId = 101;

            mockRepository.Setup(repo => repo.AddArticleAsync(It.IsAny<Article>()))
                .ReturnsAsync(expectedId);

            // Act
            var result = await manageService.AddArticleAsync(newDto);

            var newId = Convert.ToInt32(result.Data);

            // Assert
            Assert.Equal(expectedId, newId);
        }

        [Fact]
        public async Task UpdateArticleAsync_Success()
        {
            // Arrange expected result;
            var articleToUpdate = new Article { Id = 1, AuthorId = 1, Title = "Test Title", Summary = "Test Summary", Content = "Test Content" };
            string newTitle = "Test Title After Update";
            var newDetails = new ArticleUpdateDto { Id = 1, AuthorId = 1, Title = newTitle, Summary = "Test Summary", Content = "Test Content" };

            mockRepository.Setup(repo => repo.GetArticleAsync(It.IsAny<int>()))
                .ReturnsAsync(articleToUpdate);

            // Act
            var result = await manageService.UpdateArticleAsync(newDetails);

            var newId = Convert.ToInt32(result.Data);

            // Assert
            mockRepository.Verify(repo => repo.UpdateArticleAsync(
            It.Is<Article>(u => u.Id == 1 && u.Title == newTitle)),
            Times.Once());
        }

        [Fact]
        public async Task DeleteArticleAsync_Success()
        {
            // Arrange expected result;
            var articleIdToDelete = 2;
            var articleToDelete = new Article { Id = articleIdToDelete, AuthorId = 1, Title = "Test Title", Summary = "Test Summary", Content = "Test Content" };

            mockRepository.Setup(repo => repo.GetArticleAsync(articleIdToDelete))
                .ReturnsAsync(articleToDelete);

            // Act
            var result = await manageService.DeleteArticleAsync(new ArticleDetailInputDto { ArticleId = articleIdToDelete} );

            // Assert
            mockRepository.Verify(repo => repo.DeleteArticleAsync(articleIdToDelete), Times.Once());

        }

    }
}
