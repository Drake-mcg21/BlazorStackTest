// Models/Movie.cs
using System.ComponentModel.DataAnnotations;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Genre is required")]
    [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters")]
    public string Genre { get; set; }

    [Required(ErrorMessage = "Release date is required")]
    public DateTime ReleaseDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Box office sales must be positive")]
    public decimal? BoxOfficeSales { get; set; }
}