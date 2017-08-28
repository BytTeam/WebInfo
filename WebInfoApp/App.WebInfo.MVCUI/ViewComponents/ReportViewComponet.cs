using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.WebInfo.Business.Abstract;
using App.WebInfo.Entities.Concrete;
using App.WebInfo.MVCUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace App.WebInfo.MVCUI.ViewComponents
{
    public class ReportViewComponet:ViewComponent
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUtileService _utileService;
        private ReportViewModel _model;
        public ReportViewComponet(IMemoryCache memoryCache,  IUtileService utileService)
        {
            _memoryCache = memoryCache;
            _utileService = utileService;
            _model=new ReportViewModel();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Bind();
            _model.Filter = new Personal
            {
                PersonalId = 31
            };
            return View(_model);
        }
        private async Task Bind()
        {
            var cacheKey = "Personal_cache_report_bind";
            if (!_memoryCache.TryGetValue(cacheKey, out ReportViewModel cacheModel))
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

                var opts = new MemoryCacheEntryOptions
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
    }
}
