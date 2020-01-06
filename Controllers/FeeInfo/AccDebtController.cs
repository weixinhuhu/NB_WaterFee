using NB_WaterFee.DbServiceReference;
using System;
using System.Web.Mvc;
using WHC.Framework.Commons;

namespace WHC.NB_WaterFee.Controllers
{
    public class AccDebtController : BaseController
    {

        object objLock = new object();
        //柜台收费
        [HttpPost]

        //柜台冲正
        public ActionResult CounterReverse()
        {
            return View();
        }

        //存取预存款
        public ActionResult PayGetMoney()
        {

            return View();
        }

        //结清欠款销户
        public ActionResult CloseAccount()
        {
            return View();
        }
        public ActionResult PaymentNotice()
        {
            return View();
        }

        public ActionResult TodayBalance()
        {
            return View();
        }


        public ActionResult PaymentNoticeExport()
        {
            var CustNo = Request["WHC_IntCustNo"] ?? "0";
            var NvcName = Request["WHC_NvcName"];
            var NvcAddr = Request["WHC_NvcAddr"];
            var VcMobile = Request["WHC_VcMobile"];
            var custinfo = new Customer
            {
                IntNo = CustNo.ToIntOrDefault(),
                NvcName = NvcName,
                NvcAddr = NvcAddr,
                VcMobile = VcMobile
            };
            var dt = new ServiceDbClient().Account_GetPaymentNotice(endcode, custinfo);
            if (dt.Rows.Count > 0)
            {
                //导出目录创建与清空
                var root = Server.MapPath("~\\");
                var dir = new System.IO.DirectoryInfo(root + "temp\\");
                if (dir.Exists == false) dir.Create();
                try
                {
                    foreach (var item in dir.GetFiles())
                    {
                        item.Delete();
                    }
                }
                catch { }
                //var filename = dir + Guid.NewGuid().ToString() + ".xls";
                var filename = dir + "催费通知单"+ DateTime.Now.ToString("D") + ".xls";
                var ds = new System.Data.DataSet();
                ds.Tables.Add(dt);
                ExcelHelper.DataSetToExcel(ds, filename);
                return Redirect(filename.Replace(root, "/").Replace("\\", "/"));
            }
            return View();
        }
    }
}
