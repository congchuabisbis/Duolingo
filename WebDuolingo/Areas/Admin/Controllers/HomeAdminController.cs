using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebDuolingo.Data;

namespace WebDuolingo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]

    public class HomeAdminController : Controller
    {
        private readonly WebDuolingoContext _context;
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
        
            return View();
        }
    }
}
