using Brighthouse.News.Api.Features.ArticleDisplay;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;

namespace Brighthouse.News.Test
{
    public class ArticleDisplayTest
    {

        private readonly Mock<IArticleDisplayService> _articleDisplayServiceMock;
        private readonly Mock<ILogger<ArticleDisplayService>> _logMock;

        public ArticleDisplayTest()
        {
            _articleDisplayServiceMock = new Mock<IArticleDisplayService>();
            _logMock = new Mock<ILogger<ArticleDisplayService>>();
        }

        [Fact]
        public async Task Test1()
        {
            // Act
            var act = ArticleDisplayEndpoint.GetArticlesAsync(_logMock.Object, _articleDisplayServiceMock.Object);

            // Assert
            Assert.IsType<Ok>(act);
        }
    }
}
