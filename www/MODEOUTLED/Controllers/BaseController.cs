using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace onsoft.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["Lang"] != null)
            {
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
               // Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["Lang"].ToString());
            }
            else
            {
             //   Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
             //   Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
                Session["Lang"] = "vi";
            }
        }
        [HttpPost]
        public ActionResult ChangeCulture(string ddlCulture, string returnUrl)
        {
           // Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
           // Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);
            Session["Lang"] = ddlCulture;
            return Json(new{url=returnUrl}, JsonRequestBehavior.AllowGet);
        }


    }
}
