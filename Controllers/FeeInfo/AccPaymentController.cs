using NB_WaterFee.DbServiceReference;
using System;
using System.Data;
using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    public class AccPaymentController : BaseController
    {
        public ActionResult CounterReverseData_Server()
        {
            var endcode = Session["EndCode"] ?? "0";
            var CustNo = Request["WHC_IntCustNo"] ?? "";
            var NvcName = Request["WHC_NvcName"] ?? "";
            var NvcAddr = Request["WHC_NvcAddr"] ?? "";
            var VcMobile = Request["WHC_VcMobile"] ?? DateTime.Now.ToString();
            var DtStart = Request["WHC_DtStart"] ?? DateTime.Now.ToString();
            var Dtend = Request["WHC_DtEnd"] ?? "";
            var custinfo = new Customer
            {
                IntNo = CustNo == "" ? 0 : CustNo.ToInt(),
                NvcName = NvcName,
                NvcAddr = NvcAddr,
                VcMobile = VcMobile
            };
            var dt = new ServiceDbClient().Account_GetPaymentDetail(endcode.ToString().ToInt(), 0, DtStart.ToDateTime(), Dtend.ToDateTime(), custinfo);
            var result = new { total = dt.Rows.Count, rows = dt };
            return ToJsonContentDate(result);
        }

        //柜台冲正数据
        public ActionResult CounterReverseDataByIntCustNo_Server()
        {
            var custno = Request["WHC_IntCustNo"] ?? "0";
            var endcode = Session["EndCode"] ?? "0";
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Account_GetWriteoffByCustNo(endcode.ToString().ToInt(), custno.ToInt());
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
    }
}
