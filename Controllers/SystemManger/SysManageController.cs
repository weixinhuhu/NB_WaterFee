using NB_WaterFee.DbServiceReference;
using System;
using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;
namespace NB_WaterFee.Controllers.SystemManger
{
    public class SysManageController : BaseController
    {
        // GET: SysManage
        public ActionResult Setting()
        {
            return View();
        }
        public ActionResult Setting_Save_Server()
        {
            CommonResult result = new CommonResult();
            try
            {
                var endcode = Session["EndCode"] ?? "0";
                var ID = Request["ID"].ToIntOrDefault();
                //0预付费,1后付费
                var payMode = Request["payMode"].ToIntOrDefault(1);
                var balanceDay = Request["balanceDay"].ToIntOrDefault(1);
                var AreaCode = Request["AreaCode"];
                //0自动， 1手动
                var AutoSwitch = Request["AutoSwitch"].ToIntOrDefault(1);

                var Parm = new ParamEndUser();
                Parm.IntID = ID;
                Parm.IntEndCode = endcode.ToString().ToInt();
                Parm.IntPayMode = payMode;
                Parm.IntBalanceDay = balanceDay;
                Parm.IntSwitchMode = AutoSwitch;
                Parm.IntRegionCode = AreaCode.ToInt();
                Parm.IntLstUpdID = Session["UserID"].ToString().ToInt();
                Parm.IntCreateID = Session["UserID"].ToString().ToInt();
                Parm.DtLstUpd = DateTime.Now;

                var rs = new ServiceDbClient().Param_EndUser_Opr(Parm);
                if (rs == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = rs;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = "保存失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        public ActionResult Param_EndUser_Qry_Server() {

            CommonResult result = new CommonResult();
            try
            {
               var endcode = Session["EndCode"] ?? "0";
               result= new ServiceDbClient().Param_EndUser_Qry(endcode.ToString().ToInt());
            }
            catch (Exception ex)
            {
                result.ErrorMsg = "保存失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

    }
}