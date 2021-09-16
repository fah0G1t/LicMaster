<%@ Page Language="VB" MasterPageFile="~/MasterLic.master" AutoEventWireup="false" CodeFile="ManagControlloPresenze.aspx.vb" Inherits="ManagControlloPresenze" title="Generazione licenze" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table id="htbl_main" cellspacing="2" 
        style="width: 889px; font-family: Verdana;">
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            <fieldset style="width: 869px" class="admin_alter_row" > <legend class="text_in_cell" style="width: 342px" >1.-Richiesta licenza</legend>
            <table class="admin_alter_row" id="htbl_req_lic_info">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblReadRequestEvent" runat="server" Font-Bold="True" 
                                Width="610px" ForeColor="Red"></asp:Label></td>
                        <td colspan="1">
                        </td>
                    </tr>
        <tr>
            <td colspan="4">
                <asp:FileUpload ID="uplLicRequest" runat="server" Width="868px" /></td>
            <td colspan="1">
                <asp:Button ID="cmdUploadAndProcess" runat="server" Text="Carica File richiesta" />
            </td>
        </tr>
        <tr>
            <td colspan="5" style="width: 293px; height: 14px">
                <asp:Label ID="lblError" runat="server" Width="1012px"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 127px; height: 11px;">
                Richiedente</td>
            <td style="width: 154px; height: 11px;">
                telefono</td>
            <td style="width: 249px; height: 11px;">
                e-mail</td>
            <td style="width: 182px; height: 11px;">
                Data richiesta</td>
            <td style="height: 11px">
                </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 48px">
                <asp:TextBox ID="txtRicRagioneSoc" runat="server" BorderStyle="Solid" 
                    BorderWidth="1px" Width="227px"></asp:TextBox>
            </td>
            <td style="width: 154px; height: 48px">
                <asp:TextBox ID="txtRicTelefono" runat="server" BorderStyle="Solid" 
                    BorderWidth="1px" Width="153px"></asp:TextBox>
            </td>
            <td style="width: 249px; height: 48px">
                <asp:TextBox ID="txtRic_e_mail" runat="server" BorderStyle="Solid" 
                    BorderWidth="1px" Width="289px"></asp:TextBox>
            </td>
            <td style="width: 182px; height: 48px">
                        <asp:TextBox ID="txtDateRequest" runat="server" BorderStyle="Solid" 
                    BorderWidth="1px"></asp:TextBox>
                </td>
            <td style="height: 48px">
                </td>
        </tr>
                <tr>
                    <td style="width: 127px; height: 12px">
                        Sistema operativo</td>
                    <td style="width: 154px; height: 12px">
                Computer name</td>
                    <td style="width: 249px; height: 12px">
                Vol.Serial Number</td>
                    <td style="width: 182px; height: 12px">
                Folder</td>
                    <td style="height: 12px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 127px; height: 48px">
                <asp:TextBox ID="txtRic_OSystem" runat="server" BorderStyle="Solid" BorderWidth="1px" 
                            Width="226px"></asp:TextBox>
                    </td>
                    <td style="width: 154px; height: 48px">
                        <asp:TextBox ID="txtRicCompName" runat="server" BorderStyle="Solid" 
                            BorderWidth="1px" Width="156px"></asp:TextBox>
                    </td>
                    <td style="width: 249px; height: 48px">
                        <asp:TextBox ID="txtRicVSN" runat="server" BorderStyle="Solid" 
                            BorderWidth="1px"></asp:TextBox>
                    </td>
                    <td style="width: 182px; height: 48px">
                        <asp:TextBox ID="txtRicAppFolder" runat="server" BorderStyle="Solid" 
                            BorderWidth="1px" Width="219px"></asp:TextBox>
                    </td>
                    <td style="height: 48px">
                        &nbsp;
                    <asp:Button ID="cmdUploadFromForm" runat="server" Text="Leggi dal form" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 79px; height: 48px" colspan="4">
                        <asp:TextBox ID="txtCleanBuffer" runat="server" Height="136px" Rows="8" 
                            TextMode="MultiLine" Width="986px"></asp:TextBox>
                    </td>
                    <td style="height: 48px">
                        &nbsp;</td>
                </tr>
    </table>
             </fieldset>
                
            </td>
        </tr>
        <tr>
            <td>
            <fieldset style="background-color: #CCCCCC; admin_alter_row: ;" > <legend class="text_in_cell" 
                    style="width: 359px" >2.-Clienti registrati</legend>
            <table style="width: 682px">
                    
                    <tr>
                        <td colspan="2" style="width: 448px">
                            <asp:Label ID="lblNewClientEvent" runat="server" Font-Bold="True" Width="610px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 448px">
                            <asp:DropDownList ID="cboClients" runat="server" Width="434px" AutoPostBack="True">
                            </asp:DropDownList></td>
                        <td align="center">
                            <asp:Button ID="cmdCreateNewFromReq" runat="server" Text="Crea nuovo da richiesta"
                                Width="170px" /></td>
                    </tr>
                    <tr>
                        <td style="width: 448px">
                            <asp:Label ID="lblClientInfo" runat="server" Width="435px"></asp:Label></td>
                        <td>
                        </td>
                    </tr>
                </table>
            </fieldset> 
                
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
            <fieldset style="width: 1194px" > 
                <legend class="text_in_cell" 
                    style="width: 534px" >
                3.-Creazione nuova licenza ed elenco licenze rilasciate</legend>
            <table style="width: 1185px">
                    <tr>
                        <td colspan="3" style="width: 211px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 211px" valign="top">
                            <table style="width: 698px">
                                <tr>
                                    <td>
                            Tipo di licenza</td>
                                    <td>
                            <asp:DropDownList ID="cboLicType" runat="server" Width="206px">
                            </asp:DropDownList></td>
                                    <td>
                                        Prodotto:</td>
                                    <td>
                            <asp:DropDownList ID="cboProduct" runat="server" Width="206px" AutoPostBack="True">
                            </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                            Periodo temporale (in giorni)</td>
                                    <td>
                            <asp:TextBox ID="txtDCount" runat="server" Width="199px">30</asp:TextBox></td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                </table>
                        </td>
                        <td style="width: 213px">Nota:<br />
                            <table id="lic_note"><tr><td><asp:TextBox ID="txtLicNota" runat="server" Width="495px" BorderStyle="Solid" 
                                            BorderWidth="1px" Height="96px" TextMode="MultiLine"></asp:TextBox></td></tr></table></td>
                    </tr>
                    <tr>
                        <td style="width: 211px">
                            &nbsp;</td>
                        <td style="width: 213px">
                            <asp:Button ID="cmdCreateLic" runat="server" Text="Crea licenza" Width="170px" />
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 211px">
                            <asp:Label ID="lblNewLicEvent" runat="server" Font-Bold="True" Width="610px"></asp:Label>
                            </td>
                        <td style="width: 213px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 211px">
                            &nbsp;</td>
                        <td style="width: 213px">
                            &nbsp;</td>
                        <td style="width: 200px">
                            &nbsp;</td>
                    </tr>
                <tr>
                    <td colspan="3" style="width: 211px">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Width="678px">Elenco licenze</asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 211px">
                        <asp:Label ID="lblSendLicEvent" runat="server" Font-Bold="True" Width="679px"></asp:Label></td>
                </tr>
                    <tr>
                        <td colspan="3" style="width: 211px">
                            <asp:Table ID="tblClientLic" runat="server" Width="1166px" CellSpacing="3" 
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                            </asp:Table>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </fieldset>
            </td>
        </tr>
    </table>
</asp:Content>

