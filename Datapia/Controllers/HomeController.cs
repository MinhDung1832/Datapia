using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datapia.DataAccess;
using Datapia.Models;
using System.Data;
using System.Threading.Tasks;

namespace Datapia.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Overview()
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                var query = await BaseApiClient.CallAsync_FastApi<objCombox>("http://127.0.0.1:8000", "TokenAPI", "/users/");
                var result = query;
                return View();
            }
            Common.SaveSession("Home", "Overview");
            return RedirectToAction("Login", "Account");
            //return View();
        }

        public ActionResult GetListConfig(string data_from = "", string acc_id_val = "", string status = "")
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                var userid = Session["usercode"].ToString();
                DataTable table = BaseConnectionSql.ExecuteDataTable_V1("strConn", "sp_config_get_list", userid, data_from, acc_id_val, status);

                return PartialView("~/Views/Home/Partial/__Overview.cshtml", table);
            }
            Common.SaveSession("Home", "Overview");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult GetConfigDetail(int id)
        {
            try
            {
                if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
                {
                    var detail = BaseConnectionSql.ExecuteList_V1<config_detail>("strConn", "sp_config_get_detail", Session["usercode"].ToString(), id);
                    return Json(detail[0]);
                }
                Common.SaveSession("Home", "Overview");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "GetConfigDetail");
                return Json(null);
            }
        }

        public ActionResult InsertUpdateConfig(get_data lst)
        {
            try
            {
                if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
                {
                    lst.userid = Session["usercode"].ToString();
                    if (lst.id <= 0)
                    {
                        var query_insert = BaseConnectionSql.Execute_Update_Insert("strConn", "sp_config_insert", lst);
                        if (query_insert)
                        {
                            return Json(new { code = 1 });
                        }
                        return Json(new { code = 3 });
                    }
                    else
                    {
                        var query_update = BaseConnectionSql.Execute_Update_Insert("strConn", "sp_config_update", lst);
                        if (query_update)
                        {
                            return Json(new { code = 2 });
                        }
                        return Json(new { code = 4 });
                    }
                }
                Common.SaveSession("Home", "Overview");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "InsertUpdateConfig");
                return Json(null);
            }
        }

        public ActionResult DeleteConfig(int id)
        {
            try
            {
                if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
                {
                    var userid = Session["usercode"].ToString();
                    var query_insert = BaseConnectionSql.Execute_Update_Insert_V1("strConn", "sp_config_delete", userid, id);
                    if (query_insert)
                    {
                        return Json(new { code = 1 });
                    }
                    return Json(new { code = 3 });
                }
                Common.SaveSession("Home", "Overview");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "DeleteConfig");
                return Json(null);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}