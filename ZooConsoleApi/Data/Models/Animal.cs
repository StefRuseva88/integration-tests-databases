using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ZooConsoleAPI.Common;

namespace ZooConsoleAPI.Data.Model
{
    public class Animal
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.CatalogNumberFormat)]
        public string CatalogNumber { get; set; }

        [Required]
        [MaxLength(ValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstants.BreedMaxLength)]
        public string Breed { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TypeMaxLength)]
        public string Type { get; set; }

        [Required]
        [Range(ValidationConstants.AgeMinValue, ValidationConstants.AgeMaxValue)]
        public int Age { get; set; }

        [Required]
        [MaxLength(ValidationConstants.GenderMaxLength)]
        public string Gender { get; set; }

        [Required]
        public bool IsHealthy { get; set; }
    }
}


