using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Globalization;
using System.Reflection;
using System.Text;
using DataAccess;
using Datapia.Models;

public class BaseApiClient
{
    private static string BaseAddress = ConfigurationManager.AppSettings.Get("BaseAddress");
    private static string TokenAPI = ConfigurationManager.AppSettings.Get("TokenAPI");
    private static string APIKafka = ConfigurationManager.AppSettings.Get("APIKafka");
    private static string TokenKafka = ConfigurationManager.AppSettings.Get("TokenKafka");

    public static async Task<API_returnAPI_Call<TKey>> CallAsync_API<TKey>(string Url)
    {
        API_returnAPI_Call<TKey> rt = new API_returnAPI_Call<TKey>();
        try
        {
            var client = new HttpClient();
            string key = TokenAPI;
            string url = BaseAddress + Url;
            client.DefaultRequestHeaders.Add("Authorization", key);

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                //rt.Items = res;
                var obj = JsonConvert.DeserializeObject<List<TKey>>(JObject.Parse(res)["Item"].ToString());
                rt.Items = obj;
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = null;
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
            rt.Items = null;
        }
        return rt;
    }

    public static async Task<API_returnAPI> GetAsync_API(string Url)
    {
        API_returnAPI rt = new API_returnAPI();
        try
        {
            var client = new HttpClient();
            string key = TokenAPI;
            string url = BaseAddress + Url;
            client.DefaultRequestHeaders.Add("Authorization", key);

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                rt.Items = JObject.Parse(res)["Item"].ToString();
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = JObject.Parse(res)["Message"].ToString();
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
            rt.Items = JsonConvert.SerializeObject(ex);
        }
        return rt;
    }

