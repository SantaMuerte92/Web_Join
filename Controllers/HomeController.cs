using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Web_Join.Models;
using Web_Join.Models.ViewModels;
using Web_Join.Securitiy;

namespace Web_Join.Controllers;

public class HomeController : Controller
{
    
    private readonly BigTicketSystemContext _ctx;
    private readonly IPasswordHasher _hasher;

    public HomeController(BigTicketSystemContext ctx, IPasswordHasher hasher)
    {
        _ctx = ctx;
        _hasher = hasher;
    }


    // GET: /Home/Index
    public async Task<IActionResult> Index()
    {
        var vm = new IndexViewModel
        {
            Categories = await _ctx.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryOption { Id = c.CID, Name = c.Name })
                .ToListAsync()
        };
        return View(vm);
    }

    // POST: /Home/CreateTicket (öffentlicher Ticket-Ersteller: nur Kategorie, Titel, Beschreibung)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTicket(IndexViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.NewTicketTitle))
            ModelState.AddModelError(nameof(vm.NewTicketTitle), "Titel erforderlich.");
        if (string.IsNullOrWhiteSpace(vm.NewTicketDescription))
            ModelState.AddModelError(nameof(vm.NewTicketDescription), "Beschreibung erforderlich.");
        if (vm.NewTicketCategoryID <= 0)
            ModelState.AddModelError(nameof(vm.NewTicketCategoryID), "Kategorie wählen.");

        if (!ModelState.IsValid)
        {
            vm.Categories = await _ctx.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryOption { Id = c.CID, Name = c.Name })
                .ToListAsync();
            return View("Index", vm);
        }

        int creatorId = await ResolveCreatorUserIdAsync();
        int statusId = await _ctx.Statuses.Where(s => s.Title == "Neu").Select(s => s.SID).FirstOrDefaultAsync();
        if (statusId == 0) statusId = await _ctx.Statuses.Select(s => s.SID).FirstAsync();
        int priorityId = await _ctx.Priorities.Where(p => p.Title == "Normal").Select(p => p.PID).FirstOrDefaultAsync();
        if (priorityId == 0) priorityId = await _ctx.Priorities.Select(p => p.PID).FirstAsync();

        var ticket = new Ticket
        {
            UserID = creatorId,
            StatusID = statusId,
            PriorityID = priorityId,
            CategoryID = vm.NewTicketCategoryID,
            Title = vm.NewTicketTitle!,
            Description = vm.NewTicketDescription,
            Createdate = DateTime.UtcNow
        };
        _ctx.Tickets.Add(ticket);
        await _ctx.SaveChangesAsync();

        TempData["Message"] = "Ticket erfolgreich erstellt.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Home/Admin
    public async Task<IActionResult> Admin(bool showOnlyOpen = true)
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
        var vm = new AdminViewModel { IsAdmin = isAdmin, ShowOnlyOpen = showOnlyOpen };
        if (!isAdmin) return View(vm);

        await FillAdminListsAsync(vm);
        return View(vm);
    }

    // POST: /Home/AdminLogin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminLogin(AdminViewModel vm)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Name == vm.AdminUser);
        if (user != null && _hasher.Verify(vm.AdminPassword, user.PasswordHash ?? Array.Empty<byte>(), user.PasswordSalt ?? Array.Empty<byte>()))
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            return RedirectToAction(nameof(Admin));
        }
        vm.LoginError = "Ungültige Zugangsdaten.";
        return View("Admin", vm);
    }

    // POST: /Home/AdminLogout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AdminLogout()
    {
        HttpContext.Session.Remove("IsAdmin");
        return RedirectToAction(nameof(Admin));
    }

    // POST: /Home/AdminCreateTicket (vollständig)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminCreateTicket(AdminViewModel vm)
    {
        if (!IsAdmin()) return Unauthorized();

        if (vm.NewTicketUserID <= 0 || vm.NewTicketStatusID <= 0 || vm.NewTicketPriorityID <= 0 ||
            vm.NewTicketCategoryID <= 0 || string.IsNullOrWhiteSpace(vm.NewTicketTitle))
        {
            vm.LoginError = "Bitte alle Felder ausfüllen.";
            await FillAdminListsAsync(vm);
            return View("Admin", vm);
        }

        _ctx.Tickets.Add(new Ticket
        {
            UserID = vm.NewTicketUserID,
            StatusID = vm.NewTicketStatusID,
            PriorityID = vm.NewTicketPriorityID,
            CategoryID = vm.NewTicketCategoryID,
            Title = vm.NewTicketTitle!,
            Description = vm.NewTicketDescription,
            Createdate = DateTime.UtcNow
        });
        await _ctx.SaveChangesAsync();
        return RedirectToAction(nameof(Admin), new { showOnlyOpen = vm.ShowOnlyOpen });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(AdminViewModel vm)
    {
        if (!IsAdmin()) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(vm.CreateCategoryName))
        {
            _ctx.Categories.Add(new Category { Name = vm.CreateCategoryName! });
            await _ctx.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Admin), new { showOnlyOpen = vm.ShowOnlyOpen });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateStatus(AdminViewModel vm)
    {
        if (!IsAdmin()) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(vm.CreateStatusTitle) && !string.IsNullOrWhiteSpace(vm.CreateStatusDescription))
        {
            _ctx.Statuses.Add(new Status { Title = vm.CreateStatusTitle!, Description = vm.CreateStatusDescription! });
            await _ctx.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Admin), new { showOnlyOpen = vm.ShowOnlyOpen });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePriority(AdminViewModel vm)
    {
        if (!IsAdmin()) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(vm.CreatePriorityTitle) && !string.IsNullOrWhiteSpace(vm.CreatePriorityDescription))
        {
            _ctx.Priorities.Add(new Priority { Title = vm.CreatePriorityTitle!, Description = vm.CreatePriorityDescription! });
            await _ctx.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Admin), new { showOnlyOpen = vm.ShowOnlyOpen });
    }

    private bool IsAdmin() => HttpContext.Session.GetString("IsAdmin") == "true";

    private async Task<int> ResolveCreatorUserIdAsync()
    {
        var sessionValue = HttpContext.Session.GetString("CurrentUserId");
        if (int.TryParse(sessionValue, out var idFromSession))
            return idFromSession;
        if (int.TryParse(Request.Cookies["uid"], out var idFromCookie))
            return idFromCookie;

        var firstUserId = await _ctx.Users.OrderBy(u => u.UID).Select(u => u.UID).FirstOrDefaultAsync();
        if (firstUserId == 0)
            throw new InvalidOperationException("Kein Benutzer vorhanden.");
        return firstUserId;
    }

    private async Task FillAdminListsAsync(AdminViewModel vm)
    {
        vm.Users = await _ctx.Users.OrderBy(u => u.Name).ToListAsync();
        vm.Statuses = await _ctx.Statuses.OrderBy(s => s.Title).ToListAsync();
        vm.Priorities = await _ctx.Priorities.OrderBy(p => p.Title).ToListAsync();
        vm.Categories = await _ctx.Categories.OrderBy(c => c.Name).ToListAsync();

        var q = _ctx.Tickets
            .Include(t => t.User)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .Include(t => t.Category)
            .AsQueryable();

        if (vm.ShowOnlyOpen)
            q = q.Where(t => t.Status.Title != "Geschlossen" && t.Status.Title != "Gelöst");

        vm.Tickets = await q.OrderByDescending(t => t.Createdate).ToListAsync();
    }
}
