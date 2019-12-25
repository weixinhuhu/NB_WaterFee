using NB_WaterFee.ServiceReference1;
using System;
using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;

namespace NewWaterFee.Controllers.Security
{
    public class OUController : BaseController
    {
        public OUController() : base()
        {
        }
        public ActionResult UserMenu()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OU_Opr_Server(OU OrgInfo)
        {
            CommonResult result = new CommonResult();
            OrgInfo.IntEnabled = 1;
            OrgInfo.IntDeleted = 0;
            OrgInfo.DtEdit = DateTime.Now;
            OrgInfo.NvcCreator = Session["FullName"].ToString();
            OrgInfo.NvcCreatorID = Session["UserId"].ToString();
            var flag = new AuthorityClient().Sys_OU_Opr(OrgInfo);
            if (flag == "0")
            {
                result.IsSuccess = true;

            }
            else
            {
                result.IsSuccess = false;
                result.ErrorMsg = flag;
            }
            return ToJsonContent(result);
        }

        public ActionResult OU_Recover_Server(OU OrgInfo, String Pid)
        {
            CommonResult result = new CommonResult();
            OrgInfo.IntEnabled = 1;
            OrgInfo.IntDeleted = 0;
            OrgInfo.IntPID = Convert.ToInt32(Pid);
            OrgInfo.DtEdit = DateTime.Now;
            OrgInfo.NvcCreator = Session["FullName"].ToString();
            OrgInfo.NvcCreatorID = Session["UserId"].ToString();
            var flag = new AuthorityClient().Sys_OU_Opr(OrgInfo);
            if (flag == "0")
            {
                result.IsSuccess = true;

            }
            else
            {
                result.IsSuccess = false;
                result.ErrorMsg = flag;
            }
            return ToJsonContent(result);
        }
        public ActionResult OU_Del_Server(String id)
        {
            CommonResult result = new CommonResult();
            OU OrgInfo = new OU();
            OrgInfo.IntDeleted = 1;
            OrgInfo.IntID = Convert.ToInt32(id);
            OrgInfo.NvcCreator = Session["FullName"].ToString();
            OrgInfo.NvcCreatorID = Session["UserId"].ToString();
            var flag = new AuthorityClient().Sys_OU_Opr(OrgInfo);
            if (flag == "0")
            {
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.ErrorMsg = flag;
            }
            return ToJsonContent(result);
        }
        public ActionResult OU_FindById_Server()
        {
            CommonResult result = new CommonResult();
            var id = Convert.ToInt32(Request["ID"]);
            var dt = new AuthorityClient().Sys_Ou_GetByID(id);
            return ToJsonContent(dt);
        }
        public ActionResult GetSys_OU_Menu(string OuID, int iMode)
        {
            var iOuID = Convert.ToInt32(OuID);
            var treelist = new AuthorityClient().Sys_OU_Menu_Qry(iOuID, iMode);
            return ToJsonContentDate(treelist);
        }

    }
}