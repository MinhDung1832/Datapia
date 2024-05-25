using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Datapia.DataAccess;
using Datapia.Models;
using DataAccess;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Data;
using System.Net.Mail;

namespace Datapia.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            Session["err"] = "";
            if (Session["all_us_role"] != null)
            {
                return RedirectToAction("Overview", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection ip_form)
        {
            var uid = ip_form["UserId"];
            var pwd = ip_form["Password"];
            var ip = ip_form["IsChekIPV4"];
            var query = BaseConnectionSql.ExecuteList_V1<user_login>("strConn", "sp_user_login", uid.ToUpper(), Common.MD5_encript(pwd));
            if (query.Count() > 0)
            {
                Session["usercode"] = query[0].user_code.ToUpper();
                Session["username"] = query[0].user_name;
                Session["acc_info"] = query;
                Session["role_code"] = query[0].role_code;
                Session["role_name"] = query[0].role_name;
                if (Session["prev_action"] != null && Session["prev_control"] != null)
                {
                    return RedirectToAction(Session["prev_action"].ToString(), Session["prev_control"].ToString());
                }
                else
                {
                    return RedirectToAction("Overview", "Home");
                }
            }
            else
            {
                @Session["err"] = "Tài khoản đăng nhập hoặc mật khẩu không đúng!";
                return View();
            }
        }

        public ActionResult Register()
        {
            Session["err"] = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(FormCollection ip_form_1)
        {
            var user_code = ip_form_1["user_code"];
            var user_name = ip_form_1["user_name"];
            var emailaddress = ip_form_1["emailaddress"];
            var password = ip_form_1["password"];
            var otp = ip_form_1["otp"];
            var query = BaseConnectionSql.Execute_STRING_ExecuteScalar("strConn", "sp_check_email_register", emailaddress);
            if (query.ToString() == "SUCCESS")
            {
                var check_otp = Session["otp"].ToString();
                if (check_otp == otp)
                {
                    var regis = BaseConnectionSql.Execute_Update_Insert_V1("strConn", "sp_user_register", user_code, user_name, Common.MD5_encript(password), password, emailaddress);
                    if (regis)
                    {
                        @Session["err"] = "Đăng ký thành công!";
                        return RedirectToAction("Login", "Account");
                    }
                    @Session["err"] = "Đăng ký không thành công, vui lòng thử lại sau!";
                    return View();
                }
                @Session["err"] = "Mã xác thực không đúng, vui lòng kiểm tra lại!";
                return View();
            }
            else
            {
                @Session["err"] = "Email đăng ký tài khoản đã tồn tại!";
                return View();
            }
        }

        public ActionResult send_mail(string mail)
        {
            var query = BaseConnectionSql.Execute_STRING_ExecuteScalar("strConn", "sp_check_email_register", mail);
            if (query.ToString() == "SUCCESS")
            {
                var otp = DataAccess.Common.RandomString(6);

                Session["otp"] = otp;

                var check = sendEmail(mail, "Mã xác thực tài khoản", "Mã xác thực tài khoản của bạn là: " + otp);
                if (check)
                {
                    return Json(1);
                }
                return Json(2);
            }
            else
            {
                return Json(3);
            }
        }

        public static bool sendEmail(string emailAddress, string str_title, string str_content)
        {
            try
            {
                LogBuild.CreateLogger("Start send", "_sendmail");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("minhdung1832@gmail.com");

                mail.To.Add(emailAddress);

                //var mail_body = ob["mail_body"].ToString();

                mail.Subject = str_title;

                mail.IsBodyHtml = true;
                mail.Body = str_content;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("minhdung1832@gmail.com", "rawf ooto hshj psul");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                LogBuild.CreateLogger("Mail done", "_sendmail");

                return true;
            }
            catch (Exception ex)
            {
                LogBuild.CreateLogger(ex.ToString(), "_sendmail");
                return false;
            }
        }

        public ActionResult LogOut()
        {
            Session.Contents.RemoveAll();
            Response.Cookies.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AccountInfo()
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                var lst = BaseConnectionSql.ExecuteList_V1<user_login>("strConn", "sp_user_info", Session["usercode"].ToString());
                ViewBag.acc_info = lst[0];

                return View();
            }
            Common.SaveSession("Account", "AccountInfo");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult change_info(string user_name, string user_password, string email, string phone_number, string address, string nationality, string cccd, string issued_by, string license_date)
        {
            try
            {
                if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
                {
                    var query = BaseConnectionSql.Execute_Update_Insert_V1("strConn", "sp_user_update_info", Session["usercode"].ToString(), user_name, user_password, email, phone_number, address, nationality, cccd, issued_by, license_date);
                    if (query)
                    {
                        return Json(1);
                    }
                    return Json(0);
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "change_info");
                return Json(null);
            }
        }

        public ActionResult AccManagement()
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                return View();
            }
            Common.SaveSession("Account", "AccManagement");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult GetListUser()
        {
            if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
            {
                var userid = Session["usercode"].ToString();
                var role_code = Session["role_code"].ToString();
                DataTable table = BaseConnectionSql.ExecuteDataTable_V1("strConn", "sp_user_get_list", userid, role_code);

                return PartialView("~/Views/Account/Partial/__AccManagement.cshtml", table);
            }
            Common.SaveSession("Account", "AccManagement");
            return RedirectToAction("Login", "Account");
        }

        public ActionResult ChangeStatus(int id, int status)
        {
            try
            {
                if (Session["usercode"] != null && Session["usercode"].ToString().Length > 0)
                {
                    var query = BaseConnectionSql.Execute_STRING_ExecuteScalar("strConn", "sp_user_change_status", Session["usercode"].ToString(), Session["role_code"].ToString(), id, status);
                    if (query.ToString() == "SUCCESS")
                    {
                        return Json(1);
                    }
                    Common.SaveSession("Account", "AccManagement");
                    return Json(0);
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                LogBuild.CreateLogger(JsonConvert.SerializeObject(ex), "ChangeStatus");
                return Json(null);
            }
        }
    }
}