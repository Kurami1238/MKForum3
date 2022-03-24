using MKForum.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;


namespace MKForum.BackAdmin
{
    public partial class tryencryption : System.Web.UI.Page
    {
        private EncryptionHelper _encryption = new EncryptionHelper();
        static Aes myAes = Aes.Create();

        protected void Page_Load(object sender, EventArgs e)
        {



        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //r加密();

            //雜湊
            string orgText = this.TextBox1.Text;
            string key = "AABBCCDDEE";
            byte[] salt = _encryption.BuildNewSalt();

            byte[] securitBytes = _encryption.GetHashPassword(orgText, key, salt);
            string bytetext = string.Join(" ", salt.Select(obj => obj.ToString("X2"))); //ToString("x") 是轉16進位
            this.Label1.Text = bytetext;

            string ToBase64Text = Convert.ToBase64String(securitBytes);
            this.Label2.Text = ToBase64Text;

        }

        private void r加密()
        {
            //加密
            string orgText = this.TextBox1.Text;
            byte[] securitBytes = _encryption.AesEncrypt(orgText, myAes.Key, myAes.IV);
            string securitstr = string.Join(" ", securitBytes.Select(obj => obj.ToString("x"))); //ToString("x") 是轉16進位
            this.Label1.Text = securitstr;

            //解密
            string desecuritText = _encryption.AesDecrypt(securitBytes, myAes.Key, myAes.IV);
            this.Label2.Text = desecuritText;
        }
    }
}