using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MusicApplication.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [ValidateNever]
        public ICollection<Song> Songs { get; set; } = [];
    }
}
