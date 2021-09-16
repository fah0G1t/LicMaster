Imports IngenioLicenseManagement
Imports WebCommons
Imports DataUnits1._1
Imports L2005.InCharts

Imports System.Xml

'http://aspalliance.com/articleViewer.aspx?aId=259&pId=-1
'http://www.asptutorial.info/sscript/ContentType.html


Partial Class ManagControlloPresenze
    Inherits System.Web.UI.Page


    Public Enum actions
        none
        idlic
        idlic_dwnld
        idlic_cpy
        idlic_delete
    End Enum


    Dim m_pageInfo As CP_PageInfo

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strError As String = "", bQueryFlag As Boolean = True


        m_pageInfo = CType(WebCommons.pageCommons.GetFromSession(Me), CP_PageInfo)

        WebCommons.pageCommons.ResetError(lblNewClientEvent)
        WebCommons.pageCommons.ResetError(lblNewLicEvent)
        'WebCommons.pageCommons.ResetError(me.

        If Me.m_pageInfo Is Nothing Then

            Me.Header.Title = "Ingenio Controllo distribuzione licenze (Alfa)"

            m_pageInfo = New CP_PageInfo

            If Not Me.m_pageInfo.LoadClients(Me.cboClients, strError) Then
                WebCommons.pageCommons.PrintError(Me.lblNewClientEvent, "Un avento inatteso non ha  permesso di caricare l'anagrafica clienti: " + strError)
            End If

            WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)
            bQueryFlag = True
            'Else
            '    If Not Me.m_pageInfo.LoadClients(Me.cboClients, strError) Then
            '        WebCommons.pageCommons.PrintError(Me.lblNewClientEvent, "Un avento inatteso non ha  permesso di caricare l'anagrafica clienti: " + strError)
            '    End If
        End If

        Dim iIDLic As Integer = 0

        If Not Me.Page.IsPostBack Then
            GetPageInfo()

            If cboClients.Items.Count = 0 Then  'AndAlso Me.m_pageInfo IsNot Nothing
                If Not Me.m_pageInfo.LoadClients(Me.cboClients, strError) Then
                    WebCommons.pageCommons.PrintError(Me.lblNewClientEvent, "Un avento inatteso non ha  permesso di caricare l'anagrafica clienti: " + strError)
                End If
            End If

            If cboProduct.Items.Count = 0 Then
                '====================================================================================================
                Dim duProducts As New DataUnit("Prodotti", LicDB.db_struct.DBConn)

                If duProducts.ExecuteSelect(strError) Then
                    WebCommons.controls.FillCombo(Me.cboProduct, duProducts.GetDataSet, "nome", "id_prodotto", True)
                End If
                '====================================================================================================
            End If


            Dim eAction As actions = actions.none

            If Array.IndexOf(Me.Request.Params.AllKeys, actions.idlic.ToString) <> -1 Then 'c'è da gestire un'invio di licenza

                eAction = actions.idlic

            ElseIf Array.IndexOf(Me.Request.Params.AllKeys, actions.idlic_delete.ToString) <> -1 Then 'c'è da cancellare licenza

                eAction = actions.idlic_delete

            ElseIf Array.IndexOf(Me.Request.Params.AllKeys, actions.idlic_dwnld.ToString) <> -1 Then 'c'è da gestire un download di licenza

                eAction = actions.idlic_dwnld

            ElseIf Array.IndexOf(Me.Request.Params.AllKeys, actions.idlic_cpy.ToString) <> -1 Then 'edita una licenza

                eAction = actions.idlic_cpy

            End If

            If eAction <> actions.none Then
                Try
                    iIDLic = Me.Request.Params(eAction.ToString)
                Catch ex As Exception
                    iIDLic = 0
                End Try

                If iIDLic > 0 Then

                    Dim strFilePath As String = ""
                    Dim strLicNote As String = ""

                    If Me.m_pageInfo.m_Lic.Load(iIDLic, strLicNote, strError) Then
                        Select Case eAction
                            Case actions.idlic

                                If Me.m_pageInfo.m_Lic.CreateLicFile(MapPath("attachments_yard"), strFilePath) Then
                                    If Not Me.m_pageInfo.m_Lic.SendToUser(strFilePath, strError) Then
                                        WebCommons.pageCommons.PrintError(Me.lblSendLicEvent, "Non è stato possibile inviare licenza per mail: " + strError)
                                    Else
                                        WebCommons.pageCommons.PrintError(Me.lblSendLicEvent, "Licenza inviata")
                                    End If
                                Else
                                    strError = strFilePath
                                    WebCommons.pageCommons.PrintError(Me.lblSendLicEvent, "Non è stato possibile creare l'attachment da inviare per mail: " + strError)
                                End If

                            Case actions.idlic_delete

                                If Not Me.m_pageInfo.m_Lic.Delete(strError) Then
                                    WebCommons.pageCommons.PrintError(Me.lblSendLicEvent, "Non è stato possibile cancellare la licenza: " + strError)
                                Else
                                    WebCommons.pageCommons.PrintError(Me.lblSendLicEvent, "Licenza cancellata")
                                End If

                            Case actions.idlic_cpy

                                Me.m_pageInfo.m_cur_customer_id = Me.m_pageInfo.m_Lic.m_id_customer
                                Me.m_pageInfo.m_product_id = CType(Me.m_pageInfo.m_Lic.m_id_product, CP_PageInfo.prod_code)

                                Me.txtRicCompName.Text = Me.m_pageInfo.m_Lic.m_computer ' Request.m_ComputerName
                                Me.txtRicRagioneSoc.Text = Me.m_pageInfo.m_Lic.m_customer ' Request.m_Customer
                                Me.txtRicVSN.Text = Me.m_pageInfo.m_Lic.m_vsn 'Request.m_VSN

                                Me.txtRicTelefono.Text = Me.m_pageInfo.m_Lic.m_CustomerPhone
                                Me.txtRic_e_mail.Text = Me.m_pageInfo.m_Lic.m_CustomerEmail
                                Me.txtDateRequest.Text = DateTime.Now.Date
                                Me.txtRicAppFolder.Text = Me.m_pageInfo.m_Lic.m_app_dir '  Request.m_StartupDir
                                Me.txtRic_OSystem.Text = Me.m_pageInfo.m_Lic.m_os_string

                                Me.txtLicNota.Text = strLicNote

                                Try
                                    cboClients.SelectedValue = Me.m_pageInfo.m_cur_customer_id.ToString()
                                Catch ex As Exception

                                End Try
                                Try
                                    cboProduct.SelectedValue = Convert.ToInt32(Me.m_pageInfo.m_product_id).ToString()
                                Catch ex As Exception

                                End Try

                                Try
                                    cboLicType.SelectedValue = Convert.ToInt32(Me.m_pageInfo.m_Lic.m_ltype).ToString()
                                Catch ex As Exception

                                End Try



                            Case actions.idlic_dwnld

                                If Me.m_pageInfo.m_Lic.CreateLicFile(MapPath("attachments_yard"), strFilePath) Then
                                    Dim strName As String = Me.m_pageInfo.m_Lic.FileName()

                                    Dim iFileSize As Long = FileLen(strFilePath)

                                    Response.Clear()
                                    Response.AppendHeader("content-disposition", "attachment; filename=" + strName)
                                    Response.ContentType = "application/x-msdownload"
                                    'Response.AddHeader("Content-Disposition", strName)
                                    Response.AddHeader("Content-Length", iFileSize)
                                    Response.CacheControl = "no-cache"
                                    Response.WriteFile(strFilePath)
                                    Response.Flush()
                                    Response.End()
                                Else
                                    strError = strFilePath
                                    WebCommons.pageCommons.PrintError(Me.lblSendLicEvent, "Non è stato possibile creare file da scaricare: " + strError)
                                End If


                        End Select
                    Else
                        WebCommons.pageCommons.PrintError(Me.lblSendLicEvent, "Non posso leggere i dati della licenza: " + strError)
                    End If
                End If
            End If


            WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)
        End If


        Try
            Me.m_pageInfo.DumpLicenses(Me.tblClientLic, bQueryFlag, CInt(Me.m_pageInfo.m_product_id), cboClients.SelectedValue)
        Catch ex As Exception

        End Try

    End Sub


    Private Sub GetPageInfo()
        Dim strError As String = ""

        If Me.cboLicType.Items.Count = 0 Then
            Dim fo As New WebCommons.FillControlOptions

            Dim duLicType As New DataUnit(LicDB.db_struct.lc_tables.tipi_licenze_dsk, LicDB.db_struct.DBConn)

            duLicType.ExecuteSelect(strError)

            fo.bSetNullOption = False

            fo.CaptionFieldList.Clear()
            fo.CaptionFieldList.Add(LicDB.db_struct.tbl_dsk_lic_types.nome_tipo.ToString)
            fo.ValueField = LicDB.db_struct.tbl_dsk_lic_types.id_tipo.ToString

            WebCommons.controls.FillCombo(Me.cboLicType, duLicType.GetDataSet, fo)
        End If


        If Me.m_pageInfo.m_ClientLics.Data Is Nothing Then
            Dim chartManager As New L2005.InCharts.ChartsManagement(Me.MapPath("app_config/charts.xml"))

            If chartManager.LoadOK Then
                Me.m_pageInfo.m_chartOpts.tuning.OrderBy = String.Format("{0},{1}", LicDB.db_struct.tbl_clienti.nome_cliente.ToString, LicDB.db_struct.tbl_dsktop_lic.data_send.ToString)
                Me.m_pageInfo.m_ClientLics.bRunningOnDev = False 'WebCommons.pageCommons.IsDevelopmentEnv(Me)
                Me.m_pageInfo.m_ClientLics.SetChart("desktop_licenses", chartManager.xmlCharts, Me.m_pageInfo.m_chartOpts, strError)
            End If
        End If

    End Sub


    Protected Sub cboClients_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboClients.SelectedIndexChanged

        Me.m_pageInfo.m_cur_customer_id = Me.cboClients.SelectedValue

        If Me.m_pageInfo.m_Lic IsNot Nothing Then
            Me.m_pageInfo.m_Lic.Clear()
        End If

        WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)

        Me.m_pageInfo.DumpLicenses(Me.tblClientLic, True, Me.m_pageInfo.m_product_id, Me.cboClients.SelectedValue)
        Dim arr As New ArrayList

        If Me.cboClients.SelectedIndex > 0 Then
            If Commons.Strings.GetTokenFrom(Me.cboClients.SelectedItem.Text, ", ", arr) Then
                If arr.Count >= 3 Then
                    Me.lblClientInfo.Text = String.Format("telefono: <b>{0}</b> e-mail: <b>{1}</b> ", arr(1), arr(2))
                End If
            End If

            Me.m_pageInfo.DumpLicenses(Me.tblClientLic, True, CInt(Me.m_pageInfo.m_product_id), Me.cboClients.SelectedValue)
        Else
            Me.lblClientInfo.Text = ""
        End If


    End Sub

    Protected Sub cboProduct_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProduct.SelectedIndexChanged
        Try
            Me.m_pageInfo.m_product_id = CType(CInt(cboProduct.SelectedValue), CP_PageInfo.prod_code)
            WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)
        Catch ex As Exception

        End Try

        Me.m_pageInfo.DumpLicenses(Me.tblClientLic, True, Me.m_pageInfo.m_product_id, cboClients.SelectedValue)

    End Sub


    Protected Sub cmdCreateLic_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCreateLic.Click

        If cboClients.SelectedIndex > 0 Then
            Me.m_pageInfo.m_product_id = CType(CInt(cboProduct.SelectedValue), CP_PageInfo.prod_code)
        Else
            WebCommons.pageCommons.PrintError(Me.lblNewLicEvent, "Devi scegliere un prodotto a cui assegnare la licenza")
            Exit Sub
        End If


        If CInt(Me.cboClients.SelectedValue) <> 0 Then
            Dim duLics As New DataUnit(LicDB.db_struct.lc_tables.desktop_licenses, LicDB.db_struct.DBConn)
            Dim strError As String = ""

            duLics.SetIdentityKey(LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString, strError)

            Me.m_pageInfo.m_Lic.m_ltype = CType(CInt(Me.cboLicType.SelectedValue), DesktopLicense.lic_type)

            If Me.m_pageInfo.m_Lic.m_ltype = DesktopLicense.lic_type.lc_temp Then
                Dim iTest As Integer
                Try
                    iTest = Me.txtDCount.Text
                Catch ex As Exception
                    WebCommons.pageCommons.PrintError(lblNewLicEvent, "Il periodo temporale in giorni non è corretto")
                    Exit Sub
                End Try

                Me.m_pageInfo.m_Lic.m_dcount = iTest

                If Not String.IsNullOrEmpty(txtLicNota.Text) Then
                    txtLicNota.Text += vbCrLf
                End If

                Dim dtExpires As Date = Now
                dtExpires = dtExpires.AddDays(iTest)

                txtLicNota.Text += String.Format("Se installata {0} scade {1}", Now.Date.ToString(), dtExpires.ToString())

            ElseIf Me.m_pageInfo.m_Lic.m_ltype = DesktopLicense.lic_type.lc_normal Then
                Me.m_pageInfo.m_Lic.m_dcount = 0
            End If

            duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.dcount.ToString, Me.m_pageInfo.m_Lic.m_dcount)

            duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.id_cliente.ToString, Me.cboClients.SelectedValue)
            duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString, CInt(Me.m_pageInfo.m_product_id))
            duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.data_send.ToString(), Now)

            If Not String.IsNullOrEmpty(txtLicNota.Text) Then
                duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.nota.ToString, txtLicNota.Text)
            End If

            'If Not String.IsNullOrEmpty(Me.m_pageInfo.m_LicRequest.m_VSN) Then
            '    duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.vsn.ToString, Me.m_pageInfo.m_LicRequest.m_VSN)
            '    duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.c_name.ToString, Me.m_pageInfo.m_LicRequest.m_ComputerName)
            '    duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.clnt_app_dir.ToString, Me.m_pageInfo.m_LicRequest.m_StartupDir)
            '    duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.data_req.ToString, Me.m_pageInfo.m_LicRequest.m_requestDate)
            '    duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.os.ToString, Me.m_pageInfo.m_LicRequest.m_OsVersionString)

            Dim reqDate As Date
            Try
                reqDate = Date.Parse(Me.txtDateRequest.Text)
            Catch ex As Exception
                reqDate = Now
                Me.txtDateRequest.Text = Now
            End Try


            If Not String.IsNullOrEmpty(txtRicVSN.Text) Then
                duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.vsn.ToString, txtRicVSN.Text)
                duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.c_name.ToString, txtRicCompName.Text)
                duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.clnt_app_dir.ToString, Me.txtRicAppFolder.Text)
                duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.data_req.ToString, Now.Date)
                duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.os.ToString, Me.txtRic_OSystem.Text)
            Else
                WebCommons.pageCommons.PrintError(lblNewLicEvent, "Non ci sono dati da dove creare licenza")
                Exit Sub
            End If

            If Not duLics.ExecuteInsert(strError) Then
                WebCommons.pageCommons.PrintError(lblNewLicEvent, strError)
            Else
                txtLicNota.Text = ""
                WebCommons.pageCommons.PrintError(lblNewLicEvent, "Creazione eseguita")
                Me.m_pageInfo.DumpLicenses(Me.tblClientLic, True, CInt(Me.m_pageInfo.m_product_id), Me.cboClients.SelectedValue)
            End If
        Else
            WebCommons.pageCommons.PrintError(Me.lblNewLicEvent, "Devi scegliere un cliente a cui assegnare la licenza")
        End If

    End Sub

    Protected Sub cmdCreateNewFromReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCreateNewFromReq.Click
        Dim duClients As New DataUnit(LicDB.db_struct.lc_tables.Clienti, LicDB.db_struct.DBConn)
        Dim strError As String = ""

        duClients.SetIdentityKey(LicDB.db_struct.tbl_clienti.id_client.ToString, strError)

        duClients.SetInData(LicDB.db_struct.tbl_clienti.email.ToString, Me.m_pageInfo.m_LicRequest.m_EMail)
        duClients.SetInData(LicDB.db_struct.tbl_clienti.nome_cliente.ToString, Me.m_pageInfo.m_LicRequest.m_Customer)
        duClients.SetInData(LicDB.db_struct.tbl_clienti.telefono.ToString, Me.m_pageInfo.m_LicRequest.m_Telefono)

        If Not duClients.ExecuteInsert(strError) Then
            WebCommons.pageCommons.PrintError(lblNewClientEvent, strError)
        Else
            WebCommons.pageCommons.PrintError(lblNewClientEvent, "Creazione eseguita")
            Me.m_pageInfo.LoadClients(Me.cboClients, strError)
        End If

    End Sub

    Protected Sub cmdUploadAndProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUploadAndProcess.Click

        If Me.uplLicRequest.PostedFile IsNot Nothing Then

            Dim bBuffer = New Byte(CType(Me.uplLicRequest.PostedFile.ContentLength, Integer)) {}
            Dim strCrypted As String = "", dtrError As String = ""
            Try
                Me.uplLicRequest.PostedFile.InputStream.Read(bBuffer, 0, Me.uplLicRequest.PostedFile.ContentLength)
            Catch ex As Exception
                dtrError = ex.Message
                WebCommons.pageCommons.PrintError(Me.lblReadRequestEvent, "Read posted file: " + ex.Message)
            End Try

            For iByteArray As Integer = 0 To Me.uplLicRequest.PostedFile.ContentLength - 1
                Try
                    strCrypted += Convert.ToString(Chr(bBuffer(iByteArray)))
                Catch ex As Exception
                    dtrError = ex.Message
                    strCrypted = ""
                    WebCommons.pageCommons.PrintError(Me.lblReadRequestEvent, "Scanning posted file: " + ex.Message)
                End Try
            Next iByteArray

            If Not String.IsNullOrEmpty(strCrypted) Then
                'Dim requestInfo As New DesktopLicRequest
                Dim strCleanBuffer As String = ""
                Dim bGetInfoOk As Boolean = Me.m_pageInfo.m_LicRequest.GetLicRequestInfo(strCrypted, dtrError, strCleanBuffer)

                strCleanBuffer = strCleanBuffer.Replace("<?xml version=""1.0""?><config>", "")
                strCleanBuffer = strCleanBuffer.Replace("</config>", "")

                For iItem As DesktopLicRequest.request_struct = DesktopLicRequest.request_struct.lversion To DesktopLicRequest.request_struct.os
                    strCleanBuffer = strCleanBuffer.Replace(String.Format("<{0}>", iItem.ToString()), String.Format("^{0}=", iItem.ToString()))
                    strCleanBuffer = strCleanBuffer.Replace(String.Format("</{0}>", iItem.ToString()), "")
                Next

                If bGetInfoOk Then
                    Me.txtRicCompName.Text = Me.m_pageInfo.m_LicRequest.m_ComputerName
                    Me.txtRicRagioneSoc.Text = Me.m_pageInfo.m_LicRequest.m_Customer
                    Me.txtRicVSN.Text = Me.m_pageInfo.m_LicRequest.m_VSN

                    Me.txtRicTelefono.Text = Me.m_pageInfo.m_LicRequest.m_Telefono
                    Me.txtRic_e_mail.Text = Me.m_pageInfo.m_LicRequest.m_EMail
                    Me.txtDateRequest.Text = Me.m_pageInfo.m_LicRequest.m_requestDate
                    Me.txtRicAppFolder.Text = Me.m_pageInfo.m_LicRequest.m_StartupDir
                    Me.txtRic_OSystem.Text = Me.m_pageInfo.m_LicRequest.m_OsVersionString

                    WebCommons.pageCommons.PutOnSession(Me, Me.m_pageInfo)
                Else


                    Dim iIndex As Integer = -1, strTokenValue As String = "", strTokenKey As String = ""
                    Dim arr As String() = strCleanBuffer.Split("^")

                    For iItem As DesktopLicRequest.request_struct = DesktopLicRequest.request_struct.lversion To DesktopLicRequest.request_struct.os

                        strTokenKey = iItem.ToString()

                        'Dim items = From s In arrayofitems Where s = "two" Select s Take 1

                        iIndex = Array.FindIndex(arr, (Function(c As String) c.StartsWith(strTokenKey)))

                        If iIndex >= 0 Then
                            strTokenValue = arr(iIndex)
                            strTokenValue = strTokenValue.Replace(String.Format("{0}=", strTokenKey), "")
                            SetValue(iItem, strTokenValue)
                        End If
                    Next

                    WebCommons.pageCommons.PrintError(Me.lblReadRequestEvent, dtrError)
                End If

                Me.txtCleanBuffer.Text = strCleanBuffer.Replace("^", vbCrLf)
            End If
        End If

    End Sub

    Protected Sub cmdUploadFromForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUploadFromForm.Click
        Me.m_pageInfo.m_LicRequest.m_ComputerName = Me.txtRicCompName.Text
        Me.m_pageInfo.m_LicRequest.m_Customer = Me.txtRicRagioneSoc.Text
        Me.m_pageInfo.m_LicRequest.m_VSN = Me.txtRicVSN.Text

        Me.m_pageInfo.m_LicRequest.m_Telefono = Me.txtRicTelefono.Text
        Me.m_pageInfo.m_LicRequest.m_EMail = Me.txtRic_e_mail.Text
        Me.m_pageInfo.m_LicRequest.m_OsVersionString = Me.txtRic_OSystem.Text

        Try
            Me.m_pageInfo.m_LicRequest.m_requestDate = Me.txtDateRequest.Text
        Catch ex As Exception

        End Try

        Me.m_pageInfo.m_LicRequest.m_StartupDir = Me.txtRicAppFolder.Text
    End Sub

    Private Sub SetValue(ByVal pRecIdItem As DesktopLicRequest.request_struct, ByVal pStrValue As String)
        Select Case pRecIdItem
            Case DesktopLicRequest.request_struct.app_dir
                Me.txtRicAppFolder.Text = pStrValue
            Case DesktopLicRequest.request_struct.computer
                Me.txtRicCompName.Text = pStrValue
            Case DesktopLicRequest.request_struct.customer
                Me.txtRicRagioneSoc.Text = pStrValue
            Case DesktopLicRequest.request_struct.data
                Me.txtDateRequest.Text = pStrValue
            Case DesktopLicRequest.request_struct.email
                Me.txtRic_e_mail.Text = pStrValue
            Case DesktopLicRequest.request_struct.lversion

            Case DesktopLicRequest.request_struct.os
                Me.txtRic_OSystem.Text = pStrValue
            Case DesktopLicRequest.request_struct.telefono
                Me.txtRicTelefono.Text = pStrValue
            Case DesktopLicRequest.request_struct.vsn
                Me.txtRicVSN.Text = pStrValue
        End Select
    End Sub
    
    
 

    
