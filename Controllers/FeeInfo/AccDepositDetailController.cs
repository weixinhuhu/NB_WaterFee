using NB_WaterFee.DbServiceReference;
using System;
using System.Data;
using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    public class AccDepositDetailController : BaseController
    {

        public ActionResult GetDetailByCustomerNo_Server()
        {         
            var custno = Request["WHC_IntCustNo"] ?? "0";
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Account_GetDepositDetailByCustNo(endcode, custno.ToInt());

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

        public ActionResult CurrentDateList_Server()
        {
            var date = Request["WHC_DteAccount"];

            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Account_GetDepositDetail(userid, date.ToDateTime(), date.ToDateTime());
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
    }
}
