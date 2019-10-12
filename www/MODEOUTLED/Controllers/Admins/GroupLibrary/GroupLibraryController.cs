using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;

namespace onsoft.Controllers.Admins.GroupLibrary
{
    public class GroupLibraryController : BaseController
    {
        ModeoutleddbContext db = new ModeoutleddbContext();
        //
        // GET: /GroupLibrary/

        #region[GroupNewIndexot]
        public ActionResult GroupLibraryIndexot(int? page)
        {
            string Lang = Session["Lang"].ToString();

            var all = db.GroupLibraries.Where(x=>x.Lang==Lang).ToList();

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

            PagedList<Models.GroupLibrary> GroupLibrary = (PagedList<Models.GroupLibrary>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_PartialGroupLibraryIndex", GroupLibrary);

            return View(GroupLibrary);

        }
        #endregion
        #region[GroupNewCreateot]
        public ActionResult GroupLibraryCreateot()
        {
            return View();
        }
        #endregion
        #region[GroupNewCreateot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupLibraryCreateot(FormCollection collection, Models.GroupLibrary catego)
        {
            if (Request.Cookies["Username"] != null)
            {
                // Lấy dữ liệu từ view
                catego.Name = collection["Name"];
                catego.Code = collection["Code"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Image = collection["Image"];
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.Lang = Session["Lang"] != null ? Session["Lang"].ToString() : "vi";
                db.Entry(catego).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("GroupLibraryIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion


        #region[GroupNewEdit]
        public ActionResult GroupLibraryEdit(int id)
        {
            var Edit = db.GroupLibraries.First(m => m.Id == id);
            return View(Edit);
        }
        #endregion

        #region[GroupNewEditot]
        public ActionResult GroupLibraryEditot(int id)
        {
            var Edit = db.GroupLibraries.First(m => m.Id == id);

            return View(Edit);
        }
        #endregion

        #region[GroupNewEditot]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GroupLibraryEditot(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var catego = db.GroupLibraries.Find(id);
                // Lấy dữ liệu từ view
                string name = collection["Name"];
                
                catego.Name = name;
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Image = collection["Image"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.Lang = Session["Lang"] != null ? Session["Lang"].ToString() : "vi";
                db.Entry(catego).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GroupLibraryIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[GroupNewDelete]
        public ActionResult GroupNewDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from categaa in db.GroupNews where categaa.Id == id select categaa).Single();
                db.GroupNews.Remove(del);
                db.SaveChanges();

                List<GroupNew> GroupNews = db.GroupNews.ToList();

                if ((GroupNews.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("GroupLibraryIndexot");
                    }
                }

                return RedirectToAction("GroupLibraryIndexot", new { page = page });
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

            List<Models.GroupLibrary> GroupLibs = db.GroupLibraries.ToList();
            int lastpage = GroupLibs.Count / pagesize;
            if (GroupLibs.Count % pagesize > 0)
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
                                var Del = (from del in db.GroupLibraries where del.Id == id select del).SingleOrDefault();
                                //if (Del.SpTon == 0)
                                //{
                                db.GroupLibraries.Remove(Del);
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
                            return RedirectToAction("GroupLibraryIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("GroupLibraryIndexot", new { page = m });
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.GroupLibraries.Where(e => e.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Ord = int.Parse(collect["Ord" + id]);
                                }

                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }
                    return RedirectToAction("GroupLibraryIndexot", new { page = m });
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
            var news = db.GroupLibraries.Find(id);
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
        public ActionResult ChangeGroupLibrary(int id, string name, int? ord, string code)
        {
            var results = "";
            var lib = db.GroupLibraries.Find(id);
            if (lib != null)
            {
                if (name != null)
                {
                    lib.Name = name;
                    lib.Title = StringClass.NameToTag(name);
                    results = "Tên đã được thay đổi.";
                }
                if (code != null)
                {
                    lib.Code = code;
                    results = "Mã đã được thay đổi.";
                }
                if (ord != null)
                {
                    lib.Ord = ord;
                    results = "Thứ tự đã được thay đổi.";
                }
            }
            db.Entry(lib).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
    }
}
