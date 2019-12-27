using NB_WaterFee.DbServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;

namespace NB_WaterFee.Controllers.CustomerInfo
{
    public class ArcCustomerInfoController : BaseController
    {
        // GET: ArcCustomerInfo
        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListJson_Server()
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
            custormerinfo.IntNo = (Request["WHC_IntNo"] ?? "0").ToString().ToInt();

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
            var dts = DbServer.ArcCustomer_Qry(endcode, custormerinfo);

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

        public ActionResult Insert_Server(Customer CustomerInfo, Meter MeterInfo)
        {       
            CommonResult result = new CommonResult();
            try
            {
                var cInfo = Request["CustomerInfo"];
                var mInfo = Request["MeterInfo"];
                var setting = new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
                CustomerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(cInfo, setting);
                MeterInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Meter>(mInfo, setting);
                //CustomerInfo = ReflectionHelper.ReplacePropertyValue(CustomerInfo, typeof(string), null, string.Empty);
                //MeterInfo = ReflectionHelper.ReplacePropertyValue(MeterInfo, typeof(string), null, string.Empty);
                CustomerInfo.IntUserID = userid;
                CustomerInfo.IntStatus = 1;
                CustomerInfo.DteOpen = DateTime.Now;
                CustomerInfo.DteCancel = DateTime.Now;
                MeterInfo.IntCustNO = CustomerInfo.IntNo;
                CustomerInfo.VcAddrCode = "";
                CustomerInfo.VcNameCode = "";    
                CustomerInfo.IntEndCode = endcode.ToString().ToInt();
                MeterInfo.IntEndCode = endcode.ToString().ToInt();
                CustomerInfo.VcWechatNo = "";
                var flg = new ServiceDbClient().ArcCustMeter_Ins(CustomerInfo, MeterInfo);
                if (flg == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = flg;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return ToJsonContent(result);
        }
        public ActionResult Update_Server(Customer CustomerInfo, Meter MeterInfo)
        {
            CommonResult result = new CommonResult();
            try
            {
                var cInfo = Request["CustomerInfo"];
                var mInfo = Request["MeterInfo"];
                var setting = new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
                CustomerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(cInfo, setting);
                MeterInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Meter>(mInfo, setting);
                //CustomerInfo = ReflectionHelper.ReplacePropertyValue(CustomerInfo, typeof(string), null, string.Empty);
                //MeterInfo = ReflectionHelper.ReplacePropertyValue(MeterInfo, typeof(string), null, string.Empty);

                CustomerInfo.IntUserID = userid;
                CustomerInfo.IntStatus = 1;

                CustomerInfo.DteCancel = DateTime.Now;
                CustomerInfo.DteOpen = DateTime.Now;
                MeterInfo.DtCreate = DateTime.Now;
                MeterInfo.DtOnline = DateTime.Now;

                MeterInfo.IntCustNO = CustomerInfo.IntNo;
                CustomerInfo.VcAddrCode ="";
                CustomerInfo.VcNameCode = "";
                CustomerInfo.IntEndCode = endcode;
                MeterInfo.IntEndCode = endcode;
                CustomerInfo.VcWechatNo = "";

                var flg = new ServiceDbClient().ArcCustMeter_Upd(CustomerInfo, MeterInfo);
                if (flg == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = flg;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return ToJsonContent(result);
        }
       
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DictAccountWay_Server()
        {
            ServiceDbClient DbServer = new ServiceDbClient();
            var dt = DbServer.GetDictAccountWay();
            return ToJsonContentDate(dt);
        }

        public ActionResult IntAutoSwitch_Server()
        {
            ServiceDbClient DbServer = new ServiceDbClient();
            var list = DbServer.GetDictValveAuto();
            return ToJsonContentDate(list);
        }
        public ActionResult IntReplaceType_Server()
        {
            ServiceDbClient DbServer = new ServiceDbClient();
            var list = DbServer.GetDictMeterReplaceType();
            return ToJsonContentDate(list);
        }
        public ActionResult TreeCommunity_Server()
        {        
            var treelist = new ServiceDbClient().ArcCustomer_TreeCommunity(endcode);
            return ToJsonContentDate(treelist);
        }
    }
}