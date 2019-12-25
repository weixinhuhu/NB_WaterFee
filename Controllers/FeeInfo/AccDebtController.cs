using System.Web.Mvc;
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



    }
}
