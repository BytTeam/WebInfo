using App.WebInfo.Business;
using App.WebInfo.Business.Abstract;
using App.WebInfo.Entities.Concrete;
using App.WebInfo.MVCUI.Helpers;
using App.WebInfo.MVCUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.WebInfo.MVCUI.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IPersonalService _personalService;
        private readonly IHostingEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly IUtileService _utileService;
        private PersonalViewModel _model;

        public HomeController(IPersonalService personalService, IHostingEnvironment environment, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, IUtileService utileService)
        {
            _personalService = personalService;
            _environment = environment;
            _memoryCache = memoryCache;
            _utileService = utileService;
            HttpContextAccessor = httpContextAccessor;
            _model = new PersonalViewModel { Personal = new Personal() };
            Bind();

        }

        public async Task<IActionResult> Index()
        {

            SessionUser sessionUser = new SessionUser
            {
                UserName = User.Identity.Name,
                IsAdmin = User.IsInRole(AppConst.UserRole.Admin),
                IsGeneral = User.IsInRole(AppConst.UserRole.General),
                IsHelp = User.IsInRole(AppConst.UserRole.Help),
                IsPrivate = User.IsInRole(AppConst.UserRole.Admin),
                IsEducation = User.IsInRole(AppConst.UserRole.Admin),
                IsCreate = User.IsInRole(AppConst.UserRole.Create),
                IsDelete = User.IsInRole(AppConst.UserRole.Delete)
            };
            if (sessionUser.IsAdmin)
            {
                sessionUser.IsGeneral = sessionUser.IsHelp =
                    sessionUser.IsPrivate = sessionUser.IsEducation =
                        sessionUser.IsCreate = sessionUser.IsDelete = true;
            }
            HttpContextAccessor.HttpContext.Session.SetObject(AppConst.UserSessionName, sessionUser);

            List<HomeReporBoxModel> reportBoxs = new List<HomeReporBoxModel>();
            long totalPersonalCount = await _personalService.Count(x => !x.IsDelete);
            reportBoxs.Add(new HomeReporBoxModel
            {
                Count = totalPersonalCount,
                Title = "Toplam Yabancı Sayısı",
                SubTitle = "Yabancı Sayısı Yüzde",
                Icon = "icon-user",
                Progress = 100
            });

            //Okuyor Id 3
            long egitimGorenCount =
                await _personalService.Count(x => !x.IsDelete && x.EgitimDurumu.EgitimDurumuId == 3);
            reportBoxs.Add(new HomeReporBoxModel
            {
                Count = egitimGorenCount,
                Progress = totalPersonalCount == 0 ? 0 : (100 * egitimGorenCount) / totalPersonalCount,
                Title = "Eğitim Gören",
                SubTitle = "Eğitim Gören Yüzde",
                Icon = "icon-user"
            });

            long kacakCount =
               await _personalService.Count(x => !x.IsDelete && (x.IkametDurumu.IkametDurumuId == 3 || x.IkametDurumu.IkametDurumuId == 4));
            reportBoxs.Add(new HomeReporBoxModel
            {
                Count = kacakCount,
                Progress = totalPersonalCount == 0 ? 0 : (100 * kacakCount) / totalPersonalCount,
                Title = "Kaçak Sayısı",
                SubTitle = "Kaçak Sayısı Yüzde",
                Icon = "icon-info"
            });
            var model = new HomeViewModel
            {
                ReporBoxModels = reportBoxs
            };
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult ImportFile()
        {
            return View(new ImportModel() { Personals = new List<Personal>() });
        }

        [HttpPost]
        public IActionResult ImportFile(IFormFile importFile)
        {
            List<Personal> personals = new List<Personal>();
            string newFileName = FileUpload(importFile);
            string sWebRootFolder = _environment.WebRootPath;
            string sFileName = Path.Combine("uploads", newFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

            try
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 1] != null)
                        {
                            string strIkametDurumu = CheckString(worksheet.Cells[row, 1].Value);
                            string strSahisNo = CheckString(worksheet.Cells[row, 2].Value);
                            string strAdi = CheckString(worksheet.Cells[row, 3].Value);
                            string strSoyadi = CheckString(worksheet.Cells[row, 4].Value);
                            string strBabaadi = CheckString(worksheet.Cells[row, 5].Value);
                            string strAnaAdi = CheckString(worksheet.Cells[row, 6].Value);
                            string strDTarihi = CheckString(worksheet.Cells[row, 7].Value);
                            string strDYeri = CheckString(worksheet.Cells[row, 8].Value);
                            string strKIl = CheckString(worksheet.Cells[row, 9].Value);
                            string strKIlce = CheckString(worksheet.Cells[row, 10].Value);
                            string strAdliIslem = CheckString(worksheet.Cells[row, 11].Value);
                            string strMahalle = CheckString(worksheet.Cells[row, 12].Value);
                            string strSokak = CheckString(worksheet.Cells[row, 13].Value);
                            string strNo = CheckString(worksheet.Cells[row, 14].Value);
                            string strTelefon = CheckString(worksheet.Cells[row, 15].Value);
                            string strAileNo = CheckString(worksheet.Cells[row, 16].Value);
                            string strKacak = CheckString(worksheet.Cells[row, 17].Value);
                            string strIslemiYapan = CheckString(worksheet.Cells[row, 18].Value);
                            string strSYardimDurumu = CheckString(worksheet.Cells[row, 19].Value);
                            string strCinsiyet = CheckString(worksheet.Cells[row, 20].Value);
                            string strMedeniDurumu = CheckString(worksheet.Cells[row, 21].Value);
                            string strUyruk = CheckString(worksheet.Cells[row, 22].Value);
                            string strMezhep = CheckString(worksheet.Cells[row, 23].Value);
                            string strEtnikKoken = CheckString(worksheet.Cells[row, 24].Value);
                            string strEDurumu = CheckString(worksheet.Cells[row, 25].Value);
                            string strMeslegi = CheckString(worksheet.Cells[row, 26].Value);
                            string strDini = CheckString(worksheet.Cells[row, 27].Value);
                            string strSDurumu = CheckString(worksheet.Cells[row, 28].Value);
                            string strKanGrubu = CheckString(worksheet.Cells[row, 29].Value);
                            string strBeden = CheckString(worksheet.Cells[row, 30].Value);
                            string strAyakkabi = CheckString(worksheet.Cells[row, 31].Value);
                            string strKayitTarihi = CheckString(worksheet.Cells[row, 32].Value);
                            string strKayitTipi = CheckString(worksheet.Cells[row, 33].Value);
                            string strKayitDurumu = CheckString(worksheet.Cells[row, 34].Value);
                            string strKampAdi = CheckString(worksheet.Cells[row, 35].Value);
                            string strKampIli = CheckString(worksheet.Cells[row, 36].Value);
                            string strKampBolgesi = CheckString(worksheet.Cells[row, 37].Value);
                            string strKampMah = CheckString(worksheet.Cells[row, 38].Value);
                            string strKonteynerNo = CheckString(worksheet.Cells[row, 39].Value);
                            var personal = new Personal
                            {
                                IkametDurumu = new IkametDurumu
                                {
                                    IkametDurumuId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.IkametDurumuList, strIkametDurumu))
                                },
                                SahisNo = strSahisNo,
                                Adi = strAdi,
                                Soyadi = strSoyadi,
                                BabaAdi = strBabaadi,
                                AnaAdi = strAnaAdi,
                                DogumTarihi = strDTarihi,
                                DogumYeri = new DogumYeri
                                {
                                    DogumYeriId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.DogumYeriList, strDYeri))
                                },
                                AdliIslemDurumu = strAdliIslem,
                                Mahalle = strMahalle,
                                Sokak = strSokak,
                                KapiNo = strNo,
                                Telefon = strTelefon,
                                AileNo = strAileNo,
                                IslemYapan = new IslemYapan
                                {
                                    IslemYapanId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.IslemYapanList, strIslemiYapan))
                                },
                                SosyalYardimDurumu = new SosyalYardimDurumu
                                {
                                    SosyalYardimDurumuId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.SosyalYardimDurumuList, strSYardimDurumu))
                                },
                                Cinsiyet = new Cinsiyet()
                                {
                                    CinsiyeId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.CinsiyetList, strCinsiyet))
                                },
                                MedeniDurumu = new MedeniDurumu
                                {
                                    MedeniDurumuId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.MedeniDurumuList, strMedeniDurumu))
                                },
                                Uyruk = new Uyruk
                                {
                                    UyrukId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.UyrukList, strUyruk))
                                },
                                Mezhep = strMezhep,
                                Koken = new Koken
                                {
                                    KokenId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.KokenList, strEtnikKoken))
                                },
                                EgitimDurumu = new EgitimDurumu
                                {
                                    EgitimDurumuId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.EgitimDurumuList, strEDurumu))
                                },
                                Meslegi = strMeslegi,
                                Din = new Din
                                {
                                    DinId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.DinList, strDini))
                                },
                                SaglikDurumu = new SaglikDurumu
                                {
                                    SaglikDurumuId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.SaglikDurumuList, strSDurumu))
                                },
                                KanGrubu = new KanGrubu
                                {
                                    KanGrubuId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.KanGrubuList, strKanGrubu))
                                },
                                Bedeni = strBeden,
                                AyakkabiNo = strAyakkabi == "Boş" ? 0 : Convert.ToInt32(strAyakkabi),
                                KayitTarihi = strKayitTarihi,
                                KayitTipi = strKayitTipi,
                                KayitDurumu = new KayitDurumu
                                {
                                    KayitDurumuId = Convert.ToInt32(GetDefaultOrSelectedItemValue(_model.KayitDurumuList, strKayitDurumu))
                                },
                                KampAdi = strKampAdi,
                                KampMahallesi = strKampMah,
                                KonteynerNo = strKonteynerNo
                            };
                            personals.Add(personal);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            List<Personal> complatePersonal = new List<Personal>();
            //await _personalService.Add(personals);
            Parallel.For(0, personals.Count, i =>
            {
                var itemResult = AddPersonal(personals[i]);
               
               
            });

            ImportModel model = new ImportModel
            {
                Personals = personals
            };
            return View(model);
        }

        public string CheckString(object check)
        {

            if (check == null || check.ToString().Trim().Length == 0)
            {
                return "Boş";
            }
            return check.ToString();
        }

        public async Task<bool> AddPersonal(Personal item)
        {
            var resultitem = _personalService.Add(item);
            await resultitem;

            return resultitem.IsCompleted;
        }

        private string FileUpload(IFormFile imageFile)
        {
            var newFileName = string.Empty;

            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(_environment.WebRootPath, "uploads");
                var fileName = imageFile.FileName;
                var imageType = fileName.Substring(fileName.LastIndexOf('.'),
                    fileName.Length - fileName.LastIndexOf('.'));
                newFileName = Guid.NewGuid() + "-" + imageType;

                using (Stream fs = System.IO.File.Create(Path.Combine(path, newFileName)))
                {
                    if (fs != null)
                    {
                        imageFile.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
            return newFileName;
        }

        [HttpGet]
        public IActionResult Import()
        {
            string sWebRootFolder = _environment.WebRootPath;
            string sFileName = Path.Combine("uploads", "demo.xlsx");
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    bool bHeaderRow = true;
                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= ColCount; col++)
                        {
                            if (bHeaderRow)
                            {
                                sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
                            }
                        }
                        sb.Append(Environment.NewLine);
                    }
                    return View(new ImportModel() { file = sb.ToString() });
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        private async void Bind()
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
                var kayitDurumuTask = _utileService.GetKayitDurumus();
                var kokenTask = _utileService.GetKokens();
                var medeniDurumuTask = _utileService.GetMedeniDurumus();

                await Task.WhenAll(cinsiyetTask, dinTask, dogumYeriTask, egitimDurumuTask, ikametDurumuTask, ilTask, ilceTask, islemYapanTask, kanGrubuTask, uyrukTask, saglikDurumuTask, kangurubuTask, sosyalYardimTask, kayitDurumuTask, kokenTask, medeniDurumuTask);

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
                _model.SosyalYardimDurumuList = ConvertSelectList(sosyalYardimTask.Result.Select(x => new { Id = x.SosyalYardimDurumuId, Value = x.SosyalYardimDurumuName }));
                _model.KayitDurumuList = ConvertSelectList(kayitDurumuTask.Result.Select(x => new { Id = x.KayitDurumuId, Value = x.KayitDurumuName }));

                _model.KokenList = ConvertSelectList(kokenTask.Result.Select(x => new { Id = x.KokenId, Value = x.KokenName }));

                _model.MedeniDurumuList = ConvertSelectList(medeniDurumuTask.Result.Select(x => new { Id = x.MedeniDurumuId, Value = x.MedeniDurumuName }));

                cacheModel = _model;

                var opts = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                };
                _memoryCache.Set(cacheKey, cacheModel, opts);
            }
            _model = cacheModel;
        }
        public SelectList ConvertSelectList(IEnumerable<object> list)
        {
            return new SelectList(list, "Id", "Value");
        }

        public Object GetDefaultOrSelectedItemValue(SelectList list, string selectedValue)
        {
            return list.FirstOrDefault(x => x.Text == selectedValue || x.Text == "Seçiniz").Value;
        }
    }
}

