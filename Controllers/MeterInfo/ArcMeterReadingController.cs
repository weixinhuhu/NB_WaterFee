using NB_WaterFee.DbServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;

namespace WHC.NB_WaterFee.Controllers
{
    public class ArcMeterReadingController : BaseController
    { 
        // GET: ArcMeterReading
        public ActionResult List()
        {
            return View();
        }

        public ActionResult CollectChart()
        {
            return View();
        }
        
        public ActionResult CollectStat()
        {
            return View();
        }

        public ActionResult Insert()
        {
            return View();
        }

        public ActionResult ChangeTable()
        {
            return View();
        }

        public ActionResult ManualMeterReading()
        {
            return View();
        }

        public ActionResult ListJson_Server()
        {
            var WHC_StartDteFreeze = Request["WHC_StratDteFreeze"].ToDateTime();
            var WHC_EndDteFreeze = Request["WHC_EndDteFreeze"].ToDateTime();
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var Strlevel = Request["WHC_Treelevel"];
            var ParentText = Request["WHC_TreePrentText"];
            var customerinfo = new Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? "",
            };
            var custno = Request["WHC_IntCustNo"] ?? "0";
            customerinfo.IntNo = custno == "" ? 0 : custno.ToInt();

            if (Strlevel == "1")
            {
                customerinfo.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                customerinfo.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                customerinfo.NvcVillage = fuji;
                customerinfo.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                customerinfo.NvcVillage = ParentText;
                customerinfo.VcBuilding = fuji;
                customerinfo.VcUnitNum = Text;
            }
            //调用后台服务获取集中器信息          
            var dts = new ServiceDbClient().CollectData_Qry(endcode, customerinfo, WHC_StartDteFreeze, WHC_EndDteFreeze);

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
            var result = new { total = total, rows = dat };
            return ToJsonContentDate(result);
        }
        
        public ActionResult CollectStatJson_Server()
        {
            var WHC_StartDteFreeze = Request["WHC_StratDteFreeze"].ToDateTime();
            var WHC_EndDteFreeze = Request["WHC_EndDteFreeze"].ToDateTime();
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var Strlevel = Request["WHC_Treelevel"];
            var ParentText = Request["WHC_TreePrentText"];
            var customerinfo = new Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? "",
            };
            var custno = Request["WHC_IntCustNo"] ?? "0";
            customerinfo.IntNo = custno == "" ? 0 : custno.ToInt();

            if (Strlevel == "1")
            {
                customerinfo.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                customerinfo.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                customerinfo.NvcVillage = fuji;
                customerinfo.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                customerinfo.NvcVillage = ParentText;
                customerinfo.VcBuilding = fuji;
                customerinfo.VcUnitNum = Text;
            }
            //调用后台服务获取集中器信息          
            var dts = new ServiceDbClient().CollectStatus_Qry(endcode, customerinfo, WHC_StartDteFreeze, WHC_EndDteFreeze);

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
            var result = new { total = total, rows = dat };
            return ToJsonContentDate(result);
        }
        public ActionResult CollectStatJsonTotal_Server()
        {
            CommonResult result = new CommonResult();
            var WHC_StartDteFreeze = Request["WHC_StratDteFreeze"].ToDateTime();
            var WHC_EndDteFreeze = Request["WHC_EndDteFreeze"].ToDateTime();
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var Strlevel = Request["WHC_Treelevel"];
            var ParentText = Request["WHC_TreePrentText"];
            var customerinfo = new Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? "",
            };
            var custno = Request["WHC_IntCustNo"] ?? "0";
            customerinfo.IntNo = custno == "" ? 0 : custno.ToInt();

            if (Strlevel == "1")
            {
                customerinfo.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                customerinfo.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                customerinfo.NvcVillage = fuji;
                customerinfo.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                customerinfo.NvcVillage = ParentText;
                customerinfo.VcBuilding = fuji;
                customerinfo.VcUnitNum = Text;
            }
            //调用后台服务获取集中器信息          
            var dts = new ServiceDbClient().CollectStatus_Qry(endcode, customerinfo, WHC_StartDteFreeze, WHC_EndDteFreeze);
            var m = new CollectStatModel();
            foreach (DataRow item in dts.Rows)
            {
                m.Used += item["Reading"].ToString().ToDecimalOrZero();
            }
            result.StrData1 = dts.Rows.Count.ToString();
            result.StrData2 = m.Used.ToString();
            return ToJsonContentDate(result);
        }

        public class CollectStatModel
        {
            public int Count { get; set; }
            public decimal Used { get; set; }
        }  
   
        [HttpPost]
        public ActionResult WriteNumReading_Server()
        {
            CommonResult result = new CommonResult();        
            var IntCustNo = Request["IntCustNo"];
            var VcAddr = Request["VcAddr"];
            var DteFreeze = Request["DteFreeze"].ToDateTime();
            var NumReading = Request["NumReading"].ToDouble();
            var rs = new ServiceDbClient().CollectData_Ins(endcode, IntCustNo.ToInt(), VcAddr, DteFreeze, NumReading, userid.ToString());
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
