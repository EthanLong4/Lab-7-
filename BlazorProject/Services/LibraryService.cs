using BlazorProject.Models;


namespace BlazorProject.Services;

public class LibraryService
{
    private readonly string _booksPath = "Books.csv";
    private readonly string _usersPath = "Users.csv";

    private List<Book> _books = new();
    private List<User> _users = new();
    private Dictionary<int, List<int>> _borrowedBooks = new();

    public LibraryService()
    {
        _books = LoadBooks();
        _users = LoadUsers();
    }

    // This constructor is used by unit tests so no files are needed.
    public LibraryService(List<Book> books, List<User> users)
    {
        _books = books;
        _users = users;
    }





    // Books

    public List<Book> ReadBooks() => _books;

    public void AddBook(Book book)
    {
        book.Id = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
        _books.Add(book);
        SaveBooks();
    }

    public void EditBook(Book book)
    {
        var existing = _books.FirstOrDefault(b => b.Id == book.Id);
        if (existing is null) return;

        existing.Title = book.Title;
        existing.Author = book.Author;
        SaveBooks();
    }

    public void DeleteBook(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null) return;

        _books.Remove(book);
        SaveBooks();
    }



    // users

    public List<User> ReadUsers() => _users;

    public void AddUser(User user)
    {
        user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        _users.Add(user);
        SaveUsers();
    }

    public void EditUser(User user)
    {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing is null) return;

        existing.Name = user.Name;
        SaveUsers();
    }

    public void DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user is null) return;

        _users.Remove(user);
        SaveUsers();
    }




    // borrow and return

    public void BorrowBook(int userId, int bookId)
    {
        if (!_borrowedBooks.ContainsKey(userId))
            _borrowedBooks[userId] = new List<int>();

        _borrowedBooks[userId].Add(bookId);
    }

    public void ReturnBook(int userId, int bookId)
    {
        if (!_borrowedBooks.TryGetValue(userId, out var list)) return;
        list.Remove(bookId);
    }

    public List<int> GetBorrowedBookIds(int userId)
    {
        return _borrowedBooks.TryGetValue(userId, out var list) ? list : new List<int>();
    }





    //  helpers for privat d

    private List<Book> LoadBooks()
    {
        var books = new List<Book>();
        if (!File.Exists(_booksPath)) return books;

        foreach (var line in File.ReadAllLines(_booksPath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(',', 3);
            if (parts.Length < 3) continue;
            if (!int.TryParse(parts[0].Trim(), out var id)) continue;

            books.Add(new Book
            {
                Id     = id,
                Title  = parts[1].Trim(),
                Author = parts[2].Trim()
            });
        }

        return books;
    }



    private List<User> LoadUsers()
    {
        var users = new List<User>();
        if (!File.Exists(_usersPath)) return users;

        foreach (var line in File.ReadAllLines(_usersPath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(',', 2);
            if (parts.Length < 2) continue;
            if (!int.TryParse(parts[0].Trim(), out var id)) continue;

            users.Add(new User
            {
                Id   = id,
                Name = parts[1].Trim()
            });
        }

        return users;
    }

    private void SaveBooks()
    {
        var lines = _books.Select(b => $"{b.Id},{b.Title},{b.Author}");
        File.WriteAllLines(_booksPath, lines);
    }

    private void SaveUsers()
    {
        var lines = _users.Select(u => $"{u.Id},{u.Name}");
        File.WriteAllLines(_usersPath, lines);
    }
}
