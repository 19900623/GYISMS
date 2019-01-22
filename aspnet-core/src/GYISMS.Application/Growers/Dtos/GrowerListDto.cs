

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.Growers;
using Abp.AutoMapper;
using GYISMS.GYEnums;

namespace GYISMS.Growers.Dtos
{
    [AutoMapFrom(typeof(Grower))]
    public class GrowerListDto : FullAuditedEntityDto
    {

        /// <summary>
        /// Year
        /// </summary>
        public int? Year { get; set; }


        /// <summary>
        /// UnitCode
        /// </summary>
        public string UnitCode { get; set; }


        /// <summary>
        /// UnitName
        /// </summary>
        public string UnitName { get; set; }


        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }


        /// <summary>
        /// CountyCode
        /// </summary>
        public AreaCodeEnum? AreaCode { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }


        /// <summary>
        /// ContractNo
        /// </summary>
        public string ContractNo { get; set; }


        /// <summary>
        /// VillageGroup
        /// </summary>
        public string VillageGroup { get; set; }


        /// <summary>
        /// Tel
        /// </summary>
        public string Tel { get; set; }


        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }


        /// <summary>
        /// Type
        /// </summary>
        public int? Type { get; set; }


        /// <summary>
        /// PlantingArea
        /// </summary>
        public decimal? PlantingArea { get; set; }


        /// <summary>
        /// Longitude
        /// </summary>
        public decimal? Longitude { get; set; }


        /// <summary>
        /// Latitude
        /// </summary>
        public decimal? Latitude { get; set; }


        /// <summary>
        /// ContractTime
        /// </summary>
        public DateTime? ContractTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        public Guid? ScheduleDetailId { get; set; }
        public int? VisitNum { get; set; }

        public string CountyCodeName
        {
            get
            {
                return AreaCode.ToString();
            }
        }
        public bool Checked { get; set; }

    }
}