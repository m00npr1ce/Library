using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Library
{
    public partial class MainWindow : Window
    {
        private LibraryContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();

            LoadBooks();
            LoadAuthors();
            LoadGenres();
        }

        // Загрузка данных для отображения в DataGrid
        private void LoadBooks()
        {
            var books = _context.Books
                .Include(b => b.AuthorNavigation)
                .Include(b => b.GenreNavigation)
                .ToList();

            BooksDataGrid.ItemsSource = books;
        }

        // Загрузка списка авторов для фильтра
        private void LoadAuthors()
        {
            var authors = _context.Authors.ToList();
            AuthorFilterComboBox.ItemsSource = authors;
            AuthorFilterComboBox.DisplayMemberPath = "FullName";
        }

        // Загрузка списка жанров для фильтра
        private void LoadGenres()
        {
            var genres = _context.Genres.ToList();
            GenreFilterComboBox.ItemsSource = genres;
            GenreFilterComboBox.DisplayMemberPath = "Name";
        }

        // Кнопка поиска
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var query = _context.Books.Include(b => b.AuthorNavigation) // Загружаем автора
                                       .Include(b => b.GenreNavigation) // Загружаем жанр
                                       .AsQueryable();

            // Фильтрация по автору
            if (AuthorFilterComboBox.SelectedItem is Author selectedAuthor)
            {
                query = query.Where(b => b.AuthorNavigation.Id == selectedAuthor.Id);
            }

            // Фильтрация по жанру
            if (GenreFilterComboBox.SelectedItem is Genre selectedGenre)
            {
                query = query.Where(b => b.GenreNavigation.Id == selectedGenre.Id);
            }

            // Поиск по названию (игнорируем регистр)
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                query = query.Where(b => b.Title.ToLower().Contains(SearchTextBox.Text.ToLower()));
            }

            // Принудительное обновление DataGrid
            BooksDataGrid.ItemsSource = null;
            BooksDataGrid.ItemsSource = query.ToList();
        }


        // Кнопка добавления книги
        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var bookWindow = new BookWindow(_context);
            if (bookWindow.ShowDialog() == true)
            {
                LoadBooks(); // Перезагружаем данные после добавления книги
            }
        }

        // Кнопка редактирования книги
        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                var bookWindow = new BookWindow(_context, selectedBook);
                if (bookWindow.ShowDialog() == true)
                {
                    LoadBooks(); // Обновляем данные после редактирования
                }
            }
        }

        // Кнопка удаления книги
        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                _context.Books.Remove(selectedBook);
                _context.SaveChanges();
                LoadBooks();  // Обновляем список книг после удаления
            }
        }

        // Кнопка управления авторами
        private void ManageAuthorsButton_Click(object sender, RoutedEventArgs e)
        {
            var authorsWindow = new ManageAuthorsWindow();
            authorsWindow.ShowDialog();
            LoadBooks();
        }

        // Кнопка управления жанрами
        private void ManageGenresButton_Click(object sender, RoutedEventArgs e)
        {
            var genresWindow = new ManageGenresWindow();
            genresWindow.ShowDialog();
            LoadBooks();
        }
    }
}
