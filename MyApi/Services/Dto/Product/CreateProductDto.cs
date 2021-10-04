using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class CreateAddressDto
    {
        [Required]
        public string Image { get; set; }
        
        [Required]
        public string Brand { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required] 
        [Range(1, 1024, ErrorMessage = "Only positive number allowed")]
        public int Memory { get; set; }

        [Required]
        public double Price { get; set; }
        
        [Required]
        public string Color { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Amount { get; init; }
    }
}