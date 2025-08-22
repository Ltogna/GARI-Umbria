<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Custom500.aspx.vb" Inherits="AgronicaStampe_2010.Custom500" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script src="<%= ResolveClientUrl("~/Scripts/jquery-1.8.2.min.js?" & Application("GiasVersioneCorrente").ToString) %>" type="text/javascript"></script>
    <title></title>
    <script language='JavaScript' type='text/javascript'>
        function setCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function getCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
            }
            return null;
        }

        function deleteCookie(name) {
            setCookie(name, "", -1);
        }


        function goURL() {
            location.href = getCookie('paginalogin');
        }


        $(document).ready(function () {
            $('#nomepagina').html(location.href);
        });

        


    </script>
    <style>
        h1
        {
            font-size: 25px;
        }
        h3
        {
            font-size: 20px;
        }
        h4
        {
            font-size: 16px;
        }
    </style>
</head>
<body runat="server" id="body">
    <form id="form1" runat="server">
    <h1>
        <asp:Label ID="errore" runat="server"></asp:Label>
    </h1>
    <asp:PlaceHolder ID="script" runat="server"></asp:PlaceHolder>
    <br />
    <br />
    <asp:Panel ID="pannelloerrore" runat="server" Visible="false">
        <table aria-hidden="true">
            <tr>
                <th nowrap align="left" valign="top">
                    Pagina:
                </th>
                <td align="left" valign="top">
                    <span id="nomepagina"></span>
                </td>
            </tr>
            <% If Len(CStr(objError.innerexception.ToString)) > 0 Then%>
            <tr>
                <th nowrap align="left" valign="top">
                    Errore:
                </th>
                <td align="left" valign="top">
                    <%=objError.innerexception.message.ToString%>
                </td>
            </tr>
            <% End If%>
            <% If Len(CStr(objError.innerexception.ToString)) > 0 Then%>
            <tr>
                <th nowrap align="left" valign="top">
                    Stack:
                </th>
                <td align="left" valign="top">
                    <%=objError.innerexception.stacktrace.ToString%>
                </td>
            </tr>
            <% End If%>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
