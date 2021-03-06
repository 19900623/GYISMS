

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.GYEnums;
using GYISMS.VisitTasks;
using System;

namespace GYISMS.VisitTasks.Dtos
{
    public class GetVisitTasksInput : PagedAndSortedInputDto, IShouldNormalize
    {
          /// <summary>
		 /// 模糊搜索使用的关键字
		 ///</summary>
        public string Name { get; set; }
        public TaskTypeEnum? TaskType { get; set; }

        public string[] Ids { get; set; }
        public Guid ScheduleId { get; set; }
		//// custom codes
 
        //// custom codes end

			  /// <summary>
			 /// 正常化排序使用
			///</summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }


    }
}
