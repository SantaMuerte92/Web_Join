using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Join.Models;

namespace Web_Join.Views.Home;

public class IndexModel : PageModel
{
    private readonly BigTicketSystemContext _ctx;
    public IndexModel(BigTicketSystemContext ctx) => _ctx = ctx;

    // Neues Ticket
    [BindProperty] public int NewTicketUserID { get; set; }
    [BindProperty] public int NewTicketStatusID { get; set; }
    [BindProperty] public int NewTicketPriorityID { get; set; }
    [BindProperty] public int NewTicketCategoryID { get; set; }
    [BindProperty] public string NewTicketTitle { get; set; } = string.Empty;
    [BindProperty] public string? NewTicketDescription { get; set; }

    public void OnGet()
    {
        // Kein weiteres Laden nötig (Index zeigt keine Liste mehr)
    }

    public async Task<IActionResult> OnPostCreateTicketAsync()
    {
        // Einfache Validierung
        if (string.IsNullOrWhiteSpace(NewTicketTitle))
        {
            ModelState.AddModelError(nameof(NewTicketTitle), "Titel erforderlich.");
            return Page();
        }

        var ticket = new Ticket
        {
            UserID = NewTicketUserID,
            StatusID = NewTicketStatusID,
            PriorityID = NewTicketPriorityID,
            CategoryID = NewTicketCategoryID,
            Title = NewTicketTitle,
            Description = NewTicketDescription,
            Createdate = DateTime.UtcNow
        };
        _ctx.Tickets.Add(ticket);
        await _ctx.SaveChangesAsync();
        // Nach Anlage Formular schließen -> Redirect
        return RedirectToPage();
    }
}