using Front_To_Back_.Models;

namespace Front_To_Back_.ViewModels
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
