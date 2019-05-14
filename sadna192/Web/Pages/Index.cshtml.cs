using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        [BindProperty]
        public new User User { get; set; }

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<Customer> Customers { get; private set; }


        public async Task OnGetAsync()
        {
            Customers = await _db.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var contact = await _db.Customers.FindAsync(id);

            if (contact != null)
            {
                _db.Customers.Remove(contact);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage();
        }


        public IActionResult OnPostLogin()
        {
            bool succeed = false;
            try
            {
                succeed = _db.Login(User.Name, User.Password);
            }
            catch (Exception) { }
            if (succeed)
                return RedirectToPage("/About");
            else
            {
                return RedirectToPage("/index");
            }

        }

    }
}
