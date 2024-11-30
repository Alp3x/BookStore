using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Category Number")]
        [Range(1,100,ErrorMessage ="Numero entre 1-100")]
        public int DisplayOrder { get; set; }
    }
}