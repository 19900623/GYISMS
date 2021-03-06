
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using GYISMS.Documents.Dtos;
using GYISMS.Documents.DomainService;
using GYISMS.Dtos;
using GYISMS.DocAttachments;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using GYISMS.Helpers;
using GYISMS.DocCategories.DomainService;
using GYISMS.Employees;
using System.Text.RegularExpressions;

namespace GYISMS.Documents
{
    /// <summary>
    /// Document应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DocumentAppService : GYISMSAppServiceBase, IDocumentAppService
    {
        private readonly IRepository<Document, Guid> _entityRepository;
        private readonly IRepository<DocAttachment, Guid> _docAttachmentRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDocumentManager _entityManager;
        private readonly IDocCategoryManager _docCategoryManager;
        private readonly IRepository<Employee, string> _employeeRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DocumentAppService(
        IRepository<Document, Guid> entityRepository
        , IDocumentManager entityManager
            , IRepository<DocAttachment, Guid> docAttachmentRepository
        , IHostingEnvironment hostingEnvironment
        , IDocCategoryManager docCategoryManager
            , IRepository<Employee, string> employeeRepository
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _docAttachmentRepository = docAttachmentRepository;
            _hostingEnvironment = hostingEnvironment;
            _docCategoryManager = docCategoryManager;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取Document的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DocumentListDto>> GetPaged(GetDocumentsInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.CategoryCode), e => ("," + e.CategoryCode + ",").Contains(input.CategoryCode))
                .WhereIf(!string.IsNullOrEmpty(input.KeyWord), e => e.Name.Contains(input.KeyWord) || e.Summary.Contains(input.KeyWord));

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<DocumentListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<DocumentListDto>>();

            return new PagedResultDto<DocumentListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取DocumentListDto信息
        /// </summary>

        public async Task<DocumentListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<DocumentListDto>();
        }

        /// <summary>
        /// 获取编辑 Document
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetDocumentForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetDocumentForEditOutput();
            DocumentEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<DocumentEditDto>();

