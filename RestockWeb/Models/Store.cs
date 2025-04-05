namespace RestockWeb.Models
{
    public class Store
    {
        public int Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}