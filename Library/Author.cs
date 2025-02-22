using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library;

public partial class Author
{
    public int Id { get; set; }

    [Required] // Делаем обязательным, чтобы избежать null в коде
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public DateTime? BirthDate { get; set; } // Разрешаем null для необязательной даты

    [Required]
    [MaxLength(50)]
    public string Country { get; set; } = string.Empty;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
