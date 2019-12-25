using NB_WaterFee.ServiceReference1;
using System;
using System.Data;
using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    public class MenuController : BaseController
    {
        // GET: MenuController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetMenuData_Server()
        {
            var userid = Convert.ToInt32(Session["UserID"].ToString());
            var SysTypeID = "WareMIS";
            var rs = new AuthorityClient().Sys_Login_GetMenuByUserID(userid, SysTypeID);
            return Content(rs);
        }

        public ActionResult GetSys_OU_Menu(string OuID, int iMode)
        {
            var iOuID = Convert.ToInt32(OuID);
            var treelist = new AuthorityClient().Sys_OU_Menu_Qry(iOuID, iMode);
            return ToJsonContentDate(treelist);
        }

        /// <summary>
        /// 系统-菜单-增改
        /// </summary>
        /// <returns></returns>
        public ActionResult Sys_Menu_Opr_Server(Menu menuinfo)
        {
            CommonResult result = new CommonResult();
            menuinfo.NvcEditor = Session["FullName"].ToString();
            menuinfo.NvcEditorID = Session["UserId"].ToString();
            menuinfo.DtEdit = DateTime.Now;
            menuinfo.DtEdit = DateTime.Now;
            menuinfo.NvcSysTypeID = "WareMIS";
            //判断是添加还是修改
            if (menuinfo.NvcID == "" || menuinfo.NvcID == null)
            {
                menuinfo.NvcCreator = Session["FullName"].ToString();
                menuinfo.NvcCreatorID = Session["UserId"].ToString();
                menuinfo.DtCreate = DateTime.Now;
            }

            var rs = new AuthorityClient().Sys_Menu_Opr(menuinfo);
            if (rs == "0")
            {
                result.IsSuccess = true;

            }
            else
            {
                result.IsSuccess = false;
                result.ErrorMsg = rs;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 系统-菜单-删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Sys_Menu_Del_Server(string Ids)
        {
            CommonResult result = new CommonResult();
            var arrIds = Ids.Split(',');
            var rs = new AuthorityClient().Sys_Menu_BatchDel(arrIds);
            if (rs == "0")
            {
                result.IsSuccess = true;

            }
            else
            {
                result.IsSuccess = false;
                result.ErrorMsg = rs;
            }
            return ToJsonContent(result);
        }
        /// <summary>
        /// 获取菜单JSON
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuTreeJson_Server()
        {
            var treelist = new AuthorityClient().Sys_Menu_GetTree(0, 10);
            return ToJsonContent(treelist);
        }

        public ActionResult Sys_Menu_GetSon_Server()
        {
            var iQryType = Request["WHC_QryType"];
            var dts = new DataTable();
            if (iQryType == "0")
            {
                var sMenuID = Request["MenuID"];
                dts = new AuthorityClient().Sys_Menu_GetSon(sMenuID, 0, 10);
            }
            else
            {
                Menu menu = new Menu
                {
                    NvcName = Request["WHC_Name"],
                    NvcIcon = Request["WHC_Icon"],
                    NvcSeq = Request["WHC_Seq"],
                    NvcFuncId = Request["WHC_FunctionId"],
                    NvcWinformType = Request["WHC_WinformType"],
                    NvcUrl = Request["WHC_Url"],
                    NvcWebIcon = Request["WHC_WebIcon"]
                };
                dts = new AuthorityClient().Sys_Menu_Qry(menu);
            }

            int rows = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            DataTable dat = new DataTable();
            //复制源的架构和约束
            dat = dts.Clone();
            // 清除目标的所有数据
            dat.Clear();
            //对数据进行分页
            for (int i = (page - 1) * rows; i < page * rows && i < dts.Rows.Count; i++)
            {
                dat.ImportRow(dts.Rows[i]);
            }
            //最重要的是在后台取数据放在json中要添加个参数total来存放数据的总行数，如果没有这个参数则不能分页
            int total = dts.Rows.Count;
            var result = new { total, rows = dat };
            return ToJsonContentDate(result);
        }

        public ActionResult Sys_Menu_FindById()
        {
            Menu menu = new Menu();
            menu.NvcID = Request["WHC_ID"] ?? "";
            var dt = new AuthorityClient().Sys_Menu_Qry(menu);
            return ToJsonContent(dt);
        }

    }
}