End Class


Friend Class CP_PageInfo

    Public Enum prod_code
        none
        Incasa = 1
        InTempo
        City_Post
    End Enum



    Public m_LicRequest As New DesktopLicRequest
    Public m_Lic As New DesktopLicense
    Public m_ClientLics As New L2005.InCharts.Chart
    Public m_chartOpts As New L2005.InCharts.ChartOptions
    'Public m_product_id As prod_code = prod_code.InTempo '2 'Presenze
    Public m_product_id As prod_code = prod_code.none  '2 'Presenze
    Public m_cur_customer_id As Integer = 0

    '
    Public m_id_licenza As Integer

    Public Sub New()
        m_chartOpts.m_strConn = LicDB.db_struct.DBConn

    End Sub


    Public Function LoadClients(ByVal cbo As DropDownList, ByVal pError As String) As Boolean
        Dim fo As New WebCommons.FillControlOptions
        Dim bRetval As Boolean = False

        Dim duclients As New DataUnit(LicDB.db_struct.lc_tables.Clienti, LicDB.db_struct.DBConn)
        Dim where_order As New SQLHint(, LicDB.db_struct.tbl_clienti.nome_cliente.ToString())

        Dim strError As String = ""

        If duclients.ExecuteSelect(strError, where_order) Then
            fo.bClearBeforeLoad = True
            fo.bSetNullOption = True
            fo.NullOptionCaption = "<nuovo>"

            fo.CaptionFieldList.Add(LicDB.db_struct.tbl_clienti.nome_cliente.ToString)
            fo.CaptionFieldList.Add(LicDB.db_struct.tbl_clienti.telefono.ToString)
            fo.CaptionFieldList.Add(LicDB.db_struct.tbl_clienti.email.ToString)

            fo.CaptionSeparatorChar = ", "

            fo.ValueField = LicDB.db_struct.tbl_clienti.id_client.ToString

            bRetval = WebCommons.controls.FillCombo(cbo, duclients.GetDataSet, fo)
        End If

        Return bRetval
    End Function


    Public Function DumpLicenses(ByVal tbl As Table, Optional ByVal pRequeryBefore As Boolean = False, _
                                                    Optional ByVal pCurProd As Integer = 0, _
                                                    Optional ByVal pCurClient As Integer = 0) As Boolean

        m_chartOpts.m_bQueryBeforeDraw = pRequeryBefore

        m_chartOpts.pRowsByPage = 20

        'm_chartOpts.fixed_tuning.WhereText = String.Format("dsk_lc.{0}={1}", LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString, CInt(m_product_id))

        m_chartOpts.tuning.WhereText = ""

        If pCurProd > 0 Then
            m_chartOpts.tuning.Add_WhereField(SQLHint.field_type.int, "dsk_lc." + LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString, pCurProd)
        End If

        If pCurClient > 0 Then
            m_chartOpts.tuning.Add_WhereField(SQLHint.field_type.int, "dsk_lc." + LicDB.db_struct.tbl_dsktop_lic.id_cliente.ToString, pCurClient)
        End If

        Dim strError As String = ""

        m_ClientLics.DrawChart(tbl, strError, m_chartOpts)

        'If Not m_ClientLics.DrawChart(tbl, strError, m_chartOpts) Then
        '    WebCommons.pageCommons.AddErrorRow(tbl, strError)
        'End If

    End Function

    Public Function DumpLicenses(ByVal tbl As Table, ByVal pIdLic As Integer, Optional ByVal pRequeryBefore As Boolean = False) As Boolean

        m_chartOpts.m_bQueryBeforeDraw = pRequeryBefore

        m_chartOpts.pRowsByPage = 20

        'm_chartOpts.fixed_tuning.WhereText = String.Format("dsk_lc.{0}={1}", LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString, CInt(m_product_id))

        m_chartOpts.tuning.WhereText = ""

        If pIdLic > 0 Then
            m_chartOpts.tuning.Add_WhereField(SQLHint.field_type.int, "dsk_lc." + LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString, pIdLic)
        End If

        Dim strError As String = ""

        m_ClientLics.DrawChart(tbl, strError, m_chartOpts)

        'If Not m_ClientLics.DrawChart(tbl, strError, m_chartOpts) Then
        '    WebCommons.pageCommons.AddErrorRow(tbl, strError)
        'End If

    End Function
End Class