    public static async Task<API_returnAPI_Call<TKey>> PostAsync_Param_API<TKey, TSearch>(string Url, TSearch it)
    {
        API_returnAPI_Call<TKey> rt = new API_returnAPI_Call<TKey>();
        var client = new HttpClient();
        string strParam = "";
        try
        {
            string key = TokenAPI;
            string urls = BaseAddress;
            client.DefaultRequestHeaders.Add("Authorization", key);

            client.BaseAddress = new Uri(urls);
            strParam = JsonConvert.SerializeObject(it);
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Url, content);

            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                var obj = JsonConvert.DeserializeObject<List<TKey>>(JObject.Parse(res)["Item"].ToString());
                rt.Items = obj;
                LogBuild.CreateLogger(strParam + response.Content, Url);
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = null;
            }
        }
        catch (Exception e)
        {
            LogBuild.CreateLogger(strParam + e.Message, "Url");
        }
        return rt;
    }// Search obj

    public static async Task<API_returnAPI> PostAsync_API<TSearch>(string Url, TSearch it)
    {
        API_returnAPI rt = new API_returnAPI();
        var client = new HttpClient();
        string strParam = "";
        try
        {
            string key = TokenAPI;
            string urls = BaseAddress;
            client.DefaultRequestHeaders.Add("Authorization", key);

            client.BaseAddress = new Uri(urls);
            strParam = JsonConvert.SerializeObject(it);
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Url, content);

            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                LogBuild.CreateLogger(strParam + response.Content, Url);
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = JObject.Parse(res)["Message"].ToString();
            }
        }
        catch (Exception e)
        {
            LogBuild.CreateLogger(strParam + e.Message, "Url");
        }
        return rt;
    }

    public static async Task<API_returnAPI> DeleteAsync_API(string Url)
    {
        API_returnAPI rt = new API_returnAPI();
        try
        {
            var client = new HttpClient();
            string key = TokenAPI;
            string url = BaseAddress + Url;
            client.DefaultRequestHeaders.Add("Authorization", key);

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
            }
            else
            {
                rt.StatusCode = false;
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
        }
        return rt;
    }

    public static async Task<API_returnAPI_Call<TKey>> CallAsync_API_V1<TKey>(string BaseAddress, string TokenAPI, string Url)
    {
        API_returnAPI_Call<TKey> rt = new API_returnAPI_Call<TKey>();
        try
        {
            var client = new HttpClient();
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = ConfigurationManager.AppSettings.Get(TokenAPI);
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                //rt.Items = res;
                var obj = JsonConvert.DeserializeObject<List<TKey>>(res);
                rt.Items = obj;
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = null;
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
            rt.Items = null;
        }
        return rt;
    }

    public static async Task<API_returnAPI_Call<TKey>> CallAsync_API_V2<TKey>(string BaseAddress, string TokenAPI, string Url)
    {
        API_returnAPI_Call<TKey> rt = new API_returnAPI_Call<TKey>();
        try
        {
            var client = new HttpClient();
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                //rt.Items = res;
                var obj = JsonConvert.DeserializeObject<List<TKey>>(JObject.Parse(res)["item"].ToString());
                rt.Items = obj;
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = null;
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
            rt.Items = null;
        }
        return rt;
    }

    public static async Task<API_returnAPI> GetAsync_API_V1(string BaseAddress, string TokenAPI, string Url)
    {
        API_returnAPI rt = new API_returnAPI();
        try
        {
            var client = new HttpClient();
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                rt.Items = JObject.Parse(res)["Item"].ToString();
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = JObject.Parse(res)["Message"].ToString();
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
            rt.Items = JsonConvert.SerializeObject(ex);
        }
        return rt;
    }

    public static async Task<API_returnAPI_Call<TKey>> CallAsync_WordPress<TKey>(string BaseAddress, string TokenAPI, string Url)
    {
        API_returnAPI_Call<TKey> rt = new API_returnAPI_Call<TKey>();
        try
        {
            var client = new HttpClient();
            //BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                //rt.Items = res;
                var obj = JsonConvert.DeserializeObject<List<TKey>>(res);
                rt.Items = obj;
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = null;
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
            rt.Items = null;
        }
        return rt;
    }

    public static async Task<API_returnAPI_Call<TKey>> PostAsync_Param_API_V1<TKey, TSearch>(string BaseAddress, string TokenAPI, string Url, TSearch it)
    {
        API_returnAPI_Call<TKey> rt = new API_returnAPI_Call<TKey>();
        var client = new HttpClient();
        string strParam = "";
        try
        {
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            strParam = JsonConvert.SerializeObject(it);
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Url, content);

            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                var obj = JsonConvert.DeserializeObject<List<TKey>>(JObject.Parse(res)["item"].ToString());
                rt.Items = obj;
                LogBuild.CreateLogger(strParam + response.Content, Url);
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = null;
            }
        }
        catch (Exception e)
        {
            LogBuild.CreateLogger(strParam + e.Message, "Url");
        }
        return rt;
    }// Search obj

    public static async Task<API_returnAPI> PostAsync_API_V1<TSearch>(string BaseAddress, string TokenAPI, string Url, TSearch it)
    {
        API_returnAPI rt = new API_returnAPI();
        var client = new HttpClient();
        string strParam = "";
        try
        {
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            strParam = JsonConvert.SerializeObject(it);
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                LogBuild.CreateLogger(strParam + response.Content, url);
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = JObject.Parse(res)["Message"].ToString();
            }
        }
        catch (Exception e)
        {
            LogBuild.CreateLogger(strParam + e.Message, "url");
        }
        return rt;
    }

    public static async Task<API_returnAPI> PostAsync_API_Noti<TSearch>(string BaseAddress, string TokenAPI, string Url, TSearch it)
    {
        API_returnAPI rt = new API_returnAPI();
        var client = new HttpClient();
        string strParam = "";
        try
        {
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            strParam = JsonConvert.SerializeObject(it);
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Url, content);

            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                rt.Items = res;
                LogBuild.CreateLogger(strParam + response.Content, Url);
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = JObject.Parse(res)["Message"].ToString();
            }
        }
        catch (Exception e)
        {
            LogBuild.CreateLogger(strParam + e.Message, "Url");
        }
        return rt;
    }

    public static async Task<API_returnAPI> DeleteAsync_API_V1(string BaseAddress, string TokenAPI, string Url)
    {
        API_returnAPI rt = new API_returnAPI();
        try
        {
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            var client = new HttpClient();
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
            }
            else
            {
                rt.StatusCode = false;
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
        }
        return rt;
    }

    public static async Task<API_returnAPI> PostAsync_API_VV<TSearch>(string BaseAddress, string TokenAPI, string Url, TSearch it)
    {
        API_returnAPI rt = new API_returnAPI();
        var client = new HttpClient();
        string strParam = "";
        try
        {
            BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
            string url = BaseAddress + Url;
            if (!String.IsNullOrEmpty(TokenAPI))
            {
                string key = TokenAPI;
                client.DefaultRequestHeaders.Add("Authorization", key);
            }
            client.BaseAddress = new Uri(url);

            strParam = JsonConvert.SerializeObject(it);
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Url, content);

            string res = await response.Content.ReadAsStringAsync();
            LogBuild.CreateLogger(res, "ResLogin");

            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                rt.Items = JObject.Parse(res)["message"].ToString();
                LogBuild.CreateLogger(strParam + response.Content, Url);
            }
            else
            {
                rt.StatusCode = false;
            }
        }
        catch (Exception e)
        {
            LogBuild.CreateLogger(strParam + e.Message, "ResLogin");
        }
        return rt;
    }

    public class Message
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
    }

    public class Root
    {
        public int status { get; set; }
        public Message message { get; set; }
    }

    public static async Task<API_returnAPI_Call<TKey>> CallAsync_FastApi<TKey>(string BaseAddress, string TokenAPI, string Url)
    {
        API_returnAPI_Call<TKey> rt = new API_returnAPI_Call<TKey>();
        try
        {
            var client = new HttpClient();
            //BaseAddress = ConfigurationManager.AppSettings.Get(BaseAddress);
                string key = ConfigurationManager.AppSettings.Get(TokenAPI);
            string url = BaseAddress + Url + "?token=" + key;
            //if (!String.IsNullOrEmpty(TokenAPI))
            //{
            //    string key = ConfigurationManager.AppSettings.Get(TokenAPI);
            //    client.DefaultRequestHeaders.Add("Authorization", key);
            //}
            client.BaseAddress = new Uri(url);

            HttpResponseMessage response = await client.GetAsync("");
            string res = await response.Content.ReadAsStringAsync();
            if ((int)response.StatusCode == 200)
            {
                rt.StatusCode = true;
                //rt.Items = res;
                var obj = JsonConvert.DeserializeObject<List<TKey>>(res);
                //var abc =  JObject.Parse(res).ToString();
                rt.Items = obj;
            }
            else
            {
                rt.StatusCode = false;
                rt.Items = null;
            }
        }
        catch (Exception ex)
        {
            rt.StatusCode = false;
            rt.Items = null;
        }
        return rt;
    }
}