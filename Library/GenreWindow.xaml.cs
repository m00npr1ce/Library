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
    /// Interaction logic for GenreWindow.xaml
    /// </summary>
    public partial class GenreWindow : Window
    {
        private LibraryContext _context;
        private Genre _genre;
        private bool _isEditMode;
        public GenreWindow(LibraryContext context, Genre? genre = null)
        {
            InitializeComponent();
            _context = context;
            _isEditMode = genre != null;
            _genre = genre ?? new Genre();

            DataContext = _genre;

            if (_isEditMode)
            {
                NameTextBox.Text = _genre.Name;
                DescriptionTextBox.Text = _genre.Description;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введите имя автора", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _genre.Name = NameTextBox.Text;
            _genre.Description = DescriptionTextBox.Text;

            if (_isEditMode)
            {
                _context.Genres.Update(_genre);
            }
            else
            {
                _context.Genres.Add(_genre);
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
