using BlazorProject.Models;
using BlazorProject.Services;
namespace BlazorProject.Tests;




public class LibraryServiceTests
{
    //helper creates a fresh libraryservice with no files

    //pass in empty lists so every test starts clean

    private static LibraryService CreateService()
    {
        return new LibraryService(new List<Book>(), new List<User>());
    }

    //book test

    [Fact]
    public void AddBook_AddsBookToList()
    {
        // Arrange <-- how each method is going to be organized 
        var service = CreateService();
        var book = new Book { Title = "The Hobbit", Author = "Tolkien" };

        // Act <-- how each method is going to be organized 
        service.AddBook(book);

        // Assert <-- how each method is going to be organized 
        var books = service.ReadBooks();
        Assert.Single(books);                          //one book exists
        Assert.Equal("The Hobbit", books[0].Title);
        Assert.Equal("Tolkien",    books[0].Author);
    }




    [Fact]
    public void AddBook_AssignsIdAutomatically()
    {
        
        var service = CreateService();
        var book = new Book { Title = "Dune", Author = "Herbert" };

        
        service.AddBook(book);

        
        Assert.Equal(1, service.ReadBooks()[0].Id);    //first book should get Id 1
    }

    [Fact]
    public void AddBook_SecondBook_GetsNextId()
    {
        
        var service = CreateService();
        service.AddBook(new Book { Title = "Book One", Author = "Author A" });

       
        service.AddBook(new Book { Title = "Book Two", Author = "Author B" });

        
        var books = service.ReadBooks();
        Assert.Equal(2, books.Count);
        Assert.Equal(2, books[1].Id);//second book gets Id 2
    }

    [Fact]
    public void ReadBooks_ReturnsAllBooks()
    {
       
        var service = CreateService();
        service.AddBook(new Book { Title = "A", Author = "X" });
        service.AddBook(new Book { Title = "B", Author = "Y" });
        service.AddBook(new Book { Title = "C", Author = "Z" });

       
        var books = service.ReadBooks();

      


        Assert.Equal(3, books.Count);
    }

    [Fact]
    public void EditBook_UpdatesTitleAndAuthor()
    {
  
        var service = CreateService();
        service.AddBook(new Book { Title = "Old Title", Author = "Old Author" });
        int bookId = service.ReadBooks()[0].Id;

        
        service.EditBook(new Book { Id = bookId, Title = "New Title", Author = "New Author" });

  
        var updated = service.ReadBooks()[0];
        Assert.Equal("New Title",   updated.Title);
        Assert.Equal("New Author",  updated.Author);
    }

    [Fact]
    public void EditBook_WithWrongId_DoesNothing()
    {
    
        var service = CreateService();
        service.AddBook(new Book { Title = "Real Book", Author = "Real Author" });

   
        service.EditBook(new Book { Id = 999, Title = "Ghost", Author = "Nobody" });

        Assert.Equal("Real Book", service.ReadBooks()[0].Title);
    }

    [Fact]
    public void DeleteBook_RemovesBookFromList()
    {
        
        var service = CreateService();
        service.AddBook(new Book { Title = "To Delete", Author = "Someone" });
        int bookId = service.ReadBooks()[0].Id;

   
        service.DeleteBook(bookId);

        Assert.Empty(service.ReadBooks());
    }

    [Fact]
    public void DeleteBook_WithWrongId_DoesNothing()
    {
        // Arrange
        var service = CreateService();
        service.AddBook(new Book { Title = "Safe Book", Author = "Author" });

        // Act
        service.DeleteBook(999);

        // Assert – book is still there
        Assert.Single(service.ReadBooks());
    }





//users test
    [Fact]
    public void AddUser_AddsUserToList()
    {
        
        var service = CreateService();
        var user = new User { Name = "Alice" };

        service.AddUser(user);

       
        var users = service.ReadUsers();
        Assert.Single(users);
        Assert.Equal("Alice", users[0].Name);
    }

    [Fact]
    public void AddUser_AssignsIdAutomatically()
    {
       
        var service = CreateService();

        
        service.AddUser(new User { Name = "Bob" });

      
        Assert.Equal(1, service.ReadUsers()[0].Id);
    }

    [Fact]
    public void ReadUsers_ReturnsAllUsers()
    {
        // Arrange
        var service = CreateService();
        service.AddUser(new User { Name = "Alice" });
        service.AddUser(new User { Name = "Bob" });

        var users = service.ReadUsers();

        Assert.Equal(2, users.Count);
    }

    [Fact]
    public void EditUser_UpdatesName()
    {
        var service = CreateService();
        service.AddUser(new User { Name = "Old Name" });
        int userId = service.ReadUsers()[0].Id;

        service.EditUser(new User { Id = userId, Name = "New Name" });

        Assert.Equal("New Name", service.ReadUsers()[0].Name);
    }



    [Fact]
    public void EditUser_WithWrongId_DoesNothing()
    {
   
        var service = CreateService();
        service.AddUser(new User { Name = "Real User" });

     
        service.EditUser(new User { Id = 999, Name = "Ghost" });


        Assert.Equal("Real User", service.ReadUsers()[0].Name);
    }

    [Fact]
    public void DeleteUser_RemovesUserFromList()
    {
       
        var service = CreateService();
        service.AddUser(new User { Name = "To Delete" });
        int userId = service.ReadUsers()[0].Id;

      
        service.DeleteUser(userId);

        Assert.Empty(service.ReadUsers());
    }

    [Fact]
    public void DeleteUser_WithWrongId_DoesNothing()
    {
      
        var service = CreateService();
        service.AddUser(new User { Name = "Safe User" });

     
        service.DeleteUser(999);

    
        Assert.Single(service.ReadUsers());
    }


// borrow and return test
    [Fact]
    public void BorrowBook_AddsBookIdToUserList()
    {
        var service = CreateService();

        service.BorrowBook(userId: 1, bookId: 10);

        var borrowed = service.GetBorrowedBookIds(userId: 1);
        Assert.Contains(10, borrowed);
    }

    [Fact]
    public void ReturnBook_RemovesBookIdFromUserList()
    {
  
        var service = CreateService();
        service.BorrowBook(userId: 1, bookId: 10);

        service.ReturnBook(userId: 1, bookId: 10);

        var borrowed = service.GetBorrowedBookIds(userId: 1);
        Assert.DoesNotContain(10, borrowed);
    }

    [Fact]
    public void GetBorrowedBookIds_ReturnsEmptyList_WhenUserHasNotBorrowed()
    {
        var service = CreateService();

        var borrowed = service.GetBorrowedBookIds(userId: 42);

        Assert.Empty(borrowed);
    }
}
