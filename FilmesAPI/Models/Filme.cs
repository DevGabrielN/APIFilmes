using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(50, ErrorMessage = "Max length is {1} caracteres")]   
    public string Titulo { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(50, ErrorMessage = "Max length is {1} caracteres")]
    public string Genero { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    [Range(70, 600, ErrorMessage = "Duration must be between {1} and {2} minutes")]
    public int Duracao { get; set; }
}
