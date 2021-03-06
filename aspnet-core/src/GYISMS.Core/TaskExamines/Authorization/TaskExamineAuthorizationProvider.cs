
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.TaskExamines.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="TaskExamineAppPermissions" /> for all permission names. TaskExamine
    ///</summary>
    public class TaskExamineAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了TaskExamine 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var taskexamine = administration.CreateChildPermission(TaskExamineAppPermissions.TaskExamine , L("TaskExamines"));
taskexamine.CreateChildPermission(TaskExamineAppPermissions.TaskExamine_Create, L("Create"));
taskexamine.CreateChildPermission(TaskExamineAppPermissions.TaskExamine_Edit, L("Edit"));
taskexamine.CreateChildPermission(TaskExamineAppPermissions.TaskExamine_Delete, L("Delete"));
taskexamine.CreateChildPermission(TaskExamineAppPermissions.TaskExamine_BatchDelete , L("BatchDelete"));
taskexamine.CreateChildPermission(TaskExamineAppPermissions.TaskExamine_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }