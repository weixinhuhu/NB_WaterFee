using NB_WaterFee.DbServiceReference;
using System;
using System.Data;
using System.Web.Mvc;

namespace WHC.NB_WaterFee.Controllers
{
    public class FeeController : BaseController
    {
        //柜台收费
        public ActionResult CounterPay()
        {
            return View();
        }

        //结清欠款销户
        public ActionResult CloseAccount()
        {
            return View();
        }

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
        public ActionResult TodayBalance()
        {
            return View();
        }

        public ActionResult PaymentNotice()
        {
            return View();
        }
        object objLock = new object();

        /// <summary>
        /// 查看扣费记录
        /// </summary>
        /// <returns></returns>       
        public ActionResult GetAccDebtByIntCustNo_Server()
        {
            var custno = Request["IntCustNo"] ?? "0";
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Account_GetDebtByCustNo(endcode, custno.ToIntOrZero());
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

        /// <summary>
        /// 获取未收财务
        /// </summary>
        /// <returns></returns>  
        public ActionResult GetPaymentNoticeList_Server()
        {
            var CustNo = Request["WHC_IntCustNo"] ?? "0";
            var NvcName = Request["WHC_NvcName"] ?? "";
            var NvcAddr = Request["WHC_NvcAddr"] ?? "";
            var VcMobile = Request["WHC_VcMobile"] ?? "";
            var custinfo = new Customer
            {
                IntNo = CustNo.ToIntOrZero(),
                NvcName = NvcName,
                NvcAddr = NvcAddr,
                VcMobile = VcMobile
            };
            var dt = new ServiceDbClient().Account_GetPaymentNotice(endcode, custinfo);
            var result = new { total = dt.Rows.Count, rows = dt };
            return ToJsonContentDate(result);
        }


        /// <summary>
        /// 存取款
        /// </summary>
        /// <param name="custNo">客户编号</param>
        /// <returns></returns>
        public ActionResult PayGetMoney_Server(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                var payMoney = Request["payMoney"] ?? "0";
                var sRemark = "";
                var iUserID = userid;
                var sReceiptNo = "";
                ServiceDbClient DbServer = new ServiceDbClient();
                var flag = DbServer.Account_DepositOperate(endcode, custNo.ToIntOrZero(), payMoney.ToDouble(), sRemark, iUserID, sReceiptNo);
                if (flag.IsSuccess)
                {
                    result.IsSuccess = true;
                    result.StrData1 = flag.StrData1;
                }
                else
                {
                    result.ErrorMsg = flag.ErrorMsg;
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return ToJsonContentDate(result);
        }

        [HttpPost]
        public ActionResult CloseAccount_Query_Server(string custNo)
        {
            CommonResult result = new CommonResult();
            var intcustno = custNo ?? "0";
            try
            {
                var rs = new ServiceDbClient().Account_GetBillByCustNo(endcode, intcustno.ToIntOrZero());
                if (rs.IsSuccess)
                {
                    if (rs.Tbl1.Rows.Count > 0)
                    {
                        result.StrData1 = Newtonsoft.Json.JsonConvert.SerializeObject(rs.Tbl1);
                        var dt = new { total = rs.Tbl2.Rows.Count, rows = rs.Tbl2 };
                        result.IsSuccess = true;
                        // result.LstObj = dt;
                        result.StrData2 = rs.Tbl2.Rows.Count.ToString();
                        //result.Data2 = "2";
                    }
                    else
                    {
                        result.ErrorMsg = "未查询到用户号为：【" + custNo + "】 的用户档案";
                    }
                }
                else
                {
                    result.ErrorMsg = "查询欠费失败!错误如下:" + rs.ErrorMsg;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = "查询欠费失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 销户
        /// </summary>
        /// <param name="custNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CloseAccount_Close_Server(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                var intcustno = custNo ?? "0";
                var flag = new ServiceDbClient().ArcCloseAccount(endcode, intcustno.ToIntOrZero());

                if (flag == "0")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMsg = "操作失败! " + flag;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = "操作失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }


        public ActionResult PaymentNoticeExport()
        {
            var CustNo = Request["WHC_IntCustNo"] ?? "0";
            var NvcName = Request["WHC_NvcName"];
            var NvcAddr = Request["WHC_NvcAddr"];
            var VcMobile = Request["WHC_VcMobile"];
            var custinfo = new Customer
            {
                IntNo = CustNo.ToIntOrDefault(),
                NvcName = NvcName,
                NvcAddr = NvcAddr,
                VcMobile = VcMobile
            };
            var dt = new ServiceDbClient().Account_GetPaymentNotice(endcode, custinfo);
            if (dt.Rows.Count > 0)
            {
                //导出目录创建与清空
                var root = Server.MapPath("~\\");
                var dir = new System.IO.DirectoryInfo(root + "temp\\");
                if (dir.Exists == false) dir.Create();
                try
                {
                    foreach (var item in dir.GetFiles())
                    {
                        item.Delete();
                    }
                }
                catch { }
                var filename = dir + Guid.NewGuid().ToString() + ".xls";
                var ds = new System.Data.DataSet();
                ds.Tables.Add(dt);
                // ExcelHelper.DataSetToExcel(ds, filename);
                return Redirect(filename.Replace(root, "/").Replace("\\", "/"));
            }
            return View();
        }

        public ActionResult PrintTicket()
        {
            return View();
        }
        /// <summary>
        ///  打印存取款收据
        /// </summary>           
        public ActionResult PayGetMoneyPrint(string flowNo)
        {
            var dt = new ServiceDbClient().Account_GetDepositInvoiceInfo(endcode, flowNo);
            if (dt.Rows.Count > 0)
            {
                ViewBag.IntCustNo = dt.Rows[0]["IntCustNo"].ToString();
                ViewBag.NvcName = dt.Rows[0]["NvcName"].ToString();
                ViewBag.VcRoomNum = dt.Rows[0]["VcRoomNum"].ToString();
                ViewBag.NvcVillage = dt.Rows[0]["NvcVillage"].ToString();
                ViewBag.NvcAddr = dt.Rows[0]["NvcAddr"].ToString();
                ViewBag.VcType = dt.Rows[0]["VcType"].ToString();
                var MonAmount = dt.Rows[0]["MonAmount"].ToString().ToDouble();
                ViewBag.MonAmount = MonAmount.ToString("#0.00");
                ViewBag.DteAccount = dt.Rows[0]["DteAccount"].ToString();
                ViewBag.VcFlowNo = dt.Rows[0]["VcFlowNo"].ToString();
                ViewBag.UserName = dt.Rows[0]["UserName"].ToString();
            }
            return View();
        }
        /// <summary>
        /// 打印扣费信息
        /// </summary>
        /// <param name="IntFeeID">客户编号</param>
        /// <returns></returns>
        public ActionResult PrintTicketDetail(int IntFeeID)
        {
            var DtStart = Request["WHC_DtStart"] ?? DateTime.Now.ToString();
            var Dtend = Request["WHC_DtEnd"] ?? DateTime.Now.ToString(); ;
            var custinfo = new Customer();
            var dt = new ServiceDbClient().Account_GetPaymentDetail(endcode, IntFeeID, DtStart.ToDateTime(), Dtend.ToDateTime(), custinfo);
            if (dt.Rows.Count > 0)
            {
                ViewBag.IntCustNo = dt.Rows[0]["IntCustNo"].ToString();
                ViewBag.NvcName = dt.Rows[0]["NvcName"].ToString();
                ViewBag.VcRoomNum = dt.Rows[0]["VcRoomNum"].ToString();
                ViewBag.NvcVillage = dt.Rows[0]["NvcVillage"].ToString();
                ViewBag.NvcAddr = dt.Rows[0]["NvcAddr"].ToString();
                ViewBag.IntYearMon = dt.Rows[0]["IntYearMon"].ToString();
                ViewBag.DteFee = dt.Rows[0]["DteFee"].ToString();
                var MonFee = dt.Rows[0]["MonFee"].ToString().ToDouble();
                ViewBag.MonFee = MonFee.ToString("#0.00");
                var MonPenalty = dt.Rows[0]["MonPenalty"].ToString().ToDouble();
                ViewBag.MonPenalty = MonPenalty.ToString("#0.00"); ;
                ViewBag.IntDays = dt.Rows[0]["IntDays"].ToString();
                ViewBag.VcFlowNo = dt.Rows[0]["VcFlowNo"].ToString();
                ViewBag.IntPayUnit = dt.Rows[0]["IntPayUnit"].ToString();
                ViewBag.VcChargeNo = dt.Rows[0]["VcChargeNo"].ToString();
            }
            return View();
        }
    }
}
