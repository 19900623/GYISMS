

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using GYISMS.Growers;

namespace GYISMS.Growers.Dtos
{
    public class GrowerListDto : EntityDto<string>
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
[Required(ErrorMessage="Name不能为空")]
public string Name { get; set; }


/// <summary>
/// CountyCode
/// </summary>
public int? CountyCode { get; set; }


/// <summary>
/// EmployeeId
/// </summary>
public string EmployeeId { get; set; }


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
/// IsDeleted
/// </summary>
public bool? IsDeleted { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }


/// <summary>
/// CreatorUserId
/// </summary>
public long? CreatorUserId { get; set; }


/// <summary>
/// LastModificationTime
/// </summary>
public DateTime? LastModificationTime { get; set; }


/// <summary>
/// LastModifierUserId
/// </summary>
public long? LastModifierUserId { get; set; }


/// <summary>
/// DeletionTime
/// </summary>
public DateTime? DeletionTime { get; set; }


/// <summary>
/// DeleterUserId
/// </summary>
public long? DeleterUserId { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}