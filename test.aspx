<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registe.aspx.cs" Inherits="CSDN部落格.Registe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .zhuyi {
            color: Red;
            font-size: small;
        }

        #pwd-strong {
            float: left;
            margin: 0px;
            padding: 0px;
            list-style: none;
            background-image: url(images/pwdstrong.gif);
            background-repeat: no-repeat;
        }

            #pwd-strong li {
                float: left;
                padding: 0px;
                color: #ccc;
                font-size: 11px;
                width: 64px;
                height: 10px;
                text-align: center;
                padding-top: 9px;
            }

        .pwds3 {
            background-position: -20px -70px;
        }

        .pwds2 {
            background-position: -20px -44px;
        }

        .pwds1 {
            background-position: -20px -18px;
        }

        #pwd-strong li.currs {
            color: #000;
        }
        /*頭部css樣式*/
        #head {
            margin: 0;
        }

            #head a:hover {
                color: Red;
            }

        .ahead {
            color: black;
            font-size: small;
            text-decoration: none;
        }

        #lefthead {
            float: left;
            margin: 0;
        }

        #righthead {
            float: right;
            margin: 0;
        }
        /*頭部css樣式結束*/
        /*資訊部分開始*/
        #body {
            /*
border: 1px solid black;
*/
            position: absolute;
            width: 900px;
            height: 700px;
            left: 230px;
            top: 50px;
        }

        #picture {
            float: left;
        }

        #xinxi {
            border: 1px solid pink;
            width: 850px;
            height: 440px;
            position: absolute;
            left: 20px;
            top: 50px
        }

        #biaoti {
            position: absolute;
            left: 1px;
            top: 10px;
        }

        #xian {
            position: absolute;
            left: 20px;
            top: 50px;
        }

        #table {
            position: absolute;
            /*
border:1px solid black;
*/
            left: 50px;
            top: 90px;
        }
        /*資訊部分結束*/
        /*底部開始*/
        #footer {
            /*
border:1px solid black;
*/
            position: absolute;
            left: 430px;
            top: 560px;
        }

        .buttom {
            margin: 0px;
            font-size: small;
        }

        .abuttom {
            text-decoration: none;
            color: Black;
        }

        #footer a:hover {
            color: Blue;
        }

        img {
            border: 0px;
        }
        /*底部結束*/
    </style>
    <script src="emailvalidatorjs.js" type="text/javascript"></script>
    <script src="jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtPassword').keyup(function () {
                var score = testpass($(this).val());
                if (score < 34) {
                    $('#pwd-strong').css('display', 'block');
                    $('#pwd-strong').addClass('pwds1');
                    $('li:first').addClass('currs');
                }
                else if (score >= 34 && score < 68) {
                    $('#pwd-strong').css('display', 'block');
                    $('#pwd-strong').removeClass();
                    $('#pwd-strong').addClass('pwds2');
                    $('li:eq(1)').addClass('currs');
                }
                else {
                    $('#pwd-strong').css('display', 'block');
                    $('#pwd-strong').removeClass();
                    $('#pwd-strong').addClass('pwds3');
                    $('li:last').addClass('currs');
                }
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!–內容–>
        <div>
            <!–頭部–>
            <div id="head">
                <!–頭部左–>
                <div id="lefthead">
                    <a href="http://www.csdn.net/" class="ahead">首頁</a>
                    <a href="http://news.csdn.net/" class="ahead">業界</a>
                    <a href="http://mobile.csdn.net/" class="ahead">移動</a>
                    <a href="http://cloud.csdn.net/" class="ahead">雲端計算</a>
                    <a href="http://sd.csdn.net/" class="ahead">研發</a>
                    <a href="http://bbs.csdn.net/" class="ahead">論壇</a>
                    <a href="http://blog.csdn.net/" class="ahead">部落格</a>
                    <a href="http://download.csdn.net/" class="ahead">下載</a>
                    <a class="ahead">更多</a>
                </div>
                <!–頭部右–>
                <div id="righthead">
                    <span class="zhuyi">你還未登陸！</span>|&nbsp
                    <a href="http://www.csdn.net/" class="ahead">登陸</a>&nbsp&nbsp|&nbsp
                    <a href="http://www.csdn.net/" class="ahead">註冊</a>&nbsp&nbsp|&nbsp
                    <a href="https://passport.csdn.net/help/faq" class="ahead">幫助</a>
                </div>
            </div>
            <!–頭部結束–>
