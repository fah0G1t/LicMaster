<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table align="center" style="width: 372px">
            <tr>
                <td colspan="2" height="40" valign="bottom">
                    <asp:Label ID="lbl_benvenuto" runat="server" CssClass="page_name" Width="360px"></asp:Label></td>
            </tr>
            <tr>
                <td height="40" style="width: 412px" colspan="2">
                    <asp:Label ID="lbl_login" runat="server" CssClass="table_title" Width="366px">Sezione amministrativa</asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 412px" colspan="2">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 21px; height: 24px">
                                <asp:Label ID="lbl_mail" runat="server" CssClass="input" Width="69px" Font-Bold="True">login</asp:Label></td>
                            <td style="height: 24px">
                                <asp:TextBox ID="txt_log" runat="server" CssClass="inserimento"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 21px; height: 20px">
                                <asp:Label ID="Label1" runat="server" CssClass="input" Width="70px" Font-Bold="True">Password</asp:Label></td>
                            <td style="height: 20px">
                                <asp:TextBox ID="txt_password" runat="server" CssClass="inserimento" TextMode="Password"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 21px; height: 20px">
                                &nbsp;</td>
                            <td style="height: 20px">
                                <asp:Label ID="lblVer" runat="server" Text="lblVer"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 412px; height: 13px" colspan="2">
                    <asp:Label ID="lbl_error" runat="server" CssClass="error_signal"></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td valign="bottom" style="width: 95px; height: 40px">
                    &nbsp;
                    <asp:Button ID="cmdLogin" runat="server" Text="Log in" Width="72px" /></td>
                <td style="width: 125px; height: 40px" valign="bottom">
                    <asp:HyperLink ID="lnkHomePage" runat="server" CssClass="linkScheda2" meta:resourceKey="lnk_password_forgot"
                        NavigateUrl="../home.aspx">Home page</asp:HyperLink></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
