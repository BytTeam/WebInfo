﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.WebInfo.Entities.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.WebInfo.MVCUI.Models
{
    public class ReportViewModel
    {
        public Personal Filter { get; set; }
        public String DogumTarihiBegin { get; set; }
        public String DogumTarihiEnd { get; set; }
        public SelectList CinsiyetList { get; set; }
        public SelectList DinList { get; set; }
        public SelectList DogumYeriList { get; set; }
        public SelectList EgitimDurumuList { get; set; }
        public SelectList IkametDurumuList { get; set; }
        public SelectList IlList { get; set; }
        public SelectList IlceList { get; set; }
        public SelectList IslemYapanList { get; set; }
        public SelectList KanGrubuList { get; set; }
        public SelectList KayitDurumuList { get; set; }
        public SelectList KokenList { get; set; }
        public SelectList MedeniDurumuList { get; set; }
        public SelectList SaglikDurumuList { get; set; }
        public SelectList SosyalYardimDurumuList { get; set; }
        public SelectList UyrukList { get; set; }
    }
}
