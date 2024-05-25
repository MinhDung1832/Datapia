using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datapia.Models
{
    public class get_data
    {
        public string userid { get; set; }
        public int id { get; set; }
        public string data_from { get; set; }
        public string acc_id_val { get; set; }
        public string app_secret { get; set; }
        public string access_token { get; set; }
        public DateTime? get_from_date { get; set; }
        public string data_to { get; set; }
        public string acc_link { get; set; }
        public string user_code { get; set; }
        public string password { get; set; }
        public bool stop_using { get; set; }
        public bool acc_id { get; set; }
        public bool acc_name { get; set; }
        public bool camp_id { get; set; }
        public bool camp_name { get; set; }
        public bool ad_group_id { get; set; }
        public bool ad_group_name { get; set; }
        public bool currency_unit { get; set; }
        public bool start_date { get; set; }
        public bool end_date { get; set; }
        public bool year_old { get; set; }
        public bool nation { get; set; }
        public bool region_id { get; set; }
        public bool region_name { get; set; }
        public bool gender { get; set; }
        public bool performance { get; set; }
    }

    public class config_detail
    {
        public int id { get; set; }
        public string data_from { get; set; }
        public string acc_id_val { get; set; }
        public string app_secret { get; set; }
        public string access_token { get; set; }
        public string get_from_date { get; set; }
        public string data_to { get; set; }
        public string acc_link { get; set; }
        public string user_code { get; set; }
        public string password { get; set; }
        public bool stop_using { get; set; }
        public bool acc_id { get; set; }
        public bool acc_name { get; set; }
        public bool camp_id { get; set; }
        public bool camp_name { get; set; }
        public bool ad_group_id { get; set; }
        public bool ad_group_name { get; set; }
        public bool currency_unit { get; set; }
        public bool start_date { get; set; }
        public bool end_date { get; set; }
        public bool year_old { get; set; }
        public bool nation { get; set; }
        public bool region_id { get; set; }
        public bool region_name { get; set; }
        public bool gender { get; set; }
        public bool performance { get; set; }
        public int status { get; set; }
        public string create_by { get; set; }
        public string create_date { get; set; }
        public string modify_by { get; set; }
        public string modify_date { get; set; }
    }

    public class test_fastapi
    {
        public string item_no { get; set; }
        public string item_name { get; set; }
    }
}