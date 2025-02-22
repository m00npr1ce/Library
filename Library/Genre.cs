using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library;

public partial class Genre
{
    public int Id { get; set; }

    [Required] // Название жанра обязательно
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } // Описание может отсутствовать

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
