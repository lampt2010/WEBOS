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

            var all = db.GroupLibraries.ToList();

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
                int parentId = 0;
                string level = "00000";
                
               
                catego.Name = collection["Name"];
                catego.Code = collection["Code"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
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
                var catego = db.GroupNews.Find(id);
                // Lấy dữ liệu từ view
                string name = collection["Name"];
                
                catego.Name = name;
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.Lang = Session["Lang"] != null ? Session["Lang"].ToString() : "vi";
                string groid = collection["Cat"];
                if (groid != null && groid != "")
                {
                    catego.GrpID = int.Parse(groid);
                }
                else
                {
                    catego.GrpID = 0;
                }

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


    }
}
