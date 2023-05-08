namespace API.DTOs
{
        public class FilterDto
        {
            public DateTime Date { get; set; }
            public string? Year { get; set; }
            public string? Month { get; set; }
            public string? Day { get; set; }
            public int PageSize { get; set; }
            public int PageNumber { get; set; }
    }
}
