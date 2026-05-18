using FluentAssertions;
using LibraryManagementApi.API.DTOs;
using LibraryManagementApi.API.Models;
using LibraryManagementApi.API.Repositories.Interfaces;
using LibraryManagementApi.API.Services;
using LibraryManagementApi.Tests.TestData;
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
            BookTestData.ValidBook()
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

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
    {
        // Arrange

        var book = BookTestData.ValidBook();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(book);

        // Act

        var result = await _service.GetByIdAsync(
            1,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result!.Title.Should().Be("Clean Architecture");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBookDoesNotExist()
    {
        // Arrange

        _repositoryMock
            .Setup(r => r.GetByIdAsync(
                99,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act

        var result = await _service.GetByIdAsync(
            99,
            CancellationToken.None);

        // Assert

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateBook()
    {
        // Arrange

        var dto = new CreateBookDto
        {
            Title = "Domain-Driven Design",
            Author = "Eric Evans",
            ISBN = "987654321",
            PublishedYear = 2003,
            AvailableCopies = 10
        };

        var createdBook = BookTestData.ValidBook();

        _repositoryMock
            .Setup(r => r.CreateAsync(
                It.IsAny<Book>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdBook);

        // Act

        var result = await _service.CreateAsync(
            dto,
            CancellationToken.None);

        // Assert

        result.Should().NotBeNull();

        result.Title.Should().Be(dto.Title);

        result.Author.Should().Be(dto.Author);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange

        var dto = new UpdateBookDto
        {
            Title = "Updated",
            Author = "Updated",
            ISBN = "111",
            PublishedYear = 2024,
            AvailableCopies = 1
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(
                99,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act

        var result = await _service.UpdateAsync(
            99,
            dto,
            CancellationToken.None);

        // Assert

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenBookExists()
    {
        // Arrange

        var existingBook = BookTestData.ValidBook();

        var dto = new UpdateBookDto
        {
            Title = "New",
            Author = "New",
            ISBN = "999",
            PublishedYear = 2024,
            AvailableCopies = 10
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        // Act

        var result = await _service.UpdateAsync(
            1,
            dto,
            CancellationToken.None);

        // Assert

        result.Should().BeTrue();

        existingBook.Title.Should().Be(dto.Title);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange

        _repositoryMock
            .Setup(r => r.GetByIdAsync(
                99,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Book?)null);

        // Act

        var result = await _service.DeleteAsync(
            99,
            CancellationToken.None);

        // Assert

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenBookExists()
    {
        // Arrange

        var existingBook = new Book
        {
            Id = 1,
            Title = "Test Book"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBook);

        // Act

        var result = await _service.DeleteAsync(
            1,
            CancellationToken.None);

        // Assert

        result.Should().BeTrue();

        _repositoryMock.Verify(
            r => r.DeleteAsync(
                existingBook,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}