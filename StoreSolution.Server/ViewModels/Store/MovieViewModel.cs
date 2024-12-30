namespace StoreSolution.Server.ViewModels.Store
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Sinopse { get; set; }

        public decimal PricePerDay { get; set; }

        public int UnitsInStock { get; set; }
    }
}
