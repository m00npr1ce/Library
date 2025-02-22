using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library;

public partial class Book
{
    public int Id { get; set; }

    [Required] // Название книги обязательно
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    public int Author { get; set; }

    public int? PublishYear { get; set; }

    [StringLength(20)] // ISBN имеет фиксированную длину
    public string? ISBN { get; set; }

    public int Genre { get; set; }

    public int? QuantityInStock { get; set; } = 0;

    public virtual Author AuthorNavigation { get; set; } = null!; // Теперь может быть null
    public virtual Genre GenreNavigation { get; set; } = null!; // Теперь может быть null
}
