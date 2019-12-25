using NB_WaterFee.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;

namespace NewWaterFee.Controllers.Security
{
    public class RoleController : BaseController
    {
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sys_OU_Menu_Save(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<string> list = new List<string>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id);
                    }
                    var flag = new AuthorityClient().Sys_OU_Menu_Save(Convert.ToInt32(roleId), list.ToArray());
                    if (flag == "0")
                    {
                        result.StrData1 = roleId;
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.ErrorMsg = flag;
                    }
                }
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 系统-角色-查询树
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMyRoleTreeJson_Server()
        {
            var iUserId = Convert.ToInt32(Session["UserID"].ToString());
            var listtree = new AuthorityClient().Sys_Role_GetTree(iUserId, 0);
            return ToJsonContent(listtree);
        }

        /// <summary>
        /// 系统-角色-通过ID查角色、角色菜单及角色用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sys_Role_GetRoleUserByID_Server(int id)
        {
            CommonResult result = new CommonResult();
            if (id > 9000)
            {
                id -= 9000;
            }
            var rs = new AuthorityClient().Sys_Role_GetRoleMenuUserByID(id);
            if (rs.IsSuccess)
            {
                result.IsSuccess = true;
                result = rs;
            }
            else
            {
                result.ErrorMsg = rs.ErrorMsg;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 系统-角色-操作
        /// </summary>
        /// <param name="RoleInfo"></param>
        /// <param name="type">0 添加 1 修改</param>
        /// <returns></returns>
        public ActionResult Sys_Role_Opr_Server(Role RoleInfo, String type, int id)
        {
            CommonResult result = new CommonResult();
            RoleInfo.NvcEditor = Session["FullName"].ToString();
            RoleInfo.NvcEditorID = Session["UserID"].ToString();
            RoleInfo.DtEdit = DateTime.Now;
            RoleInfo.IntID = id;
            RoleInfo.IntEnabled = 1;

            if (type == "0")
            {
                RoleInfo.NvcCreator = Session["FullName"].ToString();
                RoleInfo.NvcCreatorID = Session["UserID"].ToString();
                RoleInfo.DtCreate = DateTime.Now;
            }
            var flag = new AuthorityClient().Sys_Role_Opr(RoleInfo);
            if (flag == "0")
            {
                result.IsSuccess = true;
            }
            return ToJsonContent(result);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleInfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sys_Role_Del_Server(Role RoleInfo, int id)
        {
            CommonResult result = new CommonResult();
            RoleInfo.NvcEditor = Session["FullName"].ToString();
            RoleInfo.NvcEditorID = Session["UserID"].ToString();
            RoleInfo.DtEdit = DateTime.Now;
            RoleInfo.IntID = id;
            RoleInfo.IntDeleted = 1;

            var flag = new AuthorityClient().Sys_Role_Opr(RoleInfo);
            if (flag == "0")
            {
                result.IsSuccess = true;
            }
            return ToJsonContent(result);
        }
        /// <summary>
        /// 系统-角色-包含菜单保存
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="newList"></param>
        /// <returns></returns>
        public ActionResult Sys_Role_MenuSave_Server(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId))
            {
                List<string> list = new List<string>();
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id);
                    }
                }
                var flag = new AuthorityClient().Sys_Role_MenuSave(Convert.ToInt32(roleId), list.ToArray());
                if (flag == "0")
                {
                    result.StrData1 = roleId;
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = flag;
                }
            }
            return ToJsonContent(result);
        }
        public ActionResult Sys_Role_UserSave_Server(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            List<int> list = new List<int>();
            if (!string.IsNullOrEmpty(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(Convert.ToInt32(id));
                    }
                }
                var flag = new AuthorityClient().Sys_Role_UserSave(Convert.ToInt32(roleId), list.ToArray());
                if (flag == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = flag;
                }
            }
            return ToJsonContent(result);
        }

    }
}