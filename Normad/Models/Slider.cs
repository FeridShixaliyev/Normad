using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Normad.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string Level { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
