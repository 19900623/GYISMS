
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

using GYISMS.MeetingRooms.Authorization;
using GYISMS.MeetingRooms.Dtos;
using GYISMS.MeetingRooms;
using GYISMS.Authorization;
using GYISMS.Meetings;
using Abp.Auditing;

namespace GYISMS.MeetingRooms
{
    /// <summary>
    /// MeetingRoom应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class MeetingRoomAppService : GYISMSAppServiceBase, IMeetingRoomAppService
    {
        private readonly IRepository<MeetingRoom, int> _meetingroomRepository;
        private readonly IRepository<Meeting, Guid> _meetingRepository;


        private readonly IMeetingRoomManager _meetingroomManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public MeetingRoomAppService(
        IRepository<MeetingRoom, int> meetingroomRepository
            , IMeetingRoomManager meetingroomManager
            , IRepository<Meeting, Guid> meetingRepository
            )
        {
            _meetingroomRepository = meetingroomRepository;
            _meetingroomManager = meetingroomManager;
            _meetingRepository = meetingRepository;
        }


        /// <summary>
        /// 获取MeetingRoom的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MeetingRoomListDto>> GetPagedMeetingRoomsAsync(GetMeetingRoomsInput input)
        {
            var query = _meetingroomRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name));
            // TODO:根据传入的参数添加过滤条件

            var meetingroomCount = await query.CountAsync();

            var meetingrooms = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var meetingroomListDtos = ObjectMapper.Map<List <MeetingRoomListDto>>(meetingrooms);
            var meetingroomListDtos = meetingrooms.MapTo<List<MeetingRoomListDto>>();
            return new PagedResultDto<MeetingRoomListDto>(
                    meetingroomCount,
                    meetingroomListDtos
                );
        }


        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetMeetingRoomForEditOutput> GetMeetingRoomForEdit(NullableIdDto<int> input)
        {
            var output = new GetMeetingRoomForEditOutput();
            MeetingRoomEditDto meetingroomEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _meetingroomRepository.GetAsync(input.Id.Value);

                meetingroomEditDto = entity.MapTo<MeetingRoomEditDto>();

                //meetingroomEditDto = ObjectMapper.Map<List <meetingroomEditDto>>(entity);
            }
            else
            {
                meetingroomEditDto = new MeetingRoomEditDto();
            }

            output.MeetingRoom = meetingroomEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改MeetingRoom的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateMeetingRoom(CreateOrUpdateMeetingRoomInput input)
        {

            if (input.MeetingRoom.Id.HasValue)
            {
                await UpdateMeetingRoomAsync(input.MeetingRoom);
            }
            else
            {
                await CreateMeetingRoomAsync(input.MeetingRoom);
            }
        }


        /// <summary>
        /// 新增MeetingRoom
        /// </summary>
        protected virtual async Task<MeetingRoomEditDto> CreateMeetingRoomAsync(MeetingRoomEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<MeetingRoom>(input);
            entity.IsDeleted = false;
            if (entity.Photo.Length == 0)
            {
                entity.Photo = "DefaultPhoto";
            }
            //entity = await _meetingroomRepository.InsertAsync(entity);
            var id = await _meetingroomRepository.InsertAndGetIdAsync(entity);
            return entity.MapTo<MeetingRoomEditDto>();
        }

        /// <summary>
        /// 编辑MeetingRoom
        /// </summary>
        protected virtual async Task<MeetingRoomEditDto> UpdateMeetingRoomAsync(MeetingRoomEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _meetingroomRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            var result = await _meetingroomRepository.UpdateAsync(entity);
            return result.MapTo<MeetingRoomEditDto>();
        }

        /// <summary>
        /// 删除MeetingRoom信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteMeetingRoom(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _meetingroomRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除MeetingRoom的方法
        /// </summary>
        [AbpAuthorize(MeetingRoomAppPermissions.MeetingRoom_BatchDelete)]
        public async Task BatchDeleteMeetingRoomsAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _meetingroomRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 新增或修改会议室信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MeetingRoomEditDto> CreateOrUpdateMeetingRoomAsycn(MeetingRoomEditDto input)
        {

            if (input.Id.HasValue)
            {
                return await UpdateMeetingRoomAsync(input);
            }
            else
            {
                return await CreateMeetingRoomAsync(input);
            }
        }

        /// <summary>
        /// 根据id获取会议室信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MeetingRoomListDto> GetMeetingRoomByIdAsync(int id)
        {
            var entity = await _meetingroomRepository.GetAsync(id);
            return entity.MapTo<MeetingRoomListDto>();
        }

        /// <summary>
        /// 删除会议室
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task MeetingRoomDeleteByIdAsync(MeetingRoomEditDto input)
        {
            var entity = await _meetingroomRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = AbpSession.UserId;
            await _meetingroomRepository.UpdateAsync(entity);
        }

        public async Task DeleteMeetingRoom2(int id)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _meetingroomRepository.DeleteAsync(id);
        }


        [AbpAllowAnonymous]
        [Audited]
        public async Task<List<MeetingRoomListDto>> GetDingDingMeetingRoomsAsync()
        {
            var room = _meetingroomRepository.GetAll();
            var meeting = _meetingRepository.GetAll();

            var CurrentStatusCount = await _meetingRepository.GetAll().Where(r => r.BeginTime <= DateTime.Now.AddHours(2) && r.EndTime <= DateTime.Now.AddHours(2)).CountAsync();
            //var result = from r in room
            //             join m in meeting on r.Id equals m.MeetingRoomId
            //             //into leftTable
            //             //from entity in leftTable.DefaultIfEmpty()
            //             select new MeetingRoomListDto()
            //             {
            //                 Id = r.Id,
            //                 Name = r.Name + $"({r.Num}人)",
            //                 Photo = r.Photo,
            //                 RoomTypeName = r.RoomType.ToString(),
            //                 CurrentStatus = m.BeginTime == null ? $"未来两小时可预定({((bool)r.IsApprove?"需要审核":"无需审核")})" : (m.BeginTime <= DateTime.Now.AddHours(2) && m.EndTime <= DateTime.Now.AddHours(2) ? $"未来两小时不可预定({((bool)r.IsApprove ? "需要审核" : "无需审核")})" : $"未来两小时可预定({((bool)r.IsApprove ? "需要审核" : "无需审核")})")
            //             };

            var result = from r in room
                         select new MeetingRoomListDto()
                         {
                             Id = r.Id,
                             Name = r.Name + $"({r.Num}人)",
                             Photo = r.Photo,
                             RoomTypeName = r.RoomType.ToString(),
                             CurrentStatus = CurrentStatusCount==0? $"未来两小时可预订({((bool)r.IsApprove?"需要审核":"无需审核")})" : $"未来两小时不可预订({((bool)r.IsApprove?"需要审核":"无需审核")})"
                         };
            var meetingrooms = await result
                    .OrderBy(v => v.Id).AsNoTracking()
                    .ToListAsync();
            var meetingroomListDtos = meetingrooms.MapTo<List<MeetingRoomListDto>>();
            return meetingroomListDtos;
        }

        /// <summary>
        /// 根据id获取会议室信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [Audited]
        public async Task<MeetingRoomListDto> GetDingDingMeetingRoomByIdAsync(int id)
        {
            var room =await _meetingroomRepository.GetAll().Where(v => v.Id == id).AsNoTracking().FirstOrDefaultAsync();
            var roomDto = room.MapTo<MeetingRoomListDto>();
            return roomDto;
        }
    }
}


