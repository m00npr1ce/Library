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
    /// Interaction logic for ManageGenresWindow.xaml
    /// </summary>
    public partial class ManageGenresWindow : Window
    {
        private LibraryContext _context;
        public ManageGenresWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadGenres();
        }

        private void LoadGenres()
        {
            var genres = _context.Genres.ToList();
            GenresDataGrid.ItemsSource = genres;
        }

        private void AddGenreButton_Click(object sender, RoutedEventArgs e)
        {
            var GenreWindow = new GenreWindow(_context);
            if (GenreWindow.ShowDialog() == true)
            {
                LoadGenres();
            }
        }

        private void EditGenreButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresDataGrid.SelectedItem is Genre selectedGenre)
            {
                var genreWindow = new GenreWindow(_context, selectedGenre);
                if (genreWindow.ShowDialog() == true)
                {
                    LoadGenres();
                }
            }
        }

        private void DeleteGenreButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresDataGrid.SelectedItem is Genre selectedGenre)
            {
                _context.Genres.Remove(selectedGenre);
                _context.SaveChanges();
                LoadGenres();
            }
        }
    }
}
