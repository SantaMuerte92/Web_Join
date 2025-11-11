using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Pages;

public class AdminModel : PageModel
{
    private readonly BigTicketSystemContext _ctx;
    public AdminModel(BigTicketSystemContext ctx) => _ctx = ctx;

    public bool IsAdmin => HttpContext.Session.GetString("IsAdmin") == "true";

    public List<Ticket> Tickets { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public List<Status> Statuses { get; set; } = new();
    public List<Priority> Priorities { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public string? LoginError { get; set; }
    [BindProperty] public string AdminUser { get; set; } = string.Empty;
    [BindProperty] public string AdminPassword { get; set; } = string.Empty;

    // Neues Ticket
    [BindProperty] public int NewTicketUserID { get; set; }
    [BindProperty] public int NewTicketStatusID { get; set; }
    [BindProperty] public int NewTicketPriorityID { get; set; }
    [BindProperty] public int NewTicketCategoryID { get; set; }
    [BindProperty] public string NewTicketTitle { get; set; } = string.Empty;
    [BindProperty] public string? NewTicketDescription { get; set; }

    // Stammdaten
    [BindProperty] public string CreateCategoryName { get; set; } = string.Empty;
    [BindProperty] public string CreateStatusTitle { get; set; } = string.Empty;
    [BindProperty] public string CreateStatusDescription { get; set; } = string.Empty;
    [BindProperty] public string CreatePriorityTitle { get; set; } = string.Empty;
    [BindProperty] public string CreatePriorityDescription { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)] public bool ShowOnlyOpen { get; set; } = true;

    public async Task OnGetAsync()
    {
        if (IsAdmin)
            await LoadAsync();
    }

    public async Task<IActionResult> OnPostLoginAsync()
    {
        if (AdminUser == "Admin" && AdminPassword == "0000")
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            await LoadAsync();
        }
        else
        {
            LoginError = "Ungültige Zugangsdaten.";
        }
        return Page();
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Remove("IsAdmin");
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateTicketAsync()
    {
        if (!IsAdmin) return Unauthorized();
        if (string.IsNullOrWhiteSpace(NewTicketTitle))
        {
            ModelState.AddModelError(nameof(NewTicketTitle), "Titel erforderlich.");
            await LoadAsync();
            return Page();
        }

        _ctx.Tickets.Add(new Ticket
        {
            UserID = NewTicketUserID,
            StatusID = NewTicketStatusID,
            PriorityID = NewTicketPriorityID,
            CategoryID = NewTicketCategoryID,
            Title = NewTicketTitle,
            Description = NewTicketDescription,
            Createdate = DateTime.UtcNow
        });
        await _ctx.SaveChangesAsync();
        return RedirectToPage(new { ShowOnlyOpen });
    }

    public async Task<IActionResult> OnPostCreateCategoryAsync()
    {
        if (!IsAdmin) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(CreateCategoryName))
        {
            _ctx.Categories.Add(new Category { Name = CreateCategoryName });
            await _ctx.SaveChangesAsync();
        }
        return RedirectToPage(new { ShowOnlyOpen });
    }

    public async Task<IActionResult> OnPostCreateStatusAsync()
    {
        if (!IsAdmin) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(CreateStatusTitle) && !string.IsNullOrWhiteSpace(CreateStatusDescription))
        {
            _ctx.Statuses.Add(new Status { Title = CreateStatusTitle, Description = CreateStatusDescription });
            await _ctx.SaveChangesAsync();
        }
        return RedirectToPage(new { ShowOnlyOpen });
    }

    public async Task<IActionResult> OnPostCreatePriorityAsync()
    {
        if (!IsAdmin) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(CreatePriorityTitle) && !string.IsNullOrWhiteSpace(CreatePriorityDescription))
        {
            _ctx.Priorities.Add(new Priority { Title = CreatePriorityTitle, Description = CreatePriorityDescription });
            await _ctx.SaveChangesAsync();
        }
        return RedirectToPage(new { ShowOnlyOpen });
    }

    private async Task LoadAsync()
    {
        Users = await _ctx.Users.OrderBy(u => u.Name).ToListAsync();
        Statuses = await _ctx.Statuses.OrderBy(s => s.Title).ToListAsync();
        Priorities = await _ctx.Priorities.OrderBy(p => p.Title).ToListAsync();
        Categories = await _ctx.Categories.OrderBy(c => c.Name).ToListAsync();

        var query = _ctx.Tickets
            .Include(t => t.User)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .Include(t => t.Category)
            .AsQueryable();

        if (ShowOnlyOpen)
            query = query.Where(t => t.Status.Title != "Geschlossen" && t.Status.Title != "Gelöst");

        Tickets = await query
            .OrderByDescending(t => t.Createdate)
            .ToListAsync();
    }
}