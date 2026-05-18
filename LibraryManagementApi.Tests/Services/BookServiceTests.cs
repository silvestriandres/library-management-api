using FluentAssertions;
using LibraryManagementApi.API.DTOs;
using LibraryManagementApi.API.Models;
using LibraryManagementApi.API.Repositories.Interfaces;
using LibraryManagementApi.API.Services;
using Moq;

namespace LibraryManagementApi.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _repositoryMock;

    private readonly BookService _service;

    public BookServiceTests()
    {
        _repositoryMock = new Mock<IBookRepository>();

        _service = new BookService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnBooks()
    {
        // Arrange

        var books = new List<Book>
        {
            new()
            {
                Id = 1,
                Title = "Clean Code",
                Author = "Robert C. Martin",
                ISBN = "1234567890",
                PublishedYear = 2008,
                AvailableCopies = 5
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(books);

        // Act

        var result = await _service.GetAllAsync(
            1,
            10,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.Should().HaveCount(1);

        result.First().Title.Should().Be("Clean Code");
    }
}