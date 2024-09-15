using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Datapia.Models
{
    public class objCombox
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class action_link
    {
        public string action_home { get; set; }
        public string action_1 { get; set; }
        public string action_2 { get; set; }
        public string action_3 { get; set; }
        public string action_4 { get; set; }
        public string action_5 { get; set; }
    }

    public class API_return
    {
        public bool codeReturn { set; get; }
        public string orderNo { set; get; }
        public string No { set; get; }
        public string message { set; get; }
        public string ValidationPeriodID { set; get; }
        public string Items { set; get; }
    }

    public class API_returnAPI
    {
        public bool StatusCode { set; get; }
        public string Message { set; get; }
        public string Items { set; get; }
    }

    public class API_returnAPI_Call<T>
    {
        public bool StatusCode { set; get; }
        public string Message { set; get; }
        public List<T> Items { set; get; }
    }
    public class API_returnAPI_Call1<T>
    {
        public bool StatusCode { set; get; }
        public string Message { set; get; }
        public T Items { set; get; }
    }

    public class API_returnAPI_v2
    {
        public bool StatusCode { set; get; }
        public List<message> Message { set; get; }
    }

    public class message
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
    }

    public class ResponseModel
    {
        public int Status { get; set; }
        public Message_api Message { get; set; }
    }

    public class Message_api
    {
        public string SpreadsheetId { get; set; }
    }
}