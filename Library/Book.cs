using System;
using System.Collections.Generic;

namespace Library;

public partial class Book
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int Author { get; set; }

    public int? PublishYear { get; set; }

    public string? Isbn { get; set; }

    public int Genre { get; set; }

    public int? QuantityInStock { get; set; }

    public virtual Author AuthorNavigation { get; set; } = null!;

    public virtual Genre GenreNavigation { get; set; } = null!;
}
