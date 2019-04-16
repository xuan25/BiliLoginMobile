using Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiliLogin
{
    class UserInfo
    {
        public static UserInfo GetUserInfo(CookieCollection cookies)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://account.bilibili.com/home/userInfo");
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookies);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string result = reader.ReadToEnd();
            reader.Close();
            response.Close();
            dataStream.Close();

            IJson json = JsonParser.Parse(result);
            UserInfo userInfo = new UserInfo();
            userInfo.CurrentLevel = (uint)json.GetValue("data").GetValue("level_info").GetValue("current_level").ToLong();
            userInfo.CurrentMin = (int)json.GetValue("data").GetValue("level_info").GetValue("current_min").ToLong();
            userInfo.CurrentExp = (int)json.GetValue("data").GetValue("level_info").GetValue("current_exp").ToLong();
            userInfo.NextExp = (int)json.GetValue("data").GetValue("level_info").GetValue("next_exp").ToLong();
            userInfo.BCoins = (int)json.GetValue("data").GetValue("bCoins").ToLong();
            userInfo.Coins = json.GetValue("data").GetValue("coins").ToDouble();
            userInfo.Face = Regex.Unescape(json.GetValue("data").GetValue("face").ToString());
            userInfo.NameplateCurrent = Regex.Unescape(json.GetValue("data").GetValue("nameplate_current").ToString());
            userInfo.PendantCurrent = Regex.Unescape(json.GetValue("data").GetValue("pendant_current").ToString());
            userInfo.Uname = Regex.Unescape(json.GetValue("data").GetValue("uname").ToString());
            userInfo.UserStatus = Regex.Unescape(json.GetValue("data").GetValue("userStatus").ToString());
            userInfo.VipType = (uint)json.GetValue("data").GetValue("vipType").ToLong();
            userInfo.VipStatus = (uint)json.GetValue("data").GetValue("vipStatus").ToLong();
            userInfo.OfficialVerify = (int)json.GetValue("data").GetValue("official_verify").ToLong();
            userInfo.PointBalance = (uint)json.GetValue("data").GetValue("pointBalance").ToLong();

            return userInfo;
        }

        public uint CurrentLevel;
        public int CurrentMin;
        public int CurrentExp;
        public int NextExp;
        public int BCoins;
        public double Coins;
        public string Face;
        public string NameplateCurrent;
        public string PendantCurrent;
        public string Uname;
        public string UserStatus;
        public uint VipType;
        public uint VipStatus;
        public int OfficialVerify;
        public uint PointBalance;

        public Bitmap GetFaceBitmap()
        {
            return DownloadBitmap(Face);
        }

        public Bitmap GetNamePlateBitmap()
        {
            return DownloadBitmap(NameplateCurrent);
        }

        public Bitmap GetPendantBitmap()
        {
            return DownloadBitmap(PendantCurrent);
        }

        private Bitmap DownloadBitmap(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            Bitmap result = new Bitmap(dataStream);
            response.Close();
            dataStream.Close();
            return result;
        }
    }
}
