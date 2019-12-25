using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["FullName"] != null)
            {
                ViewBag.FullName = Session["FullName"].ToString();
                ViewBag.Name = Session["UserID"].ToString();
                ViewBag.HeaderScript = "收费系统";//一级菜单代码
                //公司名称
                ViewBag.AppCompanyName = "售水收费系统";
            }
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Online()
        {
            return View();
        }

    }
}
