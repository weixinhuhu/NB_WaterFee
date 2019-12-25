using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;

namespace NewWaterFee.Controllers.Security
{
    public class SysManageController : BaseController
    {
        // GET: SysManage
        public ActionResult AutoSwitch()
        {
            return View();
        }

        public ActionResult Setting()
        {
            return View();
        }
    }
}