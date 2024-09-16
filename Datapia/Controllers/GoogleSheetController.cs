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
using System.IO;
using System.Reflection;

namespace Datapia.Controllers
{
    public class GoogleSheetController : Controller
    {
        public ActionResult Index()
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                return View();
            }
            Common.SaveSession("GoogleSheet", "Index");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult InsertConfig(string spreadsheet_id, HttpPostedFileBase files)
        {
            try
            {
                if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
                {
                    DateTimeOffset timestamp = DateTimeOffset.UtcNow;
                    long unixTimestampMs = timestamp.ToUnixTimeMilliseconds();
                    var userid = Session["usercode"].ToString();
                    string fileName = Path.GetFileName(files.FileName.Replace(".json", "_" + userid + "_" + unixTimestampMs + ".json"));
                    string data_json = "";
                    string path = Path.Combine(Server.MapPath("~/Data"), fileName);

                    // Create Data folder if it doesn't exist
                    if (!Directory.Exists(Server.MapPath("~/Data")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Data"));
                    }

                    // Save the file to the Data folder
                    files.SaveAs(path);
                    var query_insert = BaseConnectionSql.Execute_Update_Insert_V1("strConn", "sp_googlesheet_insert", userid, spreadsheet_id, path);
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
                LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "InsertUpdateConfig");
                return Json(null);
            }
        }

        public ActionResult GetListConfig()
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                var userid = Session["usercode"].ToString();
                DataTable table = BaseConnectionSql.ExecuteDataTable_V1("strConn", "sp_googlesheet_get", userid);

                return PartialView("~/Views/GoogleSheet/Partial/__Index.cshtml", table);
            }
            Common.SaveSession("GoogleSheet", "Index");
            return RedirectToAction("Login", "Account");
        }

        public async Task<ActionResult> Export_sheet(int id)
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                var query = await BaseApiClient.CallAsync_FastApi<ResponseModel>("http://127.0.0.1:8001", "TokenAPI", "/google_sheet?id=" + id);

                return Json(query);
            }
            Common.SaveSession("GoogleSheet", "Index");
            return RedirectToAction("Login", "Account");
            //return View();
        }
    }
}