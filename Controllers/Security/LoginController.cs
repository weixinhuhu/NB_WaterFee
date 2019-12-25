using NB_WaterFee.ServiceReference1;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace WHC.MVCWebMis.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 第一种登陆界面
        /// </summary>
        public ActionResult Index()
        {
            Session.Clear();

            return View();
        }

        /// <summary>
        /// 对用户登录的操作进行验证
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public ActionResult CheckUser_Server(string username, string password, string code)
        {
            CommonResult result = new CommonResult();

            bool codeValidated = true;
            if (this.TempData["ValidateCode"] != null)
            {
                codeValidated = (this.TempData["ValidateCode"].ToString() == code);
            }

            if (string.IsNullOrEmpty(username))
            {
                result.ErrorMsg = "用户名不能为空";
            }
            else if (!codeValidated)
            {
                result.ErrorMsg = "验证码输入有误";
            }
            else
            {
                try
                {
                    string ip = GetClientIp();
                    string macAddr = "";
                    var info = new AuthorityClient().Sys_Login_CheckUser(username, password, ip, macAddr);
                    if (!String.IsNullOrEmpty(info.Identity))
                    {
                        result.IsSuccess = true;
                        #region 取得用户的授权信息，并存储在Session中             
                        Session["FullName"] = info.FullName;
                        Session["UserID"] = info.IntID;
                        Session["Company_ID"] = info.IntComID;
                        Session["Dept_ID "] = info.IntDeptID;
                        Session["UserInfo"] = info;
                        Session["IsSuperAdmin"] = info.IntComID == 1 ? true : false;
                        Session["EndCode"] = info.IntComID;
                        #endregion                      
                    }
                    else
                    {
                        result.ErrorMsg = "用户名输入错误或者您已经被禁用";
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("登录失败"))
                        result.ErrorMsg = "数据库账号或密码错误,请检查数据库是否正常";
                    else
                        result.ErrorMsg = ex.Message;
                }
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        private string GetClientIp()
        {
            //可以透过代理服务器
            string userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIP))
            {
                //没有代理服务器,如果有代理服务器获取的是代理服务器的IP
                userIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(userIP))
            {
                userIP = Request.UserHostAddress;
            }

            //替换本机默认的::1
            if (userIP == "::1")
            {
                userIP = "127.0.0.1";
            }

            return userIP;
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
    }
}
