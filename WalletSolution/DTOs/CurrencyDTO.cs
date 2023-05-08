namespace API.DTOs
{
    public class CurrencyDTO
    {
            public string Name { get; set; }
            public string CurrencyCode { get; set; }
            public bool IsDefault { get; set; }
            public IFormFile? Logo { get; set; }
    }
}
