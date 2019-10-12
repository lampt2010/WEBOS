using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using onsoft.Models;
using PagedList;

namespace onsoft.Controllers.Admins.Library
{
    public class LibraryController : BaseController
    {
        //
        // GET: /Library/
        ModeoutleddbContext db = new ModeoutleddbContext();
        #region[LibraryIndexot]
        public ActionResult LibraryIndexot(string sortOrder, string sortName, string sortGroup, int? page, string currentNewsCodeFilter, string currentLibraryNameFilter, string Libraryname, string currentGroupLibraryFilter, string GroupLibrary, string currentNewsAmount)
        {

            if (Request.HttpMethod == "GET")
            {
                Libraryname = currentLibraryNameFilter;
                GroupLibrary = currentGroupLibraryFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentNewsNameFilter = Libraryname;
            ViewBag.currentGroupLibraryFilter = GroupLibrary;

            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

            // Sort Name (Truyền sortName khi phân trang)
            if (sortName != "")
            {
                ViewBag.CurrentSortName = sortName;
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            }
            // Sort Name (Truyền sortName khi sắp xếp)
            if (Session["sortName"] != null)
            {
                sortName = Session["sortName"].ToString();
                ViewBag.CurrentSortName = Session["sortName"].ToString();
                Session["sortName"] = null;
            }
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

            ViewBag.CurrentSortGroup = sortGroup;
            ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";
            string Lang = Session["Lang"].ToString();
            var all = db.Libraries.Where(x=>x.Lang==Lang).OrderByDescending(p => p.Ord).ToList();

            #region[Tìm kiếm]
            // Tìm theo tên Tin tức
            if (!String.IsNullOrEmpty(Libraryname))
            {
                if (Libraryname != "Tiêu đề thư viện")
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(Libraryname.ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
            }

            // Tìm theo nhóm Tin tức
            if (!String.IsNullOrEmpty(GroupLibrary))
            {
                int groupid = Int32.Parse(GroupLibrary);
                all = all.Where(p => p.GroupLibraryID == groupid).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["Namesearch"] != null)
            {
                all = all.Where(p => p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["GroupLibrary"] != null)
            {
                if (Session["Namesearch"] != null)
                {
                    all = all.Where(p => p.GroupLibraryID == int.Parse(Session["GroupLibrary"].ToString()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                else
                {
                    all = all.Where(p => p.GroupLibraryID == int.Parse(Session["GroupLibrary"].ToString())).OrderByDescending(p => p.Id).ToList();
                }
                Session["GroupLibrary"] = null;
            }
            if (Session["Namesearch"] != null) { Session["Namesearch"] = null; }
            #endregion

            #region[Sắp xếp]
            switch (sortOrder)
            {
                case "ord desc":
                    all = all.OrderByDescending(p => p.Ord).ToList();
                    break;
                case "ord asc":
                    all = all.OrderBy(p => p.Ord).ToList();
                    break;
                default:
                    break;
            }

            switch (sortName)
            {
                case "name desc":
                    all = all.OrderByDescending(p => p.Name).ToList();
                    break;
                case "name asc":
                    all = all.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    break;
            }
            #endregion

            if (Session["DeletePro"] != null)
            {
                var a = Session["DeletePro"].ToString();
                ViewBag.DelErr = "<p class='require'>" + a + "</p>";
                Session["DeletePro"] = null;
            }

            int pageSize = 20;
            if (currentNewsAmount != null && currentNewsAmount != "")
            {
                pageSize = Convert.ToInt32(currentNewsAmount);
            }
            int pageNumber = (page ?? 1);

            #region[Get Last Page]
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
            #endregion

            #region[view drop search]
            var cat = db.GroupLibraries.Where(n => n.Active == true && n.Lang==Lang).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.GroupLibrary = new SelectList(cat, "Id", "Name");
            }

            if (cat.Count == 0)
            {
                ViewBag.GroupLibrary = new SelectList(cat, "Id", "Name");
            }
            #endregion

            #region[Số Tin tức hiển thị trên trang]
            // Số Tin tức hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 10; i <= 100; i += 10)
            {
                if (i == pageSize)
                {
                    items.Add(new SelectListItem { Text = i + " Thư viện / trang", Value = i + "", Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = i + " Thư viện / trang", Value = i + "" });
                }
            }
            ViewBag.ddlLibraryAmount = items;
            #endregion

            ViewBag.TotalNews = db.Libraries.Where(x=>x.Lang==Lang).Count();

            // Phân trang
            PagedList<Models.Library> Newss = (PagedList<Models.Library>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_PartialLibraryIndex", Newss);

            return View(Newss);
        }
        #endregion
        #region[LibraryCreateot]
        public ActionResult LibraryCreateot()
        {

            string chuoi = "";
            string Lang = Session["Lang"].ToString();
            var cat = db.GroupLibraries.Where(n => n.Active == true && n.Lang == Lang).ToList();
            if (cat.Count > 0)
            {
                for (int i = 0; i < cat.Count; i++)
                {
                    ViewBag.GroupLibrary = new SelectList(cat, "Id", "Name");
                }
            }
            else
            {
                ViewBag.GroupLibrary = new SelectList(cat, "Id", "Name");
            }

            return View();
        }
        #endregion
        #region[LibraryCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LibraryCreate(FormCollection collection, Models.Library news, HttpPostedFileBase fileImg)
        {
            int idmem = 0;
            if (Session["uId"] != null) { idmem = int.Parse(Session["uId"].ToString()); }
            if (Request.Cookies["Username"] != null)
            {
               
                news.Name = collection["Name"];
                news.Image = collection["Image"];
                int GroupLibrary = Convert.ToInt32(collection["GroupLibrary"]);
                news.GroupLibraryID = GroupLibrary;
                news.Keyword = collection["Keyword"];
                news.Description = collection["Description"];
                news.Title = collection["Title"];
                news.Ord = Convert.ToInt32(collection["Ord"]);
                news.Action = (collection["Action"] == "false") ? false : true;
                news.Lang = Session["Lang"] != null ? Session["Lang"].ToString() : "vi";
                news.Views = 0;//so luot xem
                news.CreateDate = DateTime.Now;
                db.Entry(news).State = EntityState.Added;
                //db.sp_News_Insert(news.Name, news.Tag, news.Title, news.Keyword, news.Description, news.Image, news.SDate, news.Content, news.Detail, news.Index, news.View, news.IdGroup, news.Ord, news.Active, news.Lang);
                db.SaveChanges();
                return RedirectToAction("LibraryIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

        #region[LibraryEditot]
        public ActionResult LibraryEditot(int id)
        {
            string chuoi = "";
            string Lang = Session["Lang"].ToString();
            var Edit = db.Libraries.First(m => m.Id == id);
            var cat = db.GroupLibraries.Where(n => n.Active == true && n.Lang == Lang).ToList();
            ViewBag.GroupLibrary = new SelectList(cat, "Id", "Name", Edit.GroupLibraryID);
            return View(Edit);
        }
        #endregion
        #region[LibraryEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LibraryEdit(int id, FormCollection collection, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                var lib = db.Libraries.Find(id);
                lib.Name = collection["Name"];
                lib.Title = collection["Title"];
                lib.Image = collection["Image"];
                lib.Keyword = collection["Keyword"];
                lib.Description = collection["Description"];
                int IdGroup = Convert.ToInt32(collection["GroupLibrary"]);
                lib.GroupLibraryID = IdGroup;
                lib.Ord = Convert.ToInt32(collection["Ord"]);
                lib.Action = (collection["Action"] == "false") ? false : true;
                if (lib != null)
                {
                    db.Entry(lib).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("LibraryIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[LibraryDelete]
        public ActionResult LibraryDelete(int id, int page, int pagesize)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = db.Libraries.First(p => p.Id == id);
                db.Libraries.Remove(del);
                db.SaveChanges();
                List<Models.Library> news = db.Libraries.ToList();
                if ((news.Count % pagesize) == 0)
                {
                    if (page > 1)
                    {
                        page--;
                    }
                    else
                    {
                        return RedirectToAction("LibraryIndexot");
                    }
                }

                return RedirectToAction("LibraryIndexot", new { page = page });
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        #region[MultiCommand]
        public ActionResult MultiCommand(FormCollection collect)
        {
            string Lang = Session["Lang"].ToString();
            int m = int.Parse(collect["mPage"]);
            int pagesize = int.Parse(collect["PageSize"]);

            List<Models.Library> news = db.Libraries.Where(x=>x.Lang==Lang).ToList();
            int lastpage = news.Count / pagesize;
            if (news.Count % pagesize > 0)
            {
                lastpage++;
            }
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
                            if (key.Contains("chkIndex") || key.Contains("chkActive"))
                            {

                            }
                            else
                            {
                                checkbox = Request.Form["" + key];
                                if (checkbox != "false")
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 3));
                                    var Del = (from del in db.Libraries where del.Id == id select del).SingleOrDefault();
                                    db.Libraries.Remove(Del);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("LibraryIndexot");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("LibraryIndexot", new { page = m });
                }
                else if (collect["ddlLibraryAmount"] != null)
                {
                    if (collect["ddlLibraryAmount"].Length > 0)
                    {
                        Session["ddlLibraryAmount"] = collect["ddlLibraryAmount"];
                    }
                    return RedirectToAction("LibraryIndexot");
                }

                else
                {
                    string Namesearch = collect["LibraryName"];
                    string cat = collect["GroupLibrary"];
                    if (Namesearch.Length > 0)
                    {
                        Session["Namesearch"] = Namesearch;
                    }
                    if (cat.Length > 0)
                    {
                        Session["GroupLibrary"] = cat;
                    }
                    return RedirectToAction("LibraryIndexot", new { page = m });
                }
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion
        // AJAX: /Product/ChangeIndex
        [HttpPost]
        public ActionResult ChangeLibrary(int id, string name, string title, string code)
        {
            var results = "";
            var lib = db.Libraries.Find(id);
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
                if (title != null)
                {
                    lib.Title = title;
                    results = "Tiêu đề đã được thay đổi.";
                }

            }
            db.Entry(lib).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
        [HttpPost]
        public JsonResult AddMultiImages(int id,string images)
        {
            var list = db.LibraryDetails.Where(x => x.LibraryID == id).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                db.LibraryDetails.Remove(list[i]);
                db.SaveChanges();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);
            for (int i = 0; i < listImages.Count; i++)
            {
                var dt = new LibraryDetail();
                dt.LibraryID = id;
                dt.Url = listImages[i];
                db.Entry(dt).State = EntityState.Added;
                db.SaveChanges();
            }
            return Json(new
            {
                status = true
            });
        }
        [HttpPost]
        public JsonResult LoadDetailLibraryById(int id)
        {
           
            var list = db.LibraryDetails.Where(x => x.LibraryID == id).ToList();
            var tb = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                tb.Add(list[i].Url);
            }
            return Json(new
            {
                data = tb
            });
        }


    }
}
