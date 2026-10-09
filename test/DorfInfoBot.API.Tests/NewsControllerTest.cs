using AutoMapper;
using DorfInfoBot.API.Controllers;
using DorfInfoBot.API.Models;
using DorfInfoBot.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace DorfInfoBot.API.Tests
{

    public class NewsControllerTest
    {
        // arrange data section part one "global"
        private readonly Mock<ILogger<NewsController>> _mockLogger;
        private readonly Mock<INewsRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly NewsController _controller;

        public NewsControllerTest(){
            _mockLogger = new Mock<ILogger<NewsController>>();
            _mockRepo = new Mock<INewsRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new NewsController(_mockLogger.Object,_mockRepo.Object,_mockMapper.Object);
        }

        [Fact]
        public void NewsTestGetAll()
        {
            // arrange data section part two "local"
            int expectedStatusCode = 200;

            // act section
            IActionResult response = _controller.GetNews();

            // assert section
            Assert.NotNull(response);
            OkObjectResult objectResponse = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(expectedStatusCode,objectResponse.StatusCode);
            NewsWithoutAttachmentDto[] dtoResponse = Assert.IsType<NewsWithoutAttachmentDto[]>(objectResponse.Value);
        }
    }
}
