using Newtonsoft.Json;

namespace StoreSolution.Server.ViewModels.Store
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public decimal Discount { get; set; }

        public string? Comments { get; set; }
    }
}
