using onsoft.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace onsoft.Controllers.Admins.Lang
{
    public class LanguageController : Controller
    {
        //
        // GET: /Language/

        ModeoutleddbContext db = new ModeoutleddbContext();
        //
        // GET: /GroupLibrary/

        #region[GroupNewIndexot]
        public ActionResult LanguageIndexot(int? page)
        {
          

            var all = db.Languages.ToList();

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            // begin [get last page]
            if (page != null)
            {
                ViewBag.mPage = (int)page;
            }
            else
            {
                ViewBag.mPage = 1;
            }


            int lastPage = all.Count / pageSize;
            if (all.Count % pageSize > 0)
            {
                lastPage++;
            }
            ViewBag.LastPage = lastPage;

            ViewBag.PageSize = pageSize;
            //end [get last page]

            PagedList<Models.Language> Languages = (PagedList<Models.Language>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_PartialLanguageIndex", Languages);

            return View(Languages);

        }
        #endregion
        #region[LanguageCreateot]
        public ActionResult LanguageCreateot()
        {
            return View();
        }
        #endregion
        #region[LanguageCreateot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LanguageCreateot(FormCollection collection, Models.Language catego)
        {
            if (Request.Cookies["Username"] != null)
            {
                // Lấy dữ liệu từ view
                catego.Name = collection["Name"];
                catego.Code = collection["Code"];
                
                catego.Images = collection["Images"];
                catego.Active = (collection["Action"] == "false") ? false : true;
               
                db.Entry(catego).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("LanguageIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion


        #region[LanguageEdit]
        public ActionResult LanguageEdit(int id)
        {
            var Edit = db.Languages.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion

        #region[LanguageEditot]
        public ActionResult LanguageEditot(int id)
        {
            var Edit = db.Languages.First(m => m.Id == id);

            return View(Edit);
        }
        #endregion

        #region[LanguageEditot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LanguageEditot(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var catego = db.Languages.Find(id);
                // Lấy dữ liệu từ view
                string name = collection["Name"];

                catego.Name = name;
                catego.Images = collection["Images"];
                catego.Code = collection["Code"];
                catego.Active = (collection["Active"] == "false") ? false : true;
              
                db.Entry(catego).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LanguageIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[LanguageDelete]
        public ActionResult LanguageDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from categaa in db.GroupNews where categaa.Id == id select categaa).Single();
                db.GroupNews.Remove(del);
                db.SaveChanges();

                List<Language> Languages = db.Languages.ToList();

                if ((Languages.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("LanguageIndexot");
                    }
                }

                return RedirectToAction("LanguageIndexot", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect)
        {
            int m = int.Parse(collect["mPage"]);
            int pagesize = int.Parse(collect["PageSize"]);

            List<Models.Language> Languages = db.Languages.ToList();
            int lastpage = Languages.Count / pagesize;
            if (Languages.Count % pagesize > 0)
            {
                lastpage++;
            }
            //int lastPage = int.Parse(collect["LastPage"]);

            if (Request.Cookies["Username"] != null)
            {

                if (collect["btnDelete"] != null)
                {
                    //string str = "";
                    foreach (string key in Request.Form)
                    {
                        var checkbox = "";
                        if (key.Contains("chk"))
                        {
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                int id = Convert.ToInt32(key.Remove(0, 3));
                                var Del = (from del in db.Languages where del.Id == id select del).SingleOrDefault();
                                //if (Del.SpTon == 0)
                                //{
                                db.Languages.Remove(Del);
                                db.SaveChanges();
                                //}
                                //else
                                //{
                                //    str += Del.Name + ",";
                                //    Session["DeletePro"] = "Sản phẩm " + str + "  vẫn còn trong kho! Không được xóa!";
                                //}
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("LanguageIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("LanguageIndexot", new { page = m });
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.Languages.Where(e => e.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                   // Up.Ord = int.Parse(collect["Ord" + id]);
                                }

                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }
                    return RedirectToAction("LanguageIndexot", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion


        // AJAX: /Product/ChangeProduct

        // AJAX: /Product/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            var news = db.Languages.Find(id);
            if (news != null)
            {
                news.Active = news.Active == true ? false : true;
            }
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(results);
        }
        // AJAX: /Product/ChangeIndex
        [HttpPost]
        public ActionResult ChangeLanguage(int id, string name, int? ord, string code)
        {
            var results = "";
            var lib = db.Languages.Find(id);
            if (lib != null)
            {
                if (name != null)
                {
                    lib.Name = name;
                    results = "Tên đã được thay đổi.";
                }
                if (code != null)
                {
                    lib.Code = code;
                    results = "Mã đã được thay đổi.";
                }
              
            }
            db.Entry(lib).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }

    }
}
