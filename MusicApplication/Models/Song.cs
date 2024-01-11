using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicApplication.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Artist { get; set; }

        public int GenreId { get; set; }

        [ValidateNever]
        public Genre Genre { get; set; }
    }
}
