using Web_Join.Models.ViewModels;



namespace Web_Join.Models.ViewModels;

public class IndexViewModel
{
    public int NewTicketCategoryID { get; set; }
    public string? NewTicketTitle { get; set; }
    public string? NewTicketDescription { get; set; }

    public List<CategoryOption> Categories { get; set; } = new();
}

public class CategoryOption
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
