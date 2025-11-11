using Web_Join.Models.ViewModels;

namespace Web_Join.Models.ViewModels;

public class AdminViewModel
{
    public bool IsAdmin { get; set; }
    public bool ShowOnlyOpen { get; set; } = true;

    public string? LoginError { get; set; }
    public string? AdminUser { get; set; }
    public string? AdminPassword { get; set; }

    // Ticket Listen & Stammdaten
    public List<Ticket> Tickets { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public List<Status> Statuses { get; set; } = new();
    public List<Priority> Priorities { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    // Neues Ticket (Admin)
    public int NewTicketUserID { get; set; }
    public int NewTicketStatusID { get; set; }
    public int NewTicketPriorityID { get; set; }
    public int NewTicketCategoryID { get; set; }
    public string? NewTicketTitle { get; set; }
    public string? NewTicketDescription { get; set; }

    // Stammdaten Eingaben
    public string? CreateCategoryName { get; set; }
    public string? CreateStatusTitle { get; set; }
    public string? CreateStatusDescription { get; set; }
    public string? CreatePriorityTitle { get; set; }
    public string? CreatePriorityDescription { get; set; }
}
