<%@ Page Title="" Language="VB" MasterPageFile="~/MasterLic.master" AutoEventWireup="false" CodeFile="AccessiClienti.aspx.vb" Inherits="AccessiClienti" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        <table style="width:100%;">
            <tr>
                <td>
                    <table id="htbl_search" 
                        style="width: 28%; font-family: Verdana; font-weight: bold;">
                        <tr>
                            <td style="width: 114px">
                                Prodotto</td>
                            <td>
                                <asp:DropDownList ID="cboProdsInfo" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 114px">
                                Licenza</td>
                            <td>
                                <asp:DropDownList ID="cboLicsInfo" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 114px">
                                IP</td>
                            <td>
                                <asp:DropDownList ID="cboIPInfo" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 114px">
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="cmdSearch" runat="server" Text="Filtra" Width="74px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Table ID="tblAccessData" runat="server" Width="1173px">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </p>
</asp:Content>

