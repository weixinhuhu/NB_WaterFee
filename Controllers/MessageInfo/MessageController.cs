using NB_WaterFee.DbServiceReference;
using System;
using System.Data;
using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;

namespace NewWaterFee.Controllers.MessageInfo
{
    public class MessageController : BaseController
    {
        public ActionResult TemplateSetting()
        {
            return View();
        }
        public ActionResult TemplateList_Server()
        {
            var Status = Request["WHC_IntStatus"] ?? "-1";
            var id = Request["WHC_IntID"] ?? "0";
            var SmsTemplate_info = new SmsTemplate
            {
                NvcName = Request["WHC_TempName"] ?? "",
                IntID = id == "" ? 0 : Convert.ToInt32(id),
                IntStatus = Status == "" ? 0 : Convert.ToInt32(Status)
            };
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.SMS_Template_Qry(endcode, SmsTemplate_info);
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
        public ActionResult InsOrUpd_Server(SmsTemplate info)
        {
            CommonResult result = new CommonResult();
            var endcode = Session["EndCode"].ToString() ?? "0";
            info.IntEndCode = endcode.ToIntOrZero();
            try
            {
                ServiceDbClient DbServer = new ServiceDbClient();
                var flag = DbServer.SMS_Template_InsOrUpd(info);
                info.DtLstUpd = DateTime.Now;
                if (flag == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = flag;
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