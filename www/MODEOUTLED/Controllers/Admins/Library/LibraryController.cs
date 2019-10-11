using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
using PagedList;

namespace onsoft.Controllers.Admins.Library
{
    public class LibraryController : BaseController
    {
        //
        // GET: /Library/
        ModeoutleddbContext db = new ModeoutleddbContext();
        #region[NewsIndexot]
        public ActionResult LibraryIndexot(string sortOrder, string sortName, string sortGroup, int? page, string currentNewsCodeFilter, string currentLibraryNameFilter, string Libraryname, string currentGroupNewsFilter, string GroupNews, string currentNewsAmount)
        {

            if (Request.HttpMethod == "GET")
            {
                Libraryname = currentLibraryNameFilter;
                GroupNews = currentGroupNewsFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentNewsNameFilter = Libraryname;
            ViewBag.currentGroupNewsFilter = GroupNews;

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
            if (!String.IsNullOrEmpty(GroupNews))
            {
                int groupid = Int32.Parse(GroupNews);
                all = all.Where(p => p.GroupLibraryID == groupid).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["Namesearch"] != null)
            {
                all = all.Where(p => p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["GroupNews"] != null)
            {
                if (Session["Namesearch"] != null)
                {
                    all = all.Where(p => p.GroupLibraryID == int.Parse(Session["GroupNews"].ToString()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                else
                {
                    all = all.Where(p => p.GroupLibraryID == int.Parse(Session["GroupNews"].ToString())).OrderByDescending(p => p.Id).ToList();
                }
                Session["GroupNews"] = null;
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
        #region[NewsCreateot]
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
        #region[NewsCreate]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LibraryCreate(FormCollection collection, News news, HttpPostedFileBase fileImg)
        {
            int idmem = 0;
            if (Session["uId"] != null) { idmem = int.Parse(Session["uId"].ToString()); }
            if (Request.Cookies["Username"] != null)
            {
                news.Name = collection["Name"];
                news.Tag = StringClass.NameToTag(collection["Name"]);
                news.Image = collection["Image"];
                news.Content = collection["Content"];
                news.Detail = collection["Detail"];
                int GroupNews = Convert.ToInt32(collection["GroupNews"]);
                news.IdGroup = GroupNews;
                news.Keyword = collection["Keyword"];
                news.Description = collection["Description"];
                news.Title = collection["Title"];
                news.Ord = Convert.ToInt32(collection["Ord"]);
                news.Index = (collection["Index"] == "false") ? false : true;
                news.Active = (collection["Active"] == "false") ? false : true;
                news.Lang = Session["Lang"] != null ? Session["Lang"].ToString() : "vi";
                news.View = 0;//so luot xem
                news.SDate = DateTime.Now;
                db.sp_News_Insert(news.Name, news.Tag, news.Title, news.Keyword, news.Description, news.Image, news.SDate, news.Content, news.Detail, news.Index, news.View, news.IdGroup, news.Ord, news.Active, news.Lang);
                db.SaveChanges();
                return RedirectToAction("LibraryIndexot");
            }
            else
            {
                return Redirect("/Admins/admins");
            }
        }
        #endregion

    }
}
