

using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using GYISMS.SystemDatas;

namespace  GYISMS.SystemDatas.Dtos
{
    public class SystemDataEditDto
    {
/// <summary>
/// Id
/// </summary>
public int? Id { get; set; }


/// <summary>
/// ModelId
/// </summary>
public int? ModelId { get; set; }


/// <summary>
/// Type
/// </summary>
[Required(ErrorMessage="Type不能为空")]
public int Type { get; set; }


/// <summary>
/// Code
/// </summary>
[Required(ErrorMessage="Code不能为空")]
public string Code { get; set; }


/// <summary>
/// Desc
/// </summary>
[Required(ErrorMessage="Desc不能为空")]
public string Desc { get; set; }


/// <summary>
/// Seq
/// </summary>
public int? Seq { get; set; }


/// <summary>
/// CreationTime
/// </summary>
public DateTime? CreationTime { get; set; }






		//// custom codes
 
        //// custom codes end
    }
}