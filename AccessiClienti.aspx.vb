Imports WebCommons
Imports DataUnits1._1


Partial Class AccessiClienti
    Inherits System.Web.UI.Page

    Dim m_pageInfo As Stats_PageInfo


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strError As String = "", bQueryFlag As Boolean = True

        m_pageInfo = CType(WebCommons.pageCommons.GetFromSession(Me), Stats_PageInfo)

        If m_pageInfo Is Nothing Then
            Me.m_pageInfo = New Stats_PageInfo(Me)
            WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)
        End If

        PopulateLists()


        Try
            Me.m_pageInfo.DumpAccess(Me.tblAccessData, bQueryFlag)   'cboClients.SelectedValue
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)
        Me.m_pageInfo.DumpAccess(Me.tblAccessData, True, cboProdsInfo.SelectedValue, cboLicsInfo.SelectedValue, cboIPInfo.SelectedValue)   'cboClients.SelectedValue
    End Sub

    Private Sub PopulateLists()
        Dim strError As String = ""
        Dim duStats As New DataUnits1._1.DataUnit(LicDB.db_struct.lc_tables.licenses_stats.ToString(), LicDB.db_struct.DBConn)
        Dim where_order As New SQLHint
        Dim fo As New WebCommons.FillControlOptions, bRetval As Boolean

        fo.bClearBeforeLoad = True
        fo.bSetNullOption = True
        fo.NullOptionCaption = "<tutti>"

        If cboProdsInfo.Items.Count = 0 Then
            fo.CaptionFieldList.Clear()
            where_order.AddModifiers(SQLHint.select_functions.distinct, LicDB.db_struct.tbl_products_access.ProductCode.ToString())
            'where_order.Add_WhereField (SQLHint.field_type.tstring ,
            where_order.OrderBy = LicDB.db_struct.tbl_products_access.ProductCode.ToString()

            If duStats.ExecuteSelect(strError, where_order) Then
                fo.CaptionFieldList.Add(LicDB.db_struct.tbl_products_access.ProductCode.ToString)
                fo.CaptionSeparatorChar = ", "

                fo.ValueField = LicDB.db_struct.tbl_products_access.ProductCode.ToString

                bRetval = WebCommons.controls.FillCombo(cboProdsInfo, duStats.GetDataSet, fo)
            End If

            where_order.ResetModifiers()
        End If



        'If cboLicsInfo.Items.Count = 0 Then
        '    where_order.AddModifiers(SQLHint.select_functions.distinct, LicDB.db_struct.tbl_products_access.License.ToString())
        '    where_order.OrderBy = LicDB.db_struct.tbl_products_access.License.ToString()

        '    If duStats.ExecuteSelect(strError, where_order) Then
        '        fo.CaptionFieldList.Add(LicDB.db_struct.tbl_products_access.License.ToString)
        '        fo.CaptionSeparatorChar = ", "

        '        fo.ValueField = LicDB.db_struct.tbl_products_access.License.ToString

        '        bRetval = WebCommons.controls.FillCombo(cboLicsInfo, duStats.GetDataSet, fo)
        '    End If


        'End If

        'If cboIPInfo.Items.Count = 0 Then
        '    fo.CaptionFieldList.Clear()
        '    where_order.AddModifiers(SQLHint.select_functions.distinct, LicDB.db_struct.tbl_products_access.Ip.ToString())
        '    where_order.OrderBy = LicDB.db_struct.tbl_products_access.Ip.ToString()

        '    If duStats.ExecuteSelect(strError, where_order) Then
        '        fo.CaptionFieldList.Add(LicDB.db_struct.tbl_products_access.Ip.ToString)
        '        fo.CaptionSeparatorChar = ", "

        '        fo.ValueField = LicDB.db_struct.tbl_products_access.Ip.ToString

        '        bRetval = WebCommons.controls.FillCombo(cboIPInfo, duStats.GetDataSet, fo)
        '    End If

        '    where_order.ResetModifiers()
        'End If

    End Sub


    Protected Sub cboProdsInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProdsInfo.SelectedIndexChanged
        cboLicsInfo.Items.Clear()

        Dim strError As String = ""
        Dim duStats As New DataUnits1._1.DataUnit(LicDB.db_struct.lc_tables.licenses_stats.ToString(), LicDB.db_struct.DBConn)
        Dim where_order As New SQLHint
        Dim fo As New WebCommons.FillControlOptions, bRetval As Boolean

        fo.bClearBeforeLoad = True
        fo.bSetNullOption = True
        fo.NullOptionCaption = "<tutti>"

        where_order.AddModifiers(SQLHint.select_functions.distinct, LicDB.db_struct.tbl_products_access.License.ToString())
        where_order.OrderBy = LicDB.db_struct.tbl_products_access.License.ToString()

        If cboProdsInfo.SelectedIndex > 0 Then
            where_order.Add_WhereField(SQLHint.field_type.tstring, LicDB.db_struct.tbl_products_access.ProductCode.ToString(), cboProdsInfo.SelectedValue)
        End If

        If duStats.ExecuteSelect(strError, where_order) Then
            fo.CaptionFieldList.Add(LicDB.db_struct.tbl_products_access.License.ToString)
            fo.CaptionSeparatorChar = ", "

            fo.ValueField = LicDB.db_struct.tbl_products_access.License.ToString

            bRetval = WebCommons.controls.FillCombo(cboLicsInfo, duStats.GetDataSet, fo)
        End If

        WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)
        Me.m_pageInfo.DumpAccess(Me.tblAccessData, True, cboProdsInfo.SelectedValue, cboLicsInfo.SelectedValue, cboIPInfo.SelectedValue)   'cboClients.SelectedValue

    End Sub

    Protected Sub cboLicsInfo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLicsInfo.SelectedIndexChanged
        cboIPInfo.Items.Clear()

        Dim strError As String = ""
        Dim duStats As New DataUnits1._1.DataUnit(LicDB.db_struct.lc_tables.licenses_stats.ToString(), LicDB.db_struct.DBConn)
        Dim where_order As New SQLHint
        Dim fo As New WebCommons.FillControlOptions, bRetval As Boolean

        fo.bClearBeforeLoad = True
        fo.bSetNullOption = True
        fo.NullOptionCaption = "<tutti>"

        fo.CaptionFieldList.Clear()
        where_order.AddModifiers(SQLHint.select_functions.distinct, LicDB.db_struct.tbl_products_access.Ip.ToString())
        where_order.OrderBy = LicDB.db_struct.tbl_products_access.Ip.ToString()

        If cboLicsInfo.SelectedIndex > 0 Then
            where_order.Add_WhereField(SQLHint.field_type.tstring, LicDB.db_struct.tbl_products_access.License.ToString(), cboLicsInfo.SelectedValue)
        End If

        If duStats.ExecuteSelect(strError, where_order) Then
            fo.CaptionFieldList.Add(LicDB.db_struct.tbl_products_access.Ip.ToString)
            fo.CaptionSeparatorChar = ", "

            fo.ValueField = LicDB.db_struct.tbl_products_access.Ip.ToString

            bRetval = WebCommons.controls.FillCombo(cboIPInfo, duStats.GetDataSet, fo)
        End If

        WebCommons.pageCommons.PutOnSession(Me, m_pageInfo)
        Me.m_pageInfo.DumpAccess(Me.tblAccessData, True, cboProdsInfo.SelectedValue, cboLicsInfo.SelectedValue, cboIPInfo.SelectedValue)   'cboClients.SelectedValue

    End Sub
