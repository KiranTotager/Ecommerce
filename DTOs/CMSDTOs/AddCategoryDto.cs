using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.DTOs.CMSDTOs
{
    public class AddCategoryDto
    {
        [Required]
        [SwaggerSchema("provide the name of the category here")]
        public string Name { get; set; }
        [Required]
        [SwaggerSchema("do you want to display it on home screen")]
        public bool DisplayHome { get; set; }
        [Required]
        [SwaggerSchema("provide the category url here")]
        public string CategoryUrl { get; set; }
        [Required]
        [SwaggerSchema("provide the title for the category")]
        public string Title { get; set; }
        [Required]
        [SwaggerSchema("provide the description for the category")]
        public string Description { get; set; }
        [Required]
        [SwaggerSchema("provide the meta description for the category")]
        public string MetaDesc { get; set; }
        [Required]
        [SwaggerSchema("provide the display order details here")]
        public int DisplayOrder { get; set; }   
    }
}
