using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loto.Handle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Loto.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGetPartialBot()
        {
            return new PartialViewResult { ViewName = "_BotImports" };
        }
        public IActionResult OnGetPartialHuman()
        {
            return new PartialViewResult { ViewName = "_HumanImports" };
        }

      

        public void OnGet()
        {

        }
    }
}
