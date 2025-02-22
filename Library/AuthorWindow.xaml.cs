using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AuthorWindow.xaml
    /// </summary>
    public partial class AuthorWindow : Window
    {
        private LibraryContext _context;
        private Author _author;
        private bool _isEditMode;
        public AuthorWindow(LibraryContext context, Author? author = null)
        {
            InitializeComponent();
            _context = context;
            _isEditMode = author != null;
            _author = author ?? new Author();

            DataContext = _author;

            if (_isEditMode)
            {
                FirstNameTextBox.Text = _author.FirstName;
                LastNameTextBox.Text = _author.LastName;
                BirthDateTextBox.Text = _author.BirthDate.ToString();
                CountryTextBox.Text = _author.Country;

            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("Введите имя автора", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _author.FirstName = FirstNameTextBox.Text;
            _author.LastName = LastNameTextBox.Text;
            DateTime dateTime = DateTime.ParseExact(BirthDateTextBox.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            _author.BirthDate = dateTime;
            _author.Country = CountryTextBox.Text;

            if (_isEditMode)
            {
                _context.Authors.Update(_author);
            }
            else
            {
                _context.Authors.Add(_author);
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
