using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace MODEOUTLED.Controllers
{
    public class ShoppingCartController : Controller
    {
        ModeoutleddbContext db = new ModeoutleddbContext();

        public ActionResult Cart()
        {
            return View();
        }

    }
}
