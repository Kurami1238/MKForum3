using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;  //*** Web Service 會用到 ***
using System.Web.Configuration;

namespace MKForum
{
    public partial class test : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)  // 第一次執行
        //    {   // 自己寫的副程式，從第一頁開始。
        //        Repeater1.DataSource = MIS2000Lab_GetPageData(1);
        //        Repeater1.DataBind();
        //    }
        //}


        //// ***** 分頁。使用SQL指令進行分頁 *****
        //public static DataSet MIS2000Lab_GetPageData(int currentPage)
        //{
        //    SqlConnection Conn = new SqlConnection("資料庫的連結字串");

        //    String SqlStr = "Select * from Customers ORDER BY [CustomerID] ASC ";
        //    SqlStr += " OFFSET @A ROWS FETCH NEXT 10 ROWS ONLY";
        //    // 為了配合前端的jQeury，這裡的第一頁不可以是"零"，需是一。程式改成 ((currentPage-1) * 10)
        //    //==SQL 2012 指令的 Offset...Fetch。參考資料： http://sharedderrick.blogspot.tw/2012/06/t-sql-offset-fetch.html  

        //    SqlDataAdapter myAdapter = new SqlDataAdapter(SqlStr, Conn);
        //    myAdapter.SelectCommand.Parameters.AddWithValue("@A", ((currentPage - 1) * 10));

        //    SqlDataAdapter myAdapter = new SqlDataAdapter(SqlStr, Conn);
        //    DataSet ds = new DataSet();
        //    myAdapter.Fill(ds, "Customers");

        //    //-- 用來計算分頁的「總頁數」 ---      
        //    SqlCommand cmd = new SqlCommand("select Count(CustomerID) from Customers", Conn);
        //    Conn.Open();
        //    int myTotalCount = (int)cmd.ExecuteScalar();

        //    DataTable dt = new DataTable("PageCount");
        //    dt.Columns.Add("PageCount");
        //    dt.Rows.Add();
        //    dt.Rows[0][0] = myTotalCount;
        //    ds.Tables.Add(dt);

        //    if (Conn.State == ConnectionState.Open)
        //    {
        //        Conn.Close();
        //        Conn.Dispose();
        //    }

        //    return ds;
        //}


        //[WebMethod]
        //public static string GetCustomers(int pageIndex)
        //{
        //    return MIS2000Lab_GetPageData(pageIndex).GetXml();
        //}
    }
}