using System;
using System.Linq;
using System.Windows;

namespace Library
{
    public partial class ManageAuthorsWindow : Window
    {
        private LibraryContext _context;

        public ManageAuthorsWindow()
        {
            InitializeComponent();
            _context = new LibraryContext(); // Инициализация контекста базы данных
            LoadAuthors(); // Загрузка данных о авторах
        }

        // Метод для загрузки авторов в DataGrid
        private void LoadAuthors()
        {
            var authors = _context.Authors.ToList(); // Получаем всех авторов из базы
            AuthorsDataGrid.ItemsSource = authors; // Привязываем данные к DataGrid
        }

        // Обработчик кнопки добавления автора
        private void AddAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var AuthorWindow = new AuthorWindow(_context); // Открываем окно добавления автора
            if (AuthorWindow.ShowDialog() == true) // Показываем окно как модальное
            {
                LoadAuthors(); // Перезагружаем список авторов
            }     
            
        }

        // Обработчик кнопки редактирования автора
        private void EditAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
            {
                var authorWindow = new AuthorWindow(_context, selectedAuthor);
                if (authorWindow.ShowDialog() == true)
                {
                    LoadAuthors(); // Обновляем данные после редактирования
                }
            }
        }

        // Обработчик кнопки удаления автора
        private void DeleteAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author selectedAuthor)
            {
                _context.Authors.Remove(selectedAuthor);
                _context.SaveChanges();
                LoadAuthors();  // Обновляем список книг после удаления
            }
        }
        
    }
}
