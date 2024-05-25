using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Datapia.Models
{
    public class user_login
    {
        public int id { get; set; }
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
        public string password_show { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public string nationality { get; set; }
        public string cccd { get; set; }
        public string issued_by { get; set; }
        public string license_date { get; set; }
        public int role_code { get; set; }
        public string role_name { get; set; }
        public string status { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string modify_by { get; set; }
        public string modify_date { get; set; }
    }

    public class user_permission
    {
        public string fcode { get; set; }
        public string fname { get; set; }
        public string parentcode { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public string icon { get; set; }
        public string status { get; set; }
        public string check_display { get; set; }
        public string Menu { get; set; }
        public string orderby { get; set; }
        public string flevel { get; set; }
    }
}