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

        [DataContract]
        public class ApiJsonResp
        {
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "response")]
            public string response { get; set; }

            // source json response
            public string jsonResp;
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

        public static ApiJsonResp RegisterAppInstance(string username, string password, string guid)
        {
            ApiJsonResp resp = null;

            int code = GetRegisterAppInstance(ref resp, null, username, password, guid);

            if (code != ERR_SUCCESS)
            {
                Console.WriteLine("Error - RegisterAppInstance - code: " + code + " resp:" + resp.jsonResp);
            }

            return resp;
        }

        public static int GetRegisterAppInstance(ref ApiJsonResp resp, string url, string username, string password, string guid, string client_name = "SyncUp - Windows Client")
        {
            if (url == null) url = GetApiURL();

            string msg = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"guid\":\""+guid+"\",\"name\":\""+client_name+"\"}";
            string respjson = "";
            resp = null;

            int retCode = GetHttpPostQuery(url + "/apps.php/register", msg, ref respjson);
            resp = Deserialize<ApiJsonResp>(respjson);

            if (retCode == ERR_SUCCESS)
            {
                return resp.code;
            }

            return retCode;
        }

        public static int LoginApp(string username, string pw, string url = null)
        {
            if (username == null || username.Length == 0) return ERR_INVALID_ARG;
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
            ApiJsonResp regResp = RegisterAppInstance(username, pw, guid);

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

        public const int MAX_INVITE_COUNT = 1000;

        public const int ERR_GENERAL_EXCEPTION = 900;
        public const int ERR_HTTP_EXCEPTION = 901;
        public const int ERR_INVALID_ARG = -1;
        public const int ERR_INVALID_APP_ID = -2;
        public const int ERR_SUCCESS = 200;
        public const int ERR_INVALID_ID_PW = 203;
        public const int ERR_DUP_EMAIL_IN_ORG = 209;
        public const int ERR_DUP_EMAIL_OUT_ORG = 210;
        public const int ERR_DIRTY_EMAIL = 211;
        public const int ERR_NOT_ORG_ADMIN = 400;
        public const int ERR_ORG_EMAIL_FULL = 418;
        public const int ERR_INVALID_EMAIL = 415;

        public static Dictionary<int, string> TPErrorMessage = new Dictionary<int, string>()
        {
            { ERR_GENERAL_EXCEPTION, "System exception occurred."},
            { ERR_HTTP_EXCEPTION,    "Unable to connect to server."},
            { ERR_INVALID_ARG,       "Invlid Arguments."},
            { ERR_INVALID_APP_ID,    "Invlid App ID."},
            { ERR_INVALID_ID_PW,     "The email address and password combination does not match any known Transporter Administrator account."},
            { ERR_DUP_EMAIL_IN_ORG,  "A user with this email address is already in your Organization."},
            { ERR_DUP_EMAIL_OUT_ORG, "This email address already belongs to a Transporter user outside of this Organization."},
            { ERR_DIRTY_EMAIL,       "The email address is flagged as unsafe."},
            { ERR_NOT_ORG_ADMIN,     "The email and password combination must match an Organization Admin’s credentials."},
            { ERR_ORG_EMAIL_FULL,    "You have reached the 1,000 users per Organization limit."},
            { ERR_INVALID_EMAIL,     "Invalid email format."}
        };

        public static string GetErrorMessage(int errCode)
        {
            string errMsg = "Unknown Error";
            if (TPErrorMessage.ContainsKey(errCode))
            {
                errMsg = TPErrorMessage[errCode];
            }
            else
            {
                errMsg += " [" + errCode.ToString() + "]";
            }
            return errMsg;
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

        #region update_section

        public static Version AvailableVersion;
        public static string DownloadURL;
        public static string ChangeLogURL;

        public static string GetUpdateVersion()
        {
            string updateversion = "";
            string thisversion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Version InstalledVersion = new Version(thisversion);

    #if DEBUG
            string url = "http://localhost/updates.xml";
    #else
            string url = "http://appsoftware.connecteddata.com/tdconnector/updates.xml";
    #endif

            string respXml = "";
            int retCode = GetHttpGetQuery(url, ref respXml);
            if (retCode != 200 || respXml.Length == 0)
            {
                Trace.WriteLine("GetUpdateVersion() error : " + retCode.ToString());
                return updateversion;
            }

            try
            {
                XmlDocument receivedAppCastDocument = new XmlDocument();
                receivedAppCastDocument.LoadXml(respXml);

                XmlNodeList appCastItems = receivedAppCastDocument.SelectNodes("item");
                if (appCastItems != null)
                {
                    foreach (XmlNode item in appCastItems)
                    {
                        XmlNode appCastVersion = item.SelectSingleNode("version");
                        if (appCastVersion != null)
                        {
                            String appVersion = appCastVersion.InnerText;
                            Version version = new Version(appVersion);
                            if (version <= InstalledVersion)
                            {
                                Trace.WriteLine("GetUpdateVersion() Ignore Update : this=" + InstalledVersion.ToString() + " there=" + version.ToString());
                                continue;
                            }
                            AvailableVersion = version;
                            updateversion = AvailableVersion.ToString();
                        }
                        else
                            continue;

                        XmlNode appCastTitle = item.SelectSingleNode("title");

                        XmlNode appCastChangeLog = item.SelectSingleNode("changelog");
                        ChangeLogURL = appCastChangeLog != null ? appCastChangeLog.InnerText : "";

                        XmlNode appCastUrl = item.SelectSingleNode("url");
                        DownloadURL = appCastUrl != null ? appCastUrl.InnerText : "";
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("GetUpdateVersion() Exception : err=" + ex.Message);
            }

            return updateversion;
        }

        #endregion // update_section
    }
}