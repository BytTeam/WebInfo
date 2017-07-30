using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.WebInfo.Business.Abstract;
using App.WebInfo.DataAccess.Concrete.EntityFramework;
using App.WebInfo.Entities.Concrete;
using App.WebInfo.MVCUI.Helpers;
using App.WebInfo.MVCUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace App.WebInfo.MVCUI.Controllers
{
    public class ReportController : ControllerBase
    {
        private const string PersonalInculude = "Cinsiyet,Din,DogumYeri,EgitimDurumu,IkametDurumu,Il,Ilce,IslemYapan,KanGrubu,KayitDurumu,Koken,MedeniDurumu,SaglikDurumu,SosyalYardimDurumu,Uyruk";
        private readonly IMemoryCache _memoryCache;
        public ReportViewModel Model;
        private readonly IPersonalService _personalService;
        private readonly object _personalCacheKey = "PersonalList_CacheKey";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportController(IPersonalService personalService, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _personalService = personalService;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
            Model = Model ?? new ReportViewModel();
        }

        public IActionResult Index()
        {
            Model = new ReportViewModel();
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter(ReportViewModel model)
        {
            _httpContextAccessor.HttpContext.Session.SetString("IsFiltered", "true");
            _httpContextAccessor.HttpContext.Session.SetObject("param", model);
            return View("Index", model);
        }

        public string Buttons(long id, bool state)
        {
            string tmpl = "<div class=\"btn-group btn-relative\">" +
                          "<button class=\"btn btn-xs green dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\" aria-expanded=\"false\">" +
                          "İşlemler<i class=\"fa fa-angle-down\"></i>" +
                          "</button>" +
                          "<ul class=\"dropdown-menu\" role=\"menu\">" +
                          "<li>" +
                          "<a href=\"" + Url.Action("Detail", "Personal", new { Id = id }) + "\">" +
                          "<i class=\"icon-energy\"></i> Detay" +
                          "</a>" +
                          "</li>" +
                          "<li>" +
                          "<a href=\"" + Url.Action("Edit", "Personal", new { Id = id }) + "\">" +
                          "<i class=\"icon-docs\"></i> Düzenle" +
                          "</a>" +
                          "</li>" +
                          "<li>" +
                          "<a href=\"" + Url.Action("Delete", "Personal", new { Id = id }) + "\" data-callback=\"TableDatatablesManaged.reflesh()\" class=\"btn-delete\">" +
                          "<i class=\"icon-trash\"></i> Sil" +
                          "</a>" +
                          "</li>" +
                          "</ul>" +
                          "</div>";
            return tmpl;
        }

        public async Task<JsonResult> GetList(int iDisplayStart, int iDisplayLength, string sSearch, int iColumns, int iSortingCols, int iSortCol_0, string sSortDir_0, int sEcho)
        {
            var sql = "select * from Personal where isDelete==false";
            var where = string.Empty;
            string isFiltered = _httpContextAccessor.HttpContext.Session.GetString("IsFiltered");

            List<Personal> personalList;

            if (isFiltered == "true")
            {


                var paramx = _httpContextAccessor.HttpContext.Session.GetObject<ReportViewModel>("param");
                var param = paramx.Filter;


                if (param.Cinsiyet.CinsiyeId != -1)
                {
                    where += String.Format(" and CinsityeId={0}", param.Cinsiyet.CinsiyeId);
                }


                if (param.Din.DinId != -1)
                {
                    where += String.Format("& and DinId={0}", param.Din.DinId);
                }
                if (!string.IsNullOrEmpty(where))
                {
                    sql += where;
                }
            }

            using (WebInfoContext _context = new WebInfoContext())
            {
                personalList = _context.Personal.FromSql(sql).ToList();
            }
            
            var list = personalList;
            var filteredlist =
                list
                    .Select(x => new[] {
                        x.PersonalId.ToString(),
                        x.Adi,
                        x.Soyadi,
                        x.SahisNo,
                        x.AileNo,
                        x.AnaAdi,
                        x.BabaAdi,
                        Buttons(x.PersonalId,x.IsState)
                    }).Where(x => string.IsNullOrEmpty(sSearch) || x.Any(y => y.IndexOf(sSearch, StringComparison.CurrentCultureIgnoreCase) >= 0));

            var enumerable = filteredlist as string[][] ?? filteredlist.ToArray();
            filteredlist = sSortDir_0 == "desc" ? enumerable.OrderByDescending(x => (x[iSortCol_0])) : enumerable.OrderBy(x => (x[iSortCol_0]));

            filteredlist = filteredlist
                .Skip(iDisplayStart)
                .Take(iDisplayLength);

            var orderedlist = filteredlist;
            var model = new
            {
                aaData = orderedlist,
                iTotalDisplayRecords = enumerable.Length,
                iTotalRecords = list.Count(),
                sEcho = sEcho.ToString()
            };
            return Json(model);
        }


    }
}
