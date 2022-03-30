using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
namespace CSDN部落格
{
    public partial class Registe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
        }
        public void sendMail(string email, string activeCode)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("15031259715@163.com");
            mailMsg.To.Add(email);
            mailMsg.Subject = "請啟用註冊";
            int number = number1();
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.Append("請單擊以下連結完成啟用");
            contentBuilder.Append("< a href ='http://localhost:15464/cheng.aspx?activecode=" activeCode "&id=" number "‘>啟用</a>");
            mailMsg.Body = contentBuilder.ToString();//拼接字串
            mailMsg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            //發件方伺服器地址
            client.Host = "smtp.163.com";
            client.Port = 25;
            //mailMsg.IsBodyHtml = true;
            NetworkCredential credetial = new NetworkCredential();
            credetial.UserName = "15031259715";
            credetial.Password = "wangjing911214 ";
            client.Credentials = credetial;
            client.Send(mailMsg);
        }
        public int number1()
        {
            CSDN部落格.BLL.T_User count = new BLL.T_User();
            int a = count.GetRecordCount("");
            return a;
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            CSDN部落格.Model.T_User muser = new Model.T_User();
            muser.Name = txtName.Text;
            muser.Password = txtPassword.Text;
            muser.E_Mail = txtEmail.Text;
            string activecode = Guid.NewGuid().ToString().Substring(0, 8);
            muser.ActiveCode = activecode;//生成啟用碼
            CSDN部落格.BLL.T_User buser = new BLL.T_User();
            if (buser.Add(muser) > 0)
            {
                sendMail(txtEmail.Text, activecode);//給註冊使用者發郵件
                lbinfo.Text = "儲存成功";
            }
            else { lbinfo.Text = "儲存失敗"; }
        }
    }
}
</ div >
< !–底部結束–>
 </ div >
 </ form >
 </ body >
 </ html >

————啟用驗證
複製程式碼 程式碼如下:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
namespace CSDN部落格
{
    public partial class cheng : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //取出引數id
            int id = Convert.ToInt32(Request["id"]);
            string activeCode = Request["activecode"].ToString();
            //2判斷id為id的記錄是否存在
            //連線資料庫
            string conStr = "data source = LOVE - PC\\SQLEXPRESSPC; initial catalog = Blogs; user id = sa; password = admin";
            int number;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string sql = "select count(*) from T_User where Id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    number = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            if (number > 0)
            {
                //如果該使用者存在取出ActiveCode欄位進行比較。如果一樣，把Active欄位修改為true
                //連線資料庫
                string AC;
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string sql = "select ActiveCode from T_User where Id = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        AC = cmd.ExecuteScalar().ToString(); ;
                    }
                }
                if (activeCode == AC)
                {
                    Response.Write("啟用成功! < a href ='denglu.aspx'> 返回登入 </ a >");
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        string sql = "update T_User set Active = 1 where Id = @id";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@id", id);
                            number = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                }
                else
                {
                    Response.Write("使用者已存在，但是啟用碼錯誤！");
                }
            }
            else
            {
                Response.Write("使用者不存在，還沒註冊成功！");
            }
        }
    }
}