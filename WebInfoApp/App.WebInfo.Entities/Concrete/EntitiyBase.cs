﻿using App.Core.Entites;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.WebInfo.Entities.Concrete
{
    public class EntitiyBase : IEntity
    {
        public string CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public string LastModified { get; set; }


        public DateTime? LastModifiedDate { get; set; }

        [DefaultValue(false)]
        public bool IsState { get; set; }

        [DefaultValue(false)]
        public bool IsDelete { get; set; }
    }
}
