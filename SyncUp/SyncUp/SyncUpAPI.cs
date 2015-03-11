using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;

namespace SyncUp
{
    class SyncUpAPI
    {
        #region tp_host_address_section

        private const string ApiServer = "http://skylerbock.com:8888";

        public static string GetApiURL()
        {
            return ApiServer;
        }

        #endregion // tp_host_address_section

        #region json_response_definition_section

        public const int ERR_GENERAL_EXCEPTION = 500;
        public const int ERR_HTTP_EXCEPTION = 501;
        public const int ERR_SUCCESS = 200;
        public const int ERR_INVALID_USER_PASS = 203;
        public const int ERR_INVALID_GUID = 206;

        [DataContract]
        public class ApiJsonAppRegResp
        {
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "response")]
            public string response { get; set; }
        }

        [DataContract]
        public class ApiJsonUserRegResp
        {
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "response")]
            public string response { get; set; }
        }

        [DataContract]
        public class ApiJsonSendMsgResp
        {
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "response")]
            public string response { get; set; }
        }

        [DataContract]
        public class ApiJsonUUIDResp
        {
            [DataMember(Name = "guid")]
            public string guid { get; set; }
        }

        [DataContract]
        public class ApiJsonAuthResp
        {
            [DataMember(Name = "token")]
            public string token { get; set; }
        }


        #endregion // json_response_definition_section

        #region tp_request_section

        public static ApiJsonAppRegResp RegisterAppInstance(string username, string password, string guid)
        {
            ApiJsonAppRegResp resp = null;

            int code = GetRegisterAppInstance(ref resp, null, username, password, guid);

            if (code != ERR_SUCCESS)
            {
                Console.WriteLine("Error - RegisterAppInstance - code: " + code);
            }

            return resp;
        }

        public static int GetRegisterAppInstance(ref ApiJsonAppRegResp resp, string url, string username, string password, string guid, string client_name = "SyncUp - Windows Client")
        {
            if (url == null) url = GetApiURL();

            string msg = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"guid\":\""+guid+"\",\"name\":\""+client_name+"\"}";
            string respjson = "";
            resp = null;

            int retCode = GetHttpPostQuery(url + "/apps.php/register", msg, ref respjson);
            resp = Deserialize<ApiJsonAppRegResp>(respjson);

            if (retCode == ERR_SUCCESS)
            {
                return resp.code;
            }

            return retCode;
        }

        public static ApiJsonUserRegResp RegisterUser(string username, string password, string email)
        {
            ApiJsonUserRegResp resp = null;

            int code = GetRegisterUser(ref resp, null, username, password, email);

            if (code != ERR_SUCCESS)
            {
                Console.WriteLine("Error - RegisterAppInstance - code: " + code);
            }

            return resp;
        }

        public static int GetRegisterUser(ref ApiJsonUserRegResp resp, string url, string username, string password, string email)
        {
            if (url == null) url = GetApiURL();

            string msg = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"email\":\"" + email + "\"}";
            string respjson = "";
            resp = null;

            int retCode = GetHttpPostQuery(url + "/users.php/register", msg, ref respjson);
            resp = Deserialize<ApiJsonUserRegResp>(respjson);

            if (retCode == ERR_SUCCESS)
            {
                return resp.code;
            }

            return retCode;
        }

        public static ApiJsonSendMsgResp SendMessage(string appguid, string token, string to_userid, string message)
        {
            ApiJsonSendMsgResp resp = null;

            int code = PostSendMessage(ref resp, null, appguid, token, to_userid, message);

            if (code != ERR_SUCCESS)
            {
                Console.WriteLine("Error - SendMessage - code: " + code);
            }

            return resp;
        }

        public static int PostSendMessage(ref ApiJsonSendMsgResp resp, string url, string appid, string token, /*send message to*/ string userid, string message)
        {
            if (url == null) url = GetApiURL();

            string msg = "{\"appid\":\"" + appid + "\",\"token\":\"" + token + "\",\"userid\":\"" + userid + "\",\"msg\":\"" + message + "\"}";
            string respjson = "";
            resp = null;

            int retCode = GetHttpPostQuery(url + "/apps.php/send", msg, ref respjson);
            resp = Deserialize<ApiJsonSendMsgResp>(respjson);

            if (retCode == ERR_SUCCESS)
            {
                return resp.code;
            }

            return retCode;
        }

        public static int LoginApp(string username, string pw, string url = null)
        {
            if (username == null || username.Length == 0) return ERR_GENERAL_EXCEPTION;
            if (pw == null) pw = "";
            if (url == null) url = GetApiURL();

            //GET NEW GUID
            ApiJsonUUIDResp uuidResp = null;
            int error = GetUUID(ref uuidResp);
            if (error != ERR_SUCCESS)
            {
                Console.WriteLine("HTTP ERROR - GetUUID - code: " + error);
                return error;
            }

            string guid = uuidResp.guid;
            AppGlobals.AppGUID = guid; // save app guid

            //REGISTER GUID to user
            ApiJsonAppRegResp regResp = RegisterAppInstance(username, pw, guid);

            if (regResp == null)
            {
                return -1;
            }
            if (regResp.code != ERR_SUCCESS)
            {
                return regResp.code;
            }
            // Login success!
            AppGlobals.Username = username; // save username
            AppGlobals.Password = pw; // save password

            //GET AUTH TOKEN
            ApiJsonAuthResp authResp = null;
            error = GetAuthToken(ref authResp, username, pw, guid);

            if (error != ERR_SUCCESS)
            {
                Console.WriteLine("HTTP ERROR - GetAuthToken - code: " + error);
                return error;
            }

            //if (authResp.code != ERR_SUCCESS)
            //{
            //    Console.WriteLine("Error - GetAuthToken - code: " + authResp.code + " msg:" + authResp.response);
            //    return authResp.code;
            //}
            ////Token success!
            //ApiJsonAuthResp tokenResp = Deserialize<ApiJsonAuthResp>(authResp.response);
            string token = authResp.token;
            AppGlobals.Token = token; // save token

            return ERR_SUCCESS;
        }


        public static int GetUUID(ref ApiJsonUUIDResp resp, string url = null)
        {
            if (url == null) url = GetApiURL();

            string respjson = "";
            resp = null;

            int retCode = GetHttpPostQuery(url + "/apps.php/getuuid", "", ref respjson);
            resp = Deserialize<ApiJsonUUIDResp>(respjson);

            return retCode;
        }

        public static int GetAuthToken(ref ApiJsonAuthResp resp, string username, string password, string guid, string url = null)
        {
            if (url == null) url = GetApiURL();

            string msg = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"guid\":\"" + guid + "\"}";
            string respjson = "";
            resp = null;

            int retCode = GetHttpPostQuery(url + "/apps.php/gettoken", msg, ref respjson);
            resp = Deserialize<ApiJsonAuthResp>(respjson);

            return retCode;
        }

        #endregion // tp_request_section


        #region util_section

        public static int GetHttpResponse(string method, string url, string postMsg, ref string respMsg)
        {
            int retCode = ERR_SUCCESS;

    #if DEBUG
            Trace.WriteLine(method + " url=" + url);
            Trace.WriteLine("send=" + postMsg);
    #endif

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpResponse = null;

            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = method;

                if (method.ToUpper() == "POST")
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(postMsg);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    respMsg = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                retCode = (int)httpResponse.StatusCode;
                Trace.WriteLine("recv[" + retCode.ToString() + "]=" + respMsg);
            }
            catch (WebException ex)
            {
                Trace.WriteLine("WebException is thrown. Message is :" + ex.Message);
                if (ex.Response != null)
                {
                    using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        respMsg = streamReader.ReadToEnd();
                        streamReader.Close();
                    }

                    retCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                    Trace.WriteLine("ex-recv[" + retCode.ToString() + "]=" + respMsg);
                }
                else
                {
                    retCode = ERR_HTTP_EXCEPTION;
                    respMsg = ex.Message;
                    Trace.WriteLine("HTTP Exception[" + retCode.ToString() + "]=" + ex.Message);
                }
            }
            catch (Exception e)
            {
                retCode = ERR_GENERAL_EXCEPTION;
                respMsg = e.Message;
                Trace.WriteLine("Exception[" + retCode.ToString() + "]=" + e.Message);
            }
            return retCode;
        }


        public static int GetHttpPostQuery(string url, string postMsg, ref string respMsg)
        {
            return GetHttpResponse("POST", url, postMsg, ref respMsg);
        }

        public static int GetHttpGetQuery(string url, ref string respMsg)
        {
            return GetHttpResponse("GET", url, "", ref respMsg);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings();
                settings.UseSimpleDictionaryFormat = true;
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType(), settings);
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Deserialize() Exception : err=" + ex.Message);
            }
            return obj;
        }

        public static void InitiateSSLTrust()
        {
            // diable error on SSL certificate
            try
            {
                //Change SSL checks so that all checks pass
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );
            }
            catch (Exception ex)
            {
                Trace.WriteLine("InitiateSSLTrust() Exception : err=" + ex.Message);
            }
        }

        public static bool ValidateEmail(string email)
        {
            const string emailReg = @"^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,})$";
            Regex rgx = new Regex(emailReg);

            if (!rgx.IsMatch(email))
            {
                Trace.WriteLine("ValidateEmail() err email=" + email);
                return false;
            }
            return true;
        }

        #endregion // util_section
    }
}