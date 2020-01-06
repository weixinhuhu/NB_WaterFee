using NB_WaterFee.DbServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Linq;

using System.Web.Mvc;
using WHC.Framework.Commons;

namespace WHC.NB_WaterFee.Controllers
{
    public class ArcMeterInfoController : BaseController
    {
        // GET: CustomerInfo
        public ActionResult List()
        {
           return View();
        }

        public ActionResult ChangeTB()
        {
            return View();
        }

        public ActionResult MeterSettingInfo()
        {
            return View();
        }
        public ActionResult ManagerList()
        {
            return View();
        }

        public ActionResult RemoteFunction()
        {
            return View();
        }
        public ActionResult SwitchValveTask()
        {
            return View();
        }

        public ActionResult ListJson_Server()
        {
            var Strlevel = Request["WHC_Treelevel"];
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var ParentText = Request["WHC_TreePrentText"];
            var QryCondi = new MeterReplaceQryCondition()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                NvcAddr = Request["WHC_NvcAddr"] ?? "",
                IntCustNo = (Request["IntCustNO"] ?? "0").ToString().ToIntOrZero(),
                VcMeterAddr = Request["WHC_VcAddr"] ?? ""
            };

            if (Strlevel == "1")
            {
                QryCondi.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                QryCondi.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                QryCondi.NvcVillage = fuji;
                QryCondi.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                QryCondi.NvcVillage = ParentText;
                QryCondi.VcBuilding = fuji;
                QryCondi.VcUnitNum = Text;
            }

            QryCondi.IntEndNo = endcode;
                  
            var dts = new ServiceDbClient().GetMeterReplaceList(QryCondi);

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
  
        //设置参数
        public ActionResult SettingMangger_Server()
        {
            var Strlevel = Request["WHC_Treelevel"];
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var ParentText = Request["WHC_TreePrentText"];
            var custormerinfo = new Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                NvcAddr = Request["WHC_NvcAddr"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? ""
            };
            var useno = Request["WHC_IntNo"] ?? "0";
            custormerinfo.IntNo = useno.ToIntOrZero();

            if (Strlevel == "1")
            {
                custormerinfo.NvcVillage = "所有小区";
            };

            if (Strlevel == "2")
            {
                custormerinfo.NvcVillage = Text;
            }

            if (Strlevel == "3")
            {
                custormerinfo.NvcVillage = fuji;
                custormerinfo.VcBuilding = Text;
            }

            if (Strlevel == "4")
            {
                custormerinfo.NvcVillage = ParentText;
                custormerinfo.VcBuilding = fuji;
                custormerinfo.VcUnitNum = Text;
            }

            //调用后台服务获取集中器信息
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Terminal_GetMeterSetting(endcode, custormerinfo, new Meter());

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

        public ActionResult FindByIntCustNo_Server(string IntCustNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                result = new ServiceDbClient().FindByIntCustNo(IntCustNo);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMsg = ex.Message;
            }
            return ToJsonContent(result);
        }
        
        /// <summary>
        /// 查询参数信息 用于前端下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetParam_MeterConfigTreeJson_Server()
        {
            var tree = new ServiceDbClient().Param_MeterConfig_GetTree(endcode);
            return ToJsonContentDate(tree);
        }
        /// <summary>
        /// 查询参数信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Param_MeterConfig_Qry()
        {
         
            var dts = new ServiceDbClient().Param_MeterConfig_Qry(endcode);
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

        public ActionResult Param_MeterConfig_Ins(MeterConfig MeterConf)
        {
            CommonResult result = new CommonResult();
            MeterConf.IntID = 0;
            MeterConf.VcUserID = userid.ToString();
            MeterConf.VcUserIDUpd = userid.ToString();
            MeterConf.IntEndCode = endcode;
            MeterConf.DtCreate = DateTime.Now;
            var rs = new ServiceDbClient().Param_MeterConfig_Opr(MeterConf);
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
        public ActionResult Param_MeterConfig_Upd(MeterConfig MeterConf)
        {
            CommonResult result = new CommonResult();
            MeterConf.VcUserIDUpd =userid.ToString();
            MeterConf.IntEndCode =endcode;
            MeterConf.DtLstUpd = DateTime.Now;
            var rs = new ServiceDbClient().Param_MeterConfig_Opr(MeterConf);
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
        /// 设置参数
        /// </summary>
        /// <param name="sAddr"></param>
        /// <param name="ChrAllowUsed"></param>
        /// <param name="ChrFreezeDay"></param>
        /// <param name="ChrValveMaint"></param>
        /// <param name="ChrUpTiming"></param>
        /// <param name="ChrUpTimingUnit"></param>
        /// <param name="ChrUpAmount"></param>
        /// <returns></returns>
        public ActionResult SettingMeterInfo_Server(String sAddr, String sMeterInfoTypeNo)
        {
            CommonResult result = new CommonResult();
            var meterSettingtypeno = sMeterInfoTypeNo.ToIntOrZero();
            try
            {
                var rs = new ServiceDbClient().Terminal_SetMeterConfig(endcode, sAddr, meterSettingtypeno);
                if (rs == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = rs;
                }
            }
            catch (Exception ex)
            {
                var err = ex.ToString();
                result.IsSuccess = false;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 从十进制转换到十六进制
        /// </summary>
        /// <param name="ten"></param>
        /// <returns></returns>
        public static string Ten2Hex(string ten)
        {
            ulong tenValue = Convert.ToUInt64(ten);
            ulong divValue, resValue;
            string hex = "";
            do
            {
                //divValue = (ulong)Math.Floor(tenValue / 16);

                divValue = (ulong)Math.Floor((decimal)(tenValue / 16));

                resValue = tenValue % 16;
                hex = tenValue2Char(resValue) + hex;
                tenValue = divValue;
            }
            while (tenValue >= 16);
            if (tenValue != 0)
                hex = tenValue2Char(tenValue) + hex;
            return hex;
        }

        public static string tenValue2Char(ulong ten)
        {
            switch (ten)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return ten.ToString();
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                default:
                    return "";
            }
        }
        public ActionResult ChangeTBL_Server(MeterReplaceInfo MeterReplace)
        {
            CommonResult result = new CommonResult();
            MeterReplace.VcUserID = userid.ToString();
            MeterReplace.IntEndCode = 0;
            try
            {
                var flg = new ServiceDbClient().ArcMeter_Replace(MeterReplace);
                if (flg == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = flg;                  
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return ToJsonContent(result);
        }
    }
}