                //documentEditDto = ObjectMapper.Map<List<documentEditDto>>(entity);
            }
            else
            {
                editDto = new DocumentEditDto();
            }

            output.Document = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Document的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<APIResultDto> CreateOrUpdate(CreateOrUpdateDocumentInput input)
        {

            if (input.Document.Id.HasValue)
            {
                await Update(input.Document);
                return new APIResultDto() { Code = 0, Msg = "保存成功" };
            }
            else
            {
                var entity = await Create(input.Document);
                return new APIResultDto() { Code = 0, Msg = "保存成功", Data = entity.Id };
            }
        }


        /// <summary>
        /// 新增Document
        /// </summary>

        protected virtual async Task<DocumentEditDto> Create(DocumentEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Document>(input);
            var entity = input.MapTo<Document>();
            var categoryList = await _docCategoryManager.GetHierarchyCategories(input.CategoryId);
            entity.CategoryCode = string.Join(',', categoryList.Select(c => c.Id).ToArray());
            entity.CategoryDesc = string.Join(',', categoryList.Select(c => c.Name).ToArray());
            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<DocumentEditDto>();
        }

        /// <summary>
        /// 编辑Document
        /// </summary>

        protected virtual async Task Update(DocumentEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Document信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Document的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        public Task DownloadZipFileTest()
        {
            return Task.Run(() =>
            {
                ZipHelper.ZipFileDirectory(@"F:\zipfiles", @"F:\zipfiles.zip");
            });
        }

        public async Task<APIResultDto> DownloadQRCodeZip(GetDocumentsInput input)
        {
            var query = _entityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.CategoryCode), e => ("," + e.CategoryCode + ",").Contains(input.CategoryCode))
                .WhereIf(!string.IsNullOrEmpty(input.KeyWord), e => e.Name.Contains(input.KeyWord) || e.Summary.Contains(input.KeyWord));
            var docs = await query.Select(q => new { q.Id, q.Name, q.CategoryDesc }).ToListAsync();

            string webRootPath = _hostingEnvironment.WebRootPath;
            string filePath = webRootPath + "/docqrcodes";
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
                Directory.CreateDirectory(filePath);
            }
            else
            {
                Directory.CreateDirectory(filePath);
            }
            foreach (var item in docs)
            {
                QRCodeHelper.GenerateQRCode(item.Id.ToString(), string.Format("{0}/{1}-{2}.jpg", filePath, item.CategoryDesc.Replace(',', '-'), item.Name), QRCoder.QRCodeGenerator.ECCLevel.Q, 20);
            }
            var zipFiles = "/downloads/资料二维码.zip";
            var zipPath = webRootPath + "/downloads";
            if (!Directory.Exists(zipPath))
            {
                Directory.CreateDirectory(zipPath);
            }
            ZipHelper.ZipFileDirectory(filePath, string.Format("{0}{1}", webRootPath, zipFiles));
            return new APIResultDto() { Code = 0, Msg = "生成二维码成功", Data = zipFiles };
        }

        /// <summary>
        /// 钉钉资料详情&扫码搜索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<DocumentListDto> GetDocInfoByScanAsync(Guid id, string host)
        {
            var doc = await _entityRepository.GetAll().Where(v => v.Id == id).AsNoTracking().FirstOrDefaultAsync();
            var result = doc.MapTo<DocumentListDto>();
            if (result.CategoryDesc.Contains(','))
            {
                result.CategoryDesc = result.CategoryDesc.Replace(",", " > ");
            }
            var att = _docAttachmentRepository.GetAll().Where(v => v.DocId == id).AsNoTracking();
            var gridList = await (from a in att
                                  select new GridDocListDto()
                                  {
                                      Text = a.Name + a.Path.Substring(a.Path.LastIndexOf('.')),
                                      Icon = host + "knowledge/annex.png",
                                      FileUrl = host + a.Path
                                  }).AsNoTracking().ToListAsync();
            result.FileList = new List<GridDocListDto>();
            result.FileList.AddRange(gridList);
            return result;
        }

        /// <summary>
        /// 判断用户是否拥有权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<bool> GetHasDocPermissionFromScanAsync(Guid id, string userId)
        {
            var user = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(user.Department) && user.Department.Contains('['))
            {
                var departmentId = user.Department.Replace('[', ' ').Replace(']', ' ').Trim();
                int count = await _entityRepository.GetAll().Where(v => v.Id == id && (v.IsAllUser == true || v.DeptIds.Contains(departmentId) || v.EmployeeIds.Contains(userId))).AsNoTracking().CountAsync();
                if (count != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取文件列表项
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<DocumentListDto>> GetDocListByParentIdAsync(string categoryCode, string userId, int pageIndex, int pageSize)
        {
            var user = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();
            var departmentId = user.Department.Replace('[', ' ').Replace(']', ' ').Trim();
            var query = _entityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(categoryCode), e => ("," + e.CategoryCode + ",").Contains("," + categoryCode + ","));
            var list = await (from d in query
                              select new DocumentListDto()
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  Summary = d.Summary.Length > 20 ? d.Summary.Substring(0, 20) + "..." : d.Summary,
                                  ReleaseDate = d.ReleaseDate,
                                  IsAllUser = d.IsAllUser,
                                  DeptIds = d.DeptIds,
                                  EmployeeIds = d.EmployeeIds
                              }).Where(v => v.IsAllUser == true || v.DeptIds.Contains(departmentId) || v.EmployeeIds.Contains(userId))
                              .OrderBy(v => v.Id).AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return list;
        }


        /// <summary>
        /// 搜索标题和摘要
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<DocumentListDto>> GetDocListByInputAsync(string input, string catId, string userId, int pageIndex, int pageSize)
        {
            var user = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();
            var departmentId = user.Department.Replace('[', ' ').Replace(']', ' ').Trim();
            var query = _entityRepository.GetAll();
            var list = await (from d in query
                              select new DocumentListDto()
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  Summary = d.Summary.Length > 20 ? d.Summary.Substring(0, 20) + "..." : d.Summary,
                                  ReleaseDate = d.ReleaseDate,
                                  CategoryCode = d.CategoryCode,
                                  IsAllUser = d.IsAllUser,
                                  DeptIds = d.DeptIds,
                                  EmployeeIds = d.EmployeeIds
                              }).Where(v => v.IsAllUser == true || v.DeptIds.Contains(departmentId) || v.EmployeeIds.Contains(userId))
                              .WhereIf(!string.IsNullOrEmpty(catId), v => ("," + v.CategoryCode + ",").Contains("," + catId + ","))
                              .WhereIf(!string.IsNullOrEmpty(input), v => v.Name.Contains(input) || v.Summary.Contains(input))
                              .OrderBy(v => v.Id).AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return list;
        }
    }
}


