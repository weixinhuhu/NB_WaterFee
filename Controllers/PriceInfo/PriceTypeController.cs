using NB_WaterFee.DbServiceReference;
using System;
using System.Web.Mvc;
using WHC.NB_WaterFee.Controllers;

namespace WHC.WaterFeeWeb.Controllers
{
    public class PriceTypeController : BaseController
    {
        // GET: CustomerInfo
        public ActionResult List()
        {
        
            return View();
        }

        public ActionResult ListJson_Server()
        {
            var dts = new ServiceDbClient().PriceType_Qry(endcode);
            return ToJsonContentDate(dts);
        }
     
        public ActionResult GetTreeJson_Server()
        {
            var dts = new ServiceDbClient().PriceType_GetTreeJson(endcode);
            return ToJsonContentDate(dts);
        }

        public ActionResult Insert_Server(PriceType info)
        {         
            CommonResult result = new CommonResult();
            try { 
                var flag = new ServiceDbClient().PriceType_Ins(endcode.ToString().ToInt(), info);
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
                //LogTextHelper.Error(ex);//错误记录              
            }
            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult Update_Server(PriceType info)
        {
            CommonResult result = new CommonResult();
            try
            {
                var flag = new ServiceDbClient().PriceType_Upd(info);
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
              //  LogTextHelper.Error(ex);//错误记录 
                result.ErrorMsg = ex.Message;
            }
            return ToJsonContent(result);
        }

        // GET: CustomerInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerInfo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerInfo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerInfo/Edit/5
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
    }
}
