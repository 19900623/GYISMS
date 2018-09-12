﻿using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYISMS.VisitExamines
{
    /// <summary>
    /// 拜访考核
    /// </summary>
    [Table("VisitExamines")]
    public class VisitExamine : Entity<Guid>
    {
        /// <summary>
        /// 拜访记录Id 外键
        /// </summary>
        public virtual Guid? VisitRecordId { get; set; }

        /// <summary>
        /// 烟技员Id外键
        /// </summary>
        public virtual string EmployeeId { get; set; }

        /// <summary>
        /// 烟农Id 外键
        /// </summary>
        public virtual string GrowerId { get; set; }

        /// <summary>
        /// 考核项Id 外键
        /// </summary>
        public virtual string TaskExamineId { get; set; }

        /// <summary>
        /// 考核得分（优得5分、良得3分、差得1分）
        /// </summary>
        public virtual int? Score { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
    }
}