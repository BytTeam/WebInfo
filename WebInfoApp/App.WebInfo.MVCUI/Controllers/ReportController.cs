using App.WebInfo.DataAccess.Concrete.EntityFramework;
using App.WebInfo.Entities.Concrete;
using App.WebInfo.MVCUI.Helpers;
using App.WebInfo.MVCUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.WebInfo.MVCUI.Controllers
{
    public class ReportController : ControllerBase
    {

        public ReportViewModel Model { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportController(IHttpContextAccessor httpContextAccessor)
        {
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
            var sql = "select PersonalId,Adi,Soyadi,SahisNo,AileNo,AnaAdi,BabaAdi,IsState,IsDelete,LastModified,LastModifiedDate,PersonalImage,IsNewItem,AcikAdres from Personal Where IsDelete=0";
            var where = string.Empty;
            string isFiltered = _httpContextAccessor.HttpContext.Session.GetString("IsFiltered");

            List<Personal> personalList = new List<Personal>();


            if (isFiltered == "true")
            {


                var paramx = _httpContextAccessor.HttpContext.Session.GetObject<ReportViewModel>("param");
                var param = paramx.Filter;


                if (param.Cinsiyet.CinsiyeId != 1)
                {
                    where += String.Format(" and CinsiyetCinsiyeId={0}", param.Cinsiyet.CinsiyeId);
                }

                if (param.Din.DinId != 1)
                {
                    where += String.Format(" and DinId={0}", param.Din.DinId);
                }

                if (param.EgitimDurumu.EgitimDurumuId != 1)
                {
                    where += String.Format(" and EgitimDurumuId={0}", param.EgitimDurumu.EgitimDurumuId);
                }

                if (param.IslemYapan.IslemYapanId != 1)
                {
                    where += String.Format(" and IslemYapanId={0}", param.IslemYapan.IslemYapanId);
                }

                if (param.KanGrubu.KanGrubuId != 1)
                {
                    where += String.Format(" and KanGrubuId={0}", param.KanGrubu.KanGrubuId);
                }

                if (param.KayitDurumu.KayitDurumuId != 1)
                {
                    where += String.Format(" and KayitDurumuId={0}", param.KayitDurumu.KayitDurumuId);
                }

                if (param.MedeniDurumu.MedeniDurumuId != 1)
                {
                    where += String.Format(" and MedeniDurumuId={0}", param.MedeniDurumu.MedeniDurumuId);
                }

                if (param.SosyalYardimDurumu.SosyalYardimDurumuId != 1)
                {
                    where += String.Format(" and SosyalYardimDurumuId={0}", param.SosyalYardimDurumu.SosyalYardimDurumuId);
                }

                if (param.Uyruk.UyrukId != 1)
                {
                    where += String.Format(" and UyrukId={0}", param.Uyruk.UyrukId);
                }

                if (param.Koken.KokenId != 1)
                {
                    where += String.Format(" and KokenId={0}", param.Koken.KokenId);
                }

                if (param.SaglikDurumu.SaglikDurumuId != 1)
                {
                    where += String.Format(" and SaglikDurumuId={0}", param.SaglikDurumu.SaglikDurumuId);
                }

                if (param.IkametDurumu.IkametDurumuId != 1)
                {
                    where += String.Format(" and IkametDurumuId={0}", param.IkametDurumu.IkametDurumuId);
                }

                if (!string.IsNullOrEmpty(paramx.DogumTarihiBegin) && !string.IsNullOrEmpty(paramx.DogumTarihiEnd))
                {
                    var beginTarih = ConvertBetweenDate(paramx.DogumTarihiBegin);
                    var endTarih = ConvertBetweenDate(paramx.DogumTarihiEnd);
                    if (!string.IsNullOrEmpty(beginTarih) && !string.IsNullOrEmpty(endTarih))
                    {
                        where += String.Format(" and CAST(DogumTarihi as date) between '{0}' and '{1}'",
                            beginTarih, endTarih);
                    }
                }

                if (!string.IsNullOrEmpty(where))
                {
                    sql += where;
                }
            }
            var context = new WebInfoContext();
            var conn = context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = sql;
                    var reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Personal
                            {
                                PersonalId = reader.GetInt64(0),
                                Adi = reader.GetString(1),
                                Soyadi = reader.GetString(2),
                                SahisNo = reader.GetString(3),
                                AileNo = reader.GetString(4),
                                AnaAdi = reader.GetString(5),
                                BabaAdi = reader.GetString(6),
                                IsState = reader.GetBoolean(7)
                            };
                            personalList.Add(row);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                iTotalRecords = list.Count,
                sEcho = sEcho.ToString()
            };
            return Json(model);
        }

        private static string ConvertBetweenDate(string stringDate)
        {
            var convertDate = string.Empty;
            var splits = stringDate.Split('/');
            if (splits.Length == 3)
            {
                convertDate = string.Format("{0}-{1}-{2}", splits[2], splits[1], splits[0]);
            }
            return convertDate;
        }
    }
}