End Class


Friend Class Stats_PageInfo

    Public Enum prod_code
        none
        Incasa = 1
        InTempo
        City_Post
    End Enum



    Public m_ClientAccess As New L2005.InCharts.Chart
    Public m_chartOpts As New L2005.InCharts.ChartOptions
    'Public m_product_id As prod_code = prod_code.InTempo '2 'Presenze
    Public m_product_id As prod_code = prod_code.none  '2 'Presenze
    Public m_cur_customer_id As Integer = 0

    Private m_client_page As Page

    '
    Public m_id_licenza As Integer

    Public Sub New(ByVal pPage As Page)
        m_chartOpts.m_strConn = LicDB.db_struct.DBConn

        m_client_page = pPage
    End Sub



    Public Function DumpAccess(ByVal tbl As Table, Optional ByVal pRequeryBefore As Boolean = False, _
                                                    Optional ByVal pProdInfo As String = "", _
                                                    Optional ByVal pLicInfo As String = "", _
                                                    Optional ByVal pIPInfo As String = "") As Boolean


        m_chartOpts.m_bQueryBeforeDraw = pRequeryBefore
        m_chartOpts.tuning.WhereText = ""

        m_chartOpts.pRowsByPage = 20

        If Not String.IsNullOrEmpty(pProdInfo) AndAlso pProdInfo.Trim() <> "0" Then
            m_chartOpts.tuning.Add_WhereField(SQLHint.field_type.tstring, LicDB.db_struct.tbl_products_access.ProductCode.ToString(), pProdInfo)
        End If

        If Not String.IsNullOrEmpty(pLicInfo) AndAlso pLicInfo.Trim() <> "0" Then
            m_chartOpts.tuning.Add_WhereField(SQLHint.field_type.tstring, LicDB.db_struct.tbl_products_access.License.ToString(), pLicInfo)
        End If

        If Not String.IsNullOrEmpty(pIPInfo) AndAlso pIPInfo.Trim() <> "0" Then
            m_chartOpts.tuning.Add_WhereField(SQLHint.field_type.tstring, LicDB.db_struct.tbl_products_access.Ip.ToString(), pIPInfo)
        End If


        Dim strError As String = ""


        If Me.m_ClientAccess.Data Is Nothing Then
            Dim chartManager As New L2005.InCharts.ChartsManagement(Me.m_client_page.MapPath("app_config/charts.xml"))

            If chartManager.LoadOK Then
                Me.m_chartOpts.tuning.OrderBy = String.Format("{0} ASC, {1} ASC ,{2} DESC", LicDB.db_struct.tbl_products_access.ProductCode.ToString, _
                                                                                   LicDB.db_struct.tbl_products_access.License.ToString, LicDB.db_struct.tbl_products_access.LastAccessDate)
                Me.m_ClientAccess.bRunningOnDev = False 'WebCommons.pageCommons.IsDevelopmentEnv(Me)
                Me.m_ClientAccess.SetChart("licenses_access_stat", chartManager.xmlCharts, m_chartOpts, strError)
            End If
        End If



        m_ClientAccess.DrawChart(tbl, strError, m_chartOpts)

        'If Not m_ClientLics.DrawChart(tbl, strError, m_chartOpts) Then
        '    WebCommons.pageCommons.AddErrorRow(tbl, strError)
        'End If

    End Function

    'Public Function DumpLicenses(ByVal tbl As Table, ByVal pIdLic As Integer, Optional ByVal pRequeryBefore As Boolean = False) As Boolean

    '    m_chartOpts.m_bQueryBeforeDraw = pRequeryBefore

    '    m_chartOpts.pRowsByPage = 20

    '    'm_chartOpts.fixed_tuning.WhereText = String.Format("dsk_lc.{0}={1}", LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString, CInt(m_product_id))

    '    m_chartOpts.tuning.WhereText = ""

    '    If pIdLic > 0 Then
    '        m_chartOpts.tuning.Add_WhereField(SQLHint.field_type.int, "dsk_lc." + LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString, pIdLic)
    '    End If

    '    Dim strError As String = ""

    '    m_ClientLics.DrawChart(tbl, strError, m_chartOpts)

    '    'If Not m_ClientLics.DrawChart(tbl, strError, m_chartOpts) Then
    '    '    WebCommons.pageCommons.AddErrorRow(tbl, strError)
    '    'End If

    'End Function
End Class