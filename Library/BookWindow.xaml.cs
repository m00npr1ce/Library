using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        private LibraryContext _context;
        private Book _book;
        private bool _isEditMode;

        public BookWindow(LibraryContext context, Book? book = null)
        {
            InitializeComponent();
            _context = context;
            _isEditMode = book != null;
            _book = book ?? new Book();

            DataContext = _book;
            LoadData();            
        }

        private void LoadData()
        {
            AuthorComboBox.ItemsSource = _context.Authors.ToList();
            AuthorComboBox.DisplayMemberPath = "FullName";
            AuthorComboBox.SelectedValuePath = "Id";

            GenreComboBox.ItemsSource = _context.Genres.ToList();
            GenreComboBox.DisplayMemberPath = "Name";
            GenreComboBox.SelectedValuePath = "Id";

            if (_isEditMode)
            {
                TitleTextBox.Text = _book.Title;
                AuthorComboBox.SelectedValue = _book.Id;                
                GenreComboBox.SelectedValue = _book.Id;
                YearTextBox.Text = _book.PublishYear.ToString();
                ISBNTextBox.Text = _book.ISBN;
                QuantityTextBox.Text = _book.QuantityInStock.ToString();

            }
        }   
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите название книги", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AuthorComboBox.SelectedValue == null || GenreComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите автора и жанр", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _book.Author = (int)AuthorComboBox.SelectedValue;
            _book.Genre = (int)GenreComboBox.SelectedValue;
            _book.Title = TitleTextBox.Text;
            _book.ISBN = ISBNTextBox.Text;
            _book.PublishYear = int.TryParse(YearTextBox.Text, out int year) ? year : null;
            _book.QuantityInStock = int.TryParse(QuantityTextBox.Text, out int quantity) ? quantity : 0;
            
            if (_isEditMode)
            {
                _context.Books.Update(_book);
            }
            else
            {
                _context.Books.Add(_book);
            }
            
            _context.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
