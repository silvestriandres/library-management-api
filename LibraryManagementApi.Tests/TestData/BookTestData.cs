using LibraryManagementApi.API.Models;

namespace LibraryManagementApi.Tests.TestData;

public static class BookTestData
{
    public static Book ValidBook()
    {
        return new Book
        {
            Id = 1,
            Title = "Clean Code",
            Author = "Robert C. Martin",
            ISBN = "123456789",
            PublishedYear = 2008,
            AvailableCopies = 5
        };
    }
}