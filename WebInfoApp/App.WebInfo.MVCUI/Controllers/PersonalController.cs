using App.Core.Utilities;
using App.WebInfo.Business.Abstract;
using App.WebInfo.Entities.Concrete;
using App.WebInfo.MVCUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace App.WebInfo.MVCUI.Controllers
{
    [Authorize]
    public class PersonalController : ControllerBase
    {
        private const string PersonalInculude = "Cinsiyet,Din,DogumYeri,EgitimDurumu,IkametDurumu,Il,Ilce,IslemYapan,KanGrubu,KayitDurumu,Koken,MedeniDurumu,SaglikDurumu,SosyalYardimDurumu,Uyruk";
        private readonly IMemoryCache _memoryCache;
        private readonly IPersonalService _personal;
        private readonly IUtileService _utileService;
        private PersonalViewModel _model;
        private readonly IHostingEnvironment _environment;
        public sealed override IHttpContextAccessor HttpContextAccessor { get; set; }
        private readonly object _personalCacheKey = "PersonalList_CacheKey";

        public PersonalController(IPersonalService personal, IUtileService utileService, IMemoryCache memoryCache, IHostingEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _personal = personal;
            _utileService = utileService;
            _environment = environment;
            _memoryCache = memoryCache;
            HttpContextAccessor = httpContextAccessor;
            _model = new PersonalViewModel { Personal = new Personal() };
        }

        public IActionResult Index()
        {
            var model = new PersonalViewModel
            {
                Personal = new Personal(),
                SessionUser = GetLoginUser()
            };
            return View(model);
        }

        [Authorize(Roles = "Admin,Create")]
        public async Task<IActionResult> Create()
        {
            await Bind();
            _model.Personal = new Personal();
            _model.SessionUser = GetLoginUser();

            return View(_model);
        }

        public SelectList ConvertSelectList(IEnumerable<object> list)
        {
            return new SelectList(list, "Id", "Value");
        }

        public async Task<ActionResult> Edit(long? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Personal personal = await _personal.Get(x => x.PersonalId == id, PersonalInculude);

            if (personal == null)
            {
                return NotFound();
            }

            await Bind();
            _model.Personal = personal;

            return View("Create", _model);
        }

        public async Task<ActionResult> Detail(long? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Personal personal = await _personal.Get(x => x.PersonalId == id, PersonalInculude);

            if (personal == null)
            {
                return NotFound();
            }

            await Bind();
            _model.Personal = personal;

            return View(_model);
        }

        public IActionResult List()
        {
            var model = new PersonalViewModel
            {
                Personal = new Personal()
            };
            return View(model);
        }

        private async Task Bind()
        {
            var cacheKey = "Personal_cache_bind";
            PersonalViewModel cacheModel;
            if (!_memoryCache.TryGetValue(cacheKey, out cacheModel))
            {
                var cinsiyetTask = _utileService.GetCinsiyets();
                var dinTask = _utileService.GetDins();
                var dogumYeriTask = _utileService.GetDogumYeris();
                var egitimDurumuTask = _utileService.GetEgitimDurumus();
                var ikametDurumuTask = _utileService.GetIkametDurumus();
                var ilTask = _utileService.GetIls();
                var ilceTask = _utileService.GetIlces();
                var islemYapanTask = _utileService.GetIslemYapans();
                var kanGrubuTask = _utileService.GetKanGrubus();
                var uyrukTask = _utileService.GetUyruks();
                var saglikDurumuTask = _utileService.GetSaglikDurumus();
                var kangurubuTask = _utileService.GetKanGrubus();
                var sosyalYardimTask = _utileService.GetSosyalYardimDurumus();

                await Task.WhenAll(cinsiyetTask, dinTask, dogumYeriTask, egitimDurumuTask, ikametDurumuTask, ilTask, ilceTask, islemYapanTask, kanGrubuTask, uyrukTask, saglikDurumuTask, kangurubuTask, sosyalYardimTask);

                _model.CinsiyetList = ConvertSelectList(cinsiyetTask.Result.Select(x => new { Id = x.CinsiyeId, Value = x.CinsiyetName }));
                _model.DinList = ConvertSelectList(dinTask.Result.Select(x => new { Id = x.DinId, Value = x.DinName }));
                _model.DogumYeriList = ConvertSelectList(dogumYeriTask.Result.Select(x => new { Id = x.DogumYeriId, Value = x.DogumYeriName }));
                _model.EgitimDurumuList = ConvertSelectList(egitimDurumuTask.Result.Select(x => new { Id = x.EgitimDurumuId, Value = x.EgitimDurumuName }));
                _model.IkametDurumuList = ConvertSelectList(ikametDurumuTask.Result.Select(x => new { Id = x.IkametDurumuId, Value = x.IkametDurumuName }));
                _model.IlList = ConvertSelectList(ilTask.Result.Select(x => new { Id = x.IlId, Value = x.IlName }));
                _model.IlceList = ConvertSelectList(ilceTask.Result.Select(x => new { Id = x.IlceId, Value = x.IlceName }));
                _model.IslemYapanList = ConvertSelectList(islemYapanTask.Result.Select(x => new { Id = x.IslemYapanId, Value = x.IslemYapanName }));
                _model.KanGrubuList = ConvertSelectList(kanGrubuTask.Result.Select(x => new { Id = x.KanGrubuId, Value = x.KanGrubuName }));
                _model.UyrukList = ConvertSelectList(uyrukTask.Result.Select(x => new { Id = x.UyrukId, Value = x.UyrukName }));
                _model.SaglikDurumuList = ConvertSelectList(saglikDurumuTask.Result.Select(x => new { Id = x.SaglikDurumuId, Value = x.SaglikDurumuName }));
                _model.KanGrubuList = ConvertSelectList(kanGrubuTask.Result.Select(x => new { Id = x.KanGrubuId, Value = x.KanGrubuName }));
                _model.SosyalYardimDurumuList=ConvertSelectList(sosyalYardimTask.Result.Select(x=>new{Id=x.SosyalYardimDurumuId,Value=x.SosyalYardimDurumuName}));

                cacheModel = _model;

                var opts = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                };
                _memoryCache.Set(cacheKey, cacheModel, opts);
            }
            _model = cacheModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Create")]
        public async Task<IActionResult> Create(PersonalViewModel model, IFormFile personalImage)
        {
            try
            {
                await Bind();
                if (!ModelState.IsValid)
                {
                    _model.Personal = model.Personal;


                    return View(_model);
                }

                var fileName = FileUpload(personalImage);
                if (personalImage != null && !string.IsNullOrEmpty(fileName))
                {
                    model.Personal.PersonalImage = fileName;
                }
                if (model.Personal.PersonalId != 0)
                {
                    try
                    {
                        model.Personal.LastModified = GetLoginUser().UserName;
                    }
                    catch
                    {

                    }
                    model.Personal.LastModifiedDate = DateTime.Now;
                    model.Personal.IsNewItem = false;
                    var updatePersonal = _personal.Update(model.Personal);
                    await updatePersonal;
                    if (updatePersonal.IsCompleted)
                    {
                        alertUi.AlertUiType = updatePersonal.IsCompleted ? AlertUiType.success : AlertUiType.error;
                        CleaCache();
                    }
                }
                else
                {
                    model.Personal.CreationDate = DateTime.Now;
                    model.Personal.IsNewItem = true;
                    try
                    {
                        model.Personal.CreatedBy = GetLoginUser().UserName;
                    }
                    catch
                    {

                    }
                    var addPersonal = _personal.Add(model.Personal);
                    await addPersonal;
                    if (addPersonal.IsCompleted)
                    {
                        alertUi.AlertUiType = addPersonal.IsCompleted ? AlertUiType.success : AlertUiType.error;
                        CleaCache();
                    }
                   
                }

                AlertUiMessage();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return RedirectToAction("Index", model);
        }

        private string FileUpload(IFormFile imageFile)
        {
            var newFileName = string.Empty;

            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(_environment.WebRootPath, "uploads");
                path = Path.Combine(path, "personalImage");
                var fileName = imageFile.FileName;
                var imageType = fileName.Substring(fileName.LastIndexOf('.'),
                    fileName.Length - fileName.LastIndexOf('.'));
                newFileName = Guid.NewGuid() + "-" + imageType;
                string newFile = Path.Combine(path, newFileName);
                using (Stream fs = System.IO.File.Create(newFile))
                {
                    if (fs != null)
                    {
                        imageFile.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
            return "/uploads/personalImage/" + newFileName;
        }

        public string Buttons(long id, bool state)
        {
            string tmpl = "<div class=\"btn-group\">" +
                          "<button class=\"btn btn-xs green dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\" aria-expanded=\"false\">" +
                          "İşlemler<i class=\"fa fa-angle-down\"></i>" +
                          "</button>" +
                          "<ul class=\"dropdown-menu\" role=\"menu\">" +
                          "<li>" +
                          "<a href=\"" + Url.Action("Detail", "Personal", new { Id = id }) + "\">" +
                          "<i class=\"icon-docs\"></i> Detay" +
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

            List<Personal> cacheModel;

            if (!_memoryCache.TryGetValue(_personalCacheKey, out cacheModel))
            {
                var lists = _personal.GetList(x => !x.IsDelete);
                await lists;
                cacheModel = lists.Result;
                var opts = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memoryCache.Set(_personalCacheKey, cacheModel, opts);
            }
           

            List<Personal> list = cacheModel;

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

        public void CleaCache()
        {
            _memoryCache.Remove(_personalCacheKey);
        }

        public async Task<JsonResult> NewList(int iDisplayStart, int iDisplayLength, string sSearch, int iColumns, int iSortingCols, int iSortCol_0, string sSortDir_0, int sEcho)
        {

            object cacheKey = "PersonalNewList_CacheKey";

            List<Personal> cacheModel;

            if (!_memoryCache.TryGetValue(cacheKey, out cacheModel))
            {
                var lists = _personal.GetList(x => !x.IsDelete && x.IsNewItem == true);
                await lists;
                cacheModel = lists.Result;
                var opts = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memoryCache.Set(cacheKey, cacheModel, opts);
            }


            List<Personal> list = cacheModel;

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

        public async Task<JsonResult> Delete(long? id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new { isSuccess = false });
                }

                Personal personal = await _personal.Get(x => x.PersonalId == id);
                if (personal == null)
                {
                    return Json(new { isSuccess = false });
                }
                personal.IsDelete = true;
                await _personal.Update(personal);
                CleaCache();
            }
            catch (Exception)
            {
                return Json(new { isSuccess = false });
            }
            return Json(new { isSuccess = true });
        }


    }
}