<!–身體–>
            <div id="body">
                <!–圖片–>
                <div id="picture">
                    <img src="images/zhuce.gif" />
                </div>
                <!–資訊–>
                <div id="xinxi">
                    <div id="biaoti">
                        <img src="images/zhuce1.gif" />
                    </div>
                    <div id="xian">
                        <img src="images/zhuce2.png" />
                    </div>
                    <div id="table">
                        <table>
                            <!–使用者名稱–>
                            <tr>
                                <td><span class="zhuyi">*</span>使用者名稱：</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" ControlToValidate="TextBox1"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ErrorMessage="使用者名稱不能為空" ControlToValidate="txtName" Display="Dynamic"
                                        Font-Size="15px" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <label style="color: #c0c0c0; font-size: small;">（字母、數字或下劃線組合。）</label></td>
                            </tr>
                            <!–密碼–>
                            <tr>
                                <td><span class="zhuyi">*</span>密碼：</td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="不能為空"
                                        Font-Size="15px" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <div>
                                        <ul id="pwd-strong" style="display: none;">
                                            <li>弱</li>
                                            <li>中</li>
                                            <li>強</li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <label style="color: #c0c0c0; font-size: small;">（為了您的帳戶安全，強烈建議您的密碼使用字元 數字等多種不同型別的組合，並且密碼長度大於5位。）</label></td>
                            </tr>
                            <!–再次輸入密碼–>
                            <tr>
                                <td><span class="zhuyi">*</span>再次輸入密碼：</td>
                                <td>
                                    <asp:TextBox ID="txtPasswordAgain" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                        ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="不能為空"
                                        ForeColor="Red" Font-Size="15px"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                                        ErrorMessage="兩次輸入的密碼不一致" ControlToCompare="txtPassword"
                                        ControlToValidate="txtPasswordAgain" Display="Dynamic" ForeColor="Red"
                                        Font-Size="15px"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <label style="color: #c0c0c0; font-size: small;">（請確保密碼正確）</label></td>
                            </tr>
                            <!–E-mail–>
                            <tr>
                                <td><span class="zhuyi">*</span>E-mail</td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                        ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="不能為空"
                                        ForeColor="Red" Font-Size="15px"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="郵箱格式不正確"
                                        ValidationExpression="\w ([- .']\w )*@\w ([-.]\w )*\.\w ([-.]\w )*"
                                        ForeColor="Red" Font-Size="15px"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <label style="color: #c0c0c0; font-size: small;">（請填寫您常用的郵箱）</label></td>
                            </tr>
                            <!–驗證碼–>
                            <tr>
                                <td><span class="zhuyi">*</span>校驗碼：</td>
                                <td>
                                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
                                <td>
                                    <img src="checkCode.aspx" onclick="this.src='checkCode.aspx?aaa=' new Date()" alt="" />圖片看不清？點選重新得到驗證碼
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                        ControlToValidate="TextBox5" Display="Dynamic" ErrorMessage="不能為空"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <span class="zhuyi">（如您連續輸入不對驗證碼，請檢查您的瀏覽器是否禁用了Cookie。<a href="http://passport.csdn.net/help/faq">如何啟用Cookie？</a>）</span></td>
                            </tr>
                            <tr>
                                <!–條款–>
                                <td><span class="zhuyi">*</span>註冊條款：</td>
                                <td colspan="2">
                                    <asp:CheckBox ID="CheckBox1" runat="server"
                                        OnCheckedChanged="CheckBox1_CheckedChanged" Text="我已仔細閱讀並接受" /><a href="http://passport.csdn.net/help/terms">CSDN註冊條款</a>。</td>
                            </tr>
                            <!–註冊提交–>
                            <tr>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <asp:ImageButton ID="ImageButton1" runat="server"
                                        ImageUrl="~/images/zhuce.jpg" OnClick="ImageButton1_Click" />
                                    <asp:Label ID="lbinfo" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div>
                        </div>
                    </div>
                </div>
            </div>
            <!–身體結束–>
<!–底部–>
            <div id="footer" align="center">
                <div class="buttom">
                    <a href="http://www.csdn.net/company/about.html" class="abuttom">公司簡介</a>|
                    <a href="http://www.csdn.net/company/recruit.html" class="abuttom">招賢納士</a>|
                    <a href="http://www.csdn.net/company/marketing.html" class="abuttom">廣告服務</a>|
                    <a href="http://www.csdn.net/company/account.html" class="abuttom">銀行匯款賬號</a>|
                    <a href="http://www.csdn.net/company/contact.html" class="abuttom">聯絡方式</a>|
                    <a href="http://www.csdn.net/company/statement.html" class="abuttom">版權宣告</a>|
                    <a href="http://www.csdn.net/company/layer.html" class="abuttom">法律顧問</a>|
                    <a href="" class="abuttom">問題報告</a>
                </div>
                <p class="buttom">京&nbsp&nbspICP&nbsp&nbsp證&nbsp&nbsp070598&nbsp&nbsp號</p>
                <p class="buttom">北京創新樂知資訊科技有限公司&nbsp&nbsp版權所有</p>
                <p class="buttom">
                    <img src="images/zhuce4.gif" />聯絡郵箱：webmaster(at)csdn.net
                </p>
                <p class="buttom">Copyright&nbsp©&nbsp1999-2012,&nbspCSDN.NET,&nbspAll&nbspRights&nbspReserved</p>
                <div>
                    <a href="http://www.hd315.gov.cn/beian/view.asp?bianhao=010202001032100010" class="abuttom">
                        <img src="images/denglu2.gif" /></a>
                    <a href="https://trustsealinfo.verisign.com/splash?form_file=fdf/splash.fdf&dn=passport.csdn.net&lang=zh_cn" class="abuttom">
                        <img src="images/denglu1.gif" /></a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

