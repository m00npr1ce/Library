using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public void DisplayData()
        {
            using var context = new LibraryContext();

            var books = context.Books
                                .Include(b => b.AuthorNavigation)
                                .Include(b => b.GenreNavigation)
                                .ToList();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Books in the Library:");
            foreach (var book in books)
            {
                sb.AppendLine($"Title: {book.Title}, ISBN: {book.Isbn}, Author: {book.AuthorNavigation.FirstName} {book.AuthorNavigation.LastName}, Genre: {book.GenreNavigation.Name}");
            }

            // Выводим в TextBox
            OutputTextBox.Text = sb.ToString();
        }

        public MainWindow()
        {
            InitializeComponent();

            // Добавляем данные в базу данных
            using (var context = new LibraryContext())
            {
                context.AddDataToDatabase();  // Добавление данных в базу
            }

            // Выводим данные на экран
            DisplayData();
        }
    }

}