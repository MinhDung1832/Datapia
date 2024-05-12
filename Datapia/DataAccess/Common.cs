using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Datapia.Models;

namespace Datapia.DataAccess
{
    public static class Common
    {
        private static Random random = new Random();
        public static string MD5_encript(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static void SaveSession(string controller, string action)
        {
            HttpContext.Current.Session["prev_action"] = action;
            HttpContext.Current.Session["prev_control"] = controller;
        }

        public static action_link GetAction(string contrl = null,string action_1 = null, string action_2 = null, string action_3 = null, string action_4 = null, string action_5 = null) {
            string str = "";
            str = "/" + contrl + "/";
            var ac = new action_link() { action_home = "/Home/Overview",action_1 = str + action_1,action_2 = str + action_2,action_3 = str + action_3,action_4 = str + action_4,action_5 = str + action_5};
            return ac;
        }
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}