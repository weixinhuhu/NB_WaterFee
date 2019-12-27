using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    /// <summary>
    /// 算费菜单
    /// </summary>
    public class CountFeeController : BaseController
    {
        //审核抄表数据
        public ActionResult ApproveData()
        {
            return View();
        }

        public ActionResult CountReleaseFee()
        {
            return View();
        }

        //预存款销账
        public ActionResult DepositAccount()
        {
            return View();
        }
    }
}
