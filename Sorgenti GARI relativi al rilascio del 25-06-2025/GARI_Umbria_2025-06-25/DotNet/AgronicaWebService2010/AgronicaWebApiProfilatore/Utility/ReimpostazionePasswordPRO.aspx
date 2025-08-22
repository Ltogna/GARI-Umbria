<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReimpostazionePasswordPRO.aspx.vb" Inherits="AgronicaWebApiProfilatore.ReimpostazionePasswordPRO" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Reimposta Password Profitosan</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.3.0/font/bootstrap-icons.css" />
    <link rel="stylesheet" href="css/style.css" />
    <style type="text/css">
        .allineaCentro {
            text-align: center;
        }
    </style>
</head>
<body>
    <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center">
        <img align="center" alt="" src="https://www.profitosan.it/images/logo.png" />
    </div>
    <form id="formNuovaPassword" runat="server">
        <div class="allineaCentro">
            <label class="input-group-addon" id="lblNuovaPassword" for="txtNuovaPassword">
                    <asp:Localize runat="server">Nuova Password</asp:Localize>
            </label>
            <br />
            <div>
                <input type="password" name="txtNuovaPassword" id="txtNuovaPassword" autocomplete="new-password" runat="server"/>
                <i class="bi bi-eye-slash" id="toggleNuovaPassword"></i>
            </div>
        </div>
        <br />
        <div class="allineaCentro">
            <label class="input-group-addon" id="lblRipetiNuovaPassword" for="txtRipetiNuovaPassword">
                    <asp:Localize runat="server">Ripeti Nuova Password</asp:Localize>
            </label>
            <br />
            <input type="password" name="txtRipetiNuovaPassword" id="txtRipetiNuovaPassword" autocomplete="new-password" runat="server"/>
            <i class="bi bi-eye-slash" id="toggleRipetiNuovaPassword"></i>
        </div>
        <br />
        <div style="text-align: center">
            <input type="button" name="btnConferma" id="btnConferma" runat="server" value="Modifica Password" />
        </div>
    </form>
    <script>
        const toggleNuovaPassword = document.querySelector("#toggleNuovaPassword");
        const txtNuovaPassword = document.querySelector("#txtNuovaPassword");

        toggleNuovaPassword.addEventListener("click", function () {
            // toggle the type attribute
            const type = txtNuovaPassword.getAttribute("type") === "password" ? "text" : "password";
            txtNuovaPassword.setAttribute("type", type);
            // toggle the icon
            this.classList.toggle("bi-eye");
        });

        const toggleRipetiNuovaPassword = document.querySelector("#toggleRipetiNuovaPassword");
        const txtRipetiNuovaPassword = document.querySelector("#txtRipetiNuovaPassword");

        toggleRipetiNuovaPassword.addEventListener("click", function () {
            // toggle the type attribute
            const type = txtRipetiNuovaPassword.getAttribute("type") === "password" ? "text" : "password";
            txtRipetiNuovaPassword.setAttribute("type", type);
            // toggle the icon
            this.classList.toggle("bi-eye");
        });


    </script>
</body>
</html>
