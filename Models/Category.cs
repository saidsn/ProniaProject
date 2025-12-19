using System.ComponentModel.DataAnnotations;

namespace Front_To_Back_.Models
{
    public class Category : BaseEntity
    {
        //[Required(ErrorMessage ="Bos olamaz")]
        [MaxLength(30, ErrorMessage ="Agilli ol AUYE")]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
