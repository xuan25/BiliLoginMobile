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

            dynamic json = JsonParser.Parse(result);
            UserInfo userInfo = new UserInfo();
            userInfo.CurrentLevel = (uint)json.data.level_info.current_level;
            userInfo.CurrentMin = (int)json.data.level_info.current_min;
            userInfo.CurrentExp = (int)json.data.level_info.current_exp;
            userInfo.NextExp = (int)json.data.level_info.next_exp;
            userInfo.BCoins = (int)json.data.bCoins;
            userInfo.Coins = json.data.coins;
            userInfo.Face = Regex.Unescape(json.data.face);
            userInfo.NameplateCurrent = Regex.Unescape(json.data.nameplate_current);
            userInfo.PendantCurrent = Regex.Unescape(json.data.pendant_current);
            userInfo.Uname = Regex.Unescape(json.data.uname);
            userInfo.UserStatus = Regex.Unescape(json.data.userStatus);
            userInfo.VipType = (uint)json.data.vipType;
            userInfo.VipStatus = (uint)json.data.vipStatus;
            userInfo.OfficialVerify = (int)json.data.official_verify;
            userInfo.PointBalance = (uint)json.data.pointBalance;

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
