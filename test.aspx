<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="MKForum.test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="js/jquery.js"></script>
    <script>
        var pageIndex = 1;
        var pageCount;

        //------------------------------
        $(window).scroll(function () {
            if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                GetRecords();
            }
        });
        //------------------------------

        function GetRecords() {
            pageIndex++;
            if (pageIndex == 2 || pageIndex <= pageCount) {
                $("#loader").show();
                $.ajax({
                    type: "POST",
                    url: "CbtoPost.aspx/GetCustomers",
                    data: '(pageIndex:' + pageIndex + ')',
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: Onsuccess,
                    failure: function (response) {
                        alert(response.d);
                    },
                    error: function (response) {
                        alert(response.d);
                    }
                });
            }
        }
        function OnSuccess(response) {


        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
<script>

</script>

</html>
