using System.Security.Claims;
using BusProyectApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

public class BaseController : ControllerBase {
    private readonly ApplicationDBContext _context;
    public BaseController(ApplicationDBContext context) {
        _context = context;
    }

    protected async Task<bool> isAdmin() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return false;

        var currentUser = await _context.users.FindAsync(int.Parse(userId));
        return currentUser?.IsAdmin ?? false;
    }
}