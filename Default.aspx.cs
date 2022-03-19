using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OneSignalDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                //System.Diagnostics.Debug.WriteLine(responseContent);
                NotifyExternal("2");
                //NotifyPlayer("1", "hello b");
            }
        }

        public void NotifyExternal(string externalId)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + ConfigurationManager.AppSettings["OneSignalApiKey"]);

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = ConfigurationManager.AppSettings["OneSignalAppID"],
                contents = new { en = "English Message" },
                channel_for_external_user_ids = "push",
                include_external_user_ids = new string[] { externalId }
            };



            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }
        public void NotifyPlayer(string playerId, string message)
        {
            string appId = ConfigurationManager.AppSettings["OneSignalAppID"];
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = appId,
                contents = new { en = message },
                include_player_ids = new string[] { playerId }
            };
            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
        public void NotifySubscribers(string message)
        {
            string appId = ConfigurationManager.AppSettings["OneSignalAppID"];
            string apiKey = ConfigurationManager.AppSettings["OneSignalApiKey"];
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + apiKey);

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = appId,
                contents = new { en = message },
                included_segments = new string[] { "Subscribed Users" }
                
            };
            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
        }
    }
}