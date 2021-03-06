

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.SystemDatas;
using GYISMS.GYEnums;

namespace GYISMS.SystemDatas.Dtos
{
    public class SystemDataListDto : EntityDto<int>
    {

        /// <summary>
        /// ModelId
        /// </summary>
        public ConfigModel? ModelId { get; set; }


        /// <summary>
        /// Type
        /// </summary>
        [Required(ErrorMessage = "Type不能为空")]
        public ConfigType Type { get; set; }


        /// <summary>
        /// Code
        /// </summary>
        [Required(ErrorMessage = "Code不能为空")]
        public string Code { get; set; }


        /// <summary>
        /// Desc
        /// </summary>
        public string Desc { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// Seq
        /// </summary>
        public int? Seq { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }

        public string ModelName
        {
            get
            {
                return ModelId.ToString();
            }
        }
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }
    }

    public class CheckBoxGroup
    {
        public string Label { get; set; }
        public string Value { get; set; }
        //public bool Checked { get; set; }
    }

    public class SelectGroup
    {
        public string text { get; set; }
        public string value { get; set; }
    }

    public class RadioGroup
    {
        public string text { get; set; }
        public int value { get; set; }
    }
}