using NB_WaterFee.DbServiceReference;
using System;
using System.Data;
using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    /// <summary>
    /// 算费菜单
    /// </summary>
    public class CountFeeController : BaseController
    {
        //审核抄表数据
        public ActionResult ApproveData()
        {
            return View();
        }

        public ActionResult CountReleaseFee()
        {
            return View();
        }

        //预存款销账
        public ActionResult DepositAccount()
        {
            return View();
        }

        public ActionResult Account_CalcFee_Qry_Server()
        {
            var Strlevel = Request["WHC_Treelevel"];
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var ParentText = Request["WHC_TreePrentText"];
            var flag = Request["WHC_IntFlag"];
            var condi = new AccCalcQryCondition()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                IntNo = (Request["WHC_IntNo"] ?? "").ToString().ToIntOrZero(),
                IntFlag = flag.ToIntOrZero(),
                DtFee = Request["WHC_DtDate"].ToDateTime()
            };

            if (Strlevel == "1")
            {
                condi.NvcVillage = "所有小区";
            };

            if (Strlevel == "2")
            {
                condi.NvcVillage = Text;
            }

            if (Strlevel == "3")
            {
                condi.NvcVillage = fuji;
                condi.VcBuilding = Text;
            }

            if (Strlevel == "4")
            {
                condi.NvcVillage = ParentText;
                condi.VcBuilding = fuji;
                condi.VcUnitNum = Text;
            }

            condi.IntEndCode = endcode.ToString().ToIntOrZero();
            var dts = new ServiceDbClient().Account_CalcFee_Qry(condi);
            //分页参数
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

        public ActionResult Account_CalcFee_Calc_Server()
        {
            CommonResult result = new CommonResult();
            var iCode = endcode.ToString().ToIntOrZero();
            var iUserID = userid.ToString().ToIntOrZero();
            var ids = Request["sAdrs"];
            var DtFee = Request["WHC_DtDate"].ToDateTime();
            var arrIds = ids.Split(',');
            var rs = new ServiceDbClient().Account_CalcFee_Calc(iCode, iUserID,DtFee, arrIds);
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
    }
}
