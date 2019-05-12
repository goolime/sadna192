using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Web.Pages
{
    public class AboutModel : PageModel
    {
        [ViewData]
        public string Title { get; } = "About";

        public string Message { get; set; }

        public void OnGet()
        {
            string directory = Environment.CurrentDirectory;
            Message = String.Format("Your directory is {0}.", directory);
        }
    }
}
