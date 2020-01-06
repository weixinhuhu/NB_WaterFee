using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    /// <summary>
    /// 所有需要进行登录控制的控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        //客户代码
        public int endcode;
        //操作员编号
        public int userid;
        //操作员名称
        public string username;

        #region 异常处理及记录
        /// <summary>
        /// 重新基类在Action执行之前的事情
        /// </summary>
        /// <param name="filterContext">重写方法的参数</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            endcode = (Session["EndCode"] ?? "0").ToString().ToIntOrZero();
            userid = (Session["UserID"] ?? "0").ToString().ToIntOrZero();
            username = (Session["FullName"] ?? "").ToString();
            if (Session["FullName"] == null)
            {
                Response.Redirect("/Login/Index");//如果用户为空跳转到登录界面
            }
        }
        #endregion

        #region 辅助函数

        /// <summary>
        /// 生成GUID的服务器方法
        /// </summary>
        /// <returns></returns>
        public ActionResult NewGuid()
        {
            string guid = System.Guid.NewGuid().ToString();
            return Content(guid);
        }

        /// <summary>
        /// 把object对象转换为ContentResult
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ContentResult ToJsonContent(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Content(result);
        }

        /// <summary>
        /// 返回处理过的时间的Json字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ContentResult ToJsonContentDate(object date)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return Content(JsonConvert.SerializeObject(date, Formatting.Indented, timeConverter));
        }

        public ContentResult Content(bool result)
        {
            return Content(result.ToString().ToLower());//小写方便脚本处理
        }

        public ContentResult Content(int result)
        {
            return Content(result.ToString());
        }

        /// <summary>
        /// 把对象为json字符串
        /// </summary>
        /// <param name="obj">待序列号对象</param>
        /// <returns></returns>
        protected string ToJson(object obj)
        {
            //使用Json.NET的序列号类，能够更加高效、完美
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string sActionType = context.Request.Params.Get("ActionType") == null ? "" : context.Request["ActionType"].Trim();
            if (sActionType.Equals("EasyUIDataGridToExcle"))
            {
                #region 將EasyUI的DataGrid中的數據 導出Excle 
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.Charset = "utf-8";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.AppendHeader("content-disposition", "attachment;filename=\"" + HttpUtility.HtmlEncode(context.Request["txtName"] ?? DateTime.Now.ToString("yyyyMMdd")) + ".xls\"");
                context.Response.ContentType = "Application/ms-excel";
                context.Response.Write("<html>\n<head>\n");
                context.Response.Write("<style type=\"text/css\">\n.pb{font-size:13px;border-collapse:collapse;} " +
                               "\n.pb th{font-weight:bold;text-align:center;border:0.5pt solid windowtext;padding:2px;} " +
                               "\n.pb td{border:0.5pt solid windowtext;padding:2px;}\n</style>\n</head>\n");
                context.Response.Write("<body>\n" + context.Request["txtContent"] + "\n</body>\n</html>");
                context.Response.Flush();
                context.Response.End();
                #endregion
            }
        }
    }
}
