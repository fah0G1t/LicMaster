Imports Microsoft.VisualBasic
Imports System.IO
Imports CryptedConfiguration
Imports DataUnits1._1


Namespace IngenioLicenseManagement

    Public Class CryptingKeys
        Public Const C_SALT As String = "saltySug4r"
        Public Const C_PHRASE As String = "murcielago%pipp0ne45"
    End Class



    Public Class DesktopLicense
        Public Enum lic_struct
            appname
            lversion
            ltype
            dcount
            expires
            customer
            vsn
            computer
            os
        End Enum

        Public Enum lic_type
            lc_none = 0
            lc_normal
            lc_temp
        End Enum

        Private m_id_licenza As Integer
        Public m_appname As String = ""
        Public m_appTitle As String = ""
        Public m_dataReq As Date
        Public m_dataSend As Date
        Public m_ltype As lic_type
        Public m_dcount As Integer = 0
        Public m_expires As Date

        Public m_id_product As Integer = 0

        Public m_id_customer As Integer = 0
        Public m_customer As String = ""
        Public m_CustomerEmail As String = ""
        Public m_CustomerPhone As String = ""
        Public m_vsn As String = ""
        Public m_computer As String = ""

        Public m_LicVersion As String = "0.0"
        Public m_os_string As String = "XP (default)"


        Public m_app_dir As String = ""

        Private Function CandlesticParkDate() As Date
            Dim dretval As Date = New Date(1966, 8, 29)

            Return dretval
        End Function

        Public Function FileName() As String
            Return String.Format("cp_{0}.lf", Me.m_computer)
        End Function


        Private Function GetParamValue(ByVal pItem As lic_struct) As Object
            Select Case pItem
                Case lic_struct.appname
                    Return Me.m_appname
                Case lic_struct.computer
                    Return Me.m_computer
                Case lic_struct.customer
                    Return Me.m_customer
                Case lic_struct.dcount
                    'If Me.m_ltype = lic_type.lc_temp And Me.m_dcount >= 30 Then
                    Return Me.m_dcount
                    'ElseIf Me.m_ltype = lic_type.lc_normal Then
                    '    Return 0
                    'Else
                    '    Return Nothing
                    'End If
                Case lic_struct.ltype
                    Return CInt(Me.m_ltype)
                Case lic_struct.lversion
                    Return Me.m_LicVersion
                Case lic_struct.vsn
                    Return Me.m_vsn
                Case lic_struct.os
                    Return Me.m_os_string
                Case lic_struct.expires
                    If Me.m_ltype = lic_type.lc_temp Then
                        Return CandlesticParkDate().ToShortDateString
                    Else
                        Return New Date(2030, 12, 31).ToShortDateString
                    End If

                Case Else
                    Return Nothing
            End Select
        End Function


        Public Function GetCleanStringLic(ByRef pError As String) As String
            Dim strFormat As String = "" '"<?xml version=""1.0""?><config></config>"

            pError = ""

            Try

                For iItem As lic_struct = lic_struct.appname To lic_struct.computer
                    strFormat += String.Format("<{0}>{1}</{0}>", iItem.ToString, GetParamValue(iItem))
                Next iItem

            Catch ex As Exception
                strFormat = ""
                pError = "GetCleanStringLic::" + ex.Message
            End Try

            If Not String.IsNullOrEmpty(strFormat) Then
                strFormat = "<?xml version=""1.0""?><config>" + strFormat + "</config>"
            End If

            Return strFormat
        End Function

        Public Function GetLicString(ByRef pError As String) As String

            Dim strCleanStr As String = Me.GetCleanStringLic(pError)
            Dim strCryptedLic As String = ""

            If Not String.IsNullOrEmpty(strCleanStr) Then
                Dim licFile As New ConfigHandler

                licFile.SaltValue = CryptingKeys.C_SALT
                licFile.PassPhrase = CryptingKeys.C_PHRASE

                Try
                    strCryptedLic = licFile.Encrypt(strCleanStr)
                Catch ex As Exception
                    strCryptedLic = ""
                    pError = "GetLicString::" + ex.Message
                End Try
            End If

            Return strCryptedLic
        End Function


        Public Function Load(ByVal pIdLic As Integer, ByRef pNote As String, ByRef pError As String) As Boolean
            Dim duLics As New DataUnit(LicDB.db_struct.lc_tables.desktop_licenses, LicDB.db_struct.DBConn)
            Dim oFilter As New SQLHint(String.Format("{0}={1}", LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString, pIdLic))

            Dim bretval As Boolean = False
            Dim strQry As String = "SELECT dsk_lc.*, clt.id_client,clt.nome_cliente,clt.telefono,clt.email, prd.nome, prd.titolo, prd.id_prodotto FROM desktop_licenses dsk_lc" + _
                  " INNER JOIN clienti clt ON dsk_lc.id_cliente = clt.id_client " + _
                  " INNER JOIN Prodotti prd ON dsk_lc.id_prodotto = prd.id_prodotto " + _
                  String.Format("WHERE dsk_lc.{0}={1}", LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString, pIdLic)

            If duLics.ExecuteSelect(strQry, pError) Then

                Me.m_id_licenza = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString)
                Me.m_appname = duLics.OutData(LicDB.db_struct.tbl_prodotto.nome.ToString)   'duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString)
                Me.m_appTitle = duLics.OutData(LicDB.db_struct.tbl_prodotto.titolo.ToString)

                Me.m_id_customer = duLics.OutData(LicDB.db_struct.tbl_clienti.id_client.ToString)
                Me.m_customer = duLics.OutData(LicDB.db_struct.tbl_clienti.nome_cliente.ToString)
                Me.m_CustomerEmail = duLics.OutData(LicDB.db_struct.tbl_clienti.email.ToString)
                Me.m_CustomerPhone = duLics.OutData(LicDB.db_struct.tbl_clienti.telefono.ToString)

                Me.m_id_product = duLics.OutData(LicDB.db_struct.tbl_prodotto.id_prodotto.ToString)

                Me.m_os_string = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.os.ToString())
                Me.m_computer = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.c_name.ToString)
                Me.m_vsn = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.vsn.ToString)
                Me.m_app_dir = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.clnt_app_dir.ToString)
                Me.m_dataReq = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.data_req.ToString)
                Me.m_dataSend = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.data_send.ToString)
                Me.m_dcount = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.dcount.ToString)
                Me.m_ltype = IIf(Me.m_dcount = 0, lic_type.lc_normal, lic_type.lc_temp) 'ctype(duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.clnt_app_dir.ToString))

                pNote = duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.nota.ToString())

                bretval = True
            End If

            Return bretval
        End Function

        Public Sub Clear()
            Me.m_id_licenza = 0
            Me.m_appname = ""  'duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString)
            Me.m_appTitle = ""

            Me.m_id_customer = 0
            Me.m_customer = ""
            Me.m_CustomerEmail = ""
            Me.m_CustomerPhone = ""

            Me.m_id_product = 0

            Me.m_computer = ""
            Me.m_vsn = ""
            Me.m_app_dir = ""
            Me.m_dataReq = Nothing
            Me.m_dataSend = Nothing
            Me.m_dcount = 30
            Me.m_ltype = lic_type.lc_none

        End Sub



        Public Function Delete(ByRef pError As String) As Boolean

            pError = ""

            Dim duLics As New DataUnit(LicDB.db_struct.lc_tables.desktop_licenses, LicDB.db_struct.DBConn)
            Dim oFilter As New SQLHint(String.Format("{0}={1}", LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString, Me.m_id_licenza))

            Dim bretval As Boolean = True

            If duLics.ExecuteDelete(pError, oFilter) Then

                Me.m_id_licenza = 0
                Me.m_appname = ""   'duLics.OutData(LicDB.db_struct.tbl_dsktop_lic.id_prodotto.ToString)
                Me.m_appTitle = ""

                Me.m_customer = ""
                Me.m_CustomerEmail = ""

                Me.m_computer = ""
                Me.m_vsn = ""
                Me.m_app_dir = ""
                Me.m_dcount = 30

            Else

                bretval = False
            End If

            Return bretval
        End Function

        Public ReadOnly Property LICENZA_ID() As Integer
            Get
                Return Me.m_id_licenza
            End Get
        End Property


        Public Function UpdateSendTime(ByVal pIdLic As Integer, ByRef pError As String) As Boolean
            Dim duLics As New DataUnit(LicDB.db_struct.lc_tables.desktop_licenses, LicDB.db_struct.DBConn)
            Dim oFilter As New SQLHint(String.Format("{0}={1}", LicDB.db_struct.tbl_dsktop_lic.id_licenza.ToString, pIdLic))

            Dim bretval As Boolean = False

            duLics.SetInData(LicDB.db_struct.tbl_dsktop_lic.data_send.ToString, Now)

            If duLics.ExecuteUpdate(pError, oFilter) Then
                bretval = True
            End If

            Return bretval
        End Function


        'If Me.m_pageInfo.m_Lic.Load(iTest) Then
        '    If Me.m_pageInfo.m_Lic.SendToUser(strError) Then
        Public Function CreateLicFile(ByVal pOutputFolder As String, ByRef pError As String) As Boolean
            Dim bRetval As Boolean = False
            Dim strCrypted As String = Me.GetLicString(pError)


            If Not String.IsNullOrEmpty(strCrypted) Then

                Dim strFieldAttachFolder As String = String.Format("{0}\cp_{1}.lf", pOutputFolder, Me.m_computer)

                Dim fi As IO.FileInfo = New FileInfo(strFieldAttachFolder)

                Dim swr As StreamWriter = Nothing

                Try
                    swr = fi.CreateText()
                Catch ex As Exception
                    bRetval = False
                    pError = "CreateLicFile::(FileInfo CreateText):" + ex.Message
                End Try

                If swr IsNot Nothing Then
                    Try
                        swr.Write(strCrypted)
                        swr.Close()
                        bRetval = True

                        pError = strFieldAttachFolder

                    Catch ex As Exception
                        pError = "CreateLicFile::(StreamWriter write):" + ex.Message
                    End Try
                End If

            Else
                pError = "CreateLicFile::Null license string"
            End If

            Return bRetval
        End Function



        Public Function SendToUser(ByVal pAttachmentFilePath As String, ByRef pError As String) As Boolean
            Dim bRetval As Boolean = True

            Try

                'send
                Dim myself As New ArrayList

                myself.Add(Me.m_CustomerEmail)

                Dim attFile As New Net.Mail.Attachment(pAttachmentFilePath)
                Dim strDate As String = Me.m_dataReq.Date
                Dim strBody As String = String.Format("Spettabile {0},<br/>siamo lieti di inviare la licenza d'uso richiesta in data {1} per il software {2}", Me.m_customer, Me.m_dataReq.Date.ToShortDateString(), Me.m_appname)

                If Me.m_ltype = lic_type.lc_temp Then
                    strBody += "<br/><br/>"

                    strBody += String.Format("Detta licenza come concordato è temporale per uso dimostrativo e sarà valida per {0} giorni dalla sua istallazione", Me.m_dcount)
                End If

                strBody += "<br/><br/>Istruzioni per l'installazione:<br/>"
                strBody += String.Format("<br/>Il file in allegato (licenza d'uso) dovrà essere copiato nella cartella {1} del computer con nome {2} dove è già installato il software {0}<br/>  ", Me.m_appname, Me.m_app_dir, Me.m_computer)

                strBody += "<br/><br/>Per eventuali difficoltà contattare il nostro supporto tecnico a info@ingeniosoftware.it"
                strBody += String.Format("<br/><br/>Ringraziandola per la fiducia accordata, il team di {0} le augura un buon lavoro", Me.m_appname)

                If Not SendMail("Licenza d'uso", strBody, "", myself, Me.m_appTitle, pError, attFile) Then
                    pError = "Licenza non inviata: errore posta elettronica: " + pError
                    bRetval = False
                Else
                    Dim strError As String = ""
                    UpdateSendTime(Me.m_id_licenza, strError)
                End If

                'Try
                '    fi.Delete()
                'Catch ex As Exception
                '    bRetval = False
                '    pError = "La licenza è stata corettamente inviata non ostante il file di attachement non si è potuto rimuovere: " + ex.Message
                'End Try

            Catch ex As Exception
                bRetval = False
                pError = "Evento inatteso: " + ex.Message
            End Try



            Return bRetval
        End Function

        Private Shared Function SendMail(ByVal pSubjectText As String, ByVal pBodyHtml As String, ByVal pBodyText As String, _
                                                ByVal recipientsLst As ArrayList, ByVal pAppTitle As String, ByRef pError As String, _
                                                Optional ByVal pAttachment As Net.Mail.Attachment = Nothing) As Boolean
            '     Optional ByVal pBccType As bcc_types = bcc_types.bcc_none, _

            Dim bResult As Boolean = True

            Dim mymail As New Mailing.mail(pAppTitle, "info@ingeniosoftware.it")


            'If MirabileGlobals.IsDevelopmentMachine Then
            '    pSubjectText += " (ambiente sviluppo) "
            'End If

            mymail.subject = pSubjectText + ": " + pAppTitle
            mymail.body_html = pBodyHtml
            mymail.body_txt = pBodyText
            mymail.recipientTo = recipientsLst

            If pAttachment IsNot Nothing Then
                mymail.attachment = pAttachment
            End If

            'Select Case pBccType
            '    Case bcc_types.bcc_order
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_order"))
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_contabilita"))
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_contabilita2"))
            '    Case bcc_types.bcc_registration
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_luca"))
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_info"))
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_support"))
            '    Case bcc_types.bcc_info
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_info"))
            '    Case bcc_types.bcc_err_not_pdf
            '        mymail.bccTo.Add(WebCommons.pageCommons.GetConfigValue("mail_info"))
            '    Case bcc_types.bcc_dbg_ip
            '        mymail.bccTo.Add("degli_innocenti@hotmail.com")
            'End Select

            Try
                mymail.send()
            Catch ex As Exception
                pError = ex.Message
                bResult = False
            End Try

            Try
                mymail.attachment.Dispose()
            Catch ex As Exception

            End Try


            Return bResult
        End Function


    End Class



    Public Class DesktopLicRequest

        Public m_Customer As String = ""
        Public m_Telefono As String = ""
        Public m_EMail As String = ""
        Public m_VSN As String = ""
        Public m_ComputerName As String = ""
        Public m_requestDate As Date
        Public m_StartupDir As String = ""

        Public m_OsVersionString As String = ""
        Public m_LicVersion As String = ""



        Public Enum request_struct
            lversion
            data
            customer
            vsn
            computer
            telefono
            email
            app_dir
            os
        End Enum


        Public Function GetLicRequestInfo(ByVal pCryptedInfo As String, ByRef pError As String, ByRef pGetAwayBuffer As String) As Boolean
            Dim bRetval As Boolean = True

            'Public Function TestRequestFile(ByVal pFileCostumerPath As String, ByRef pError As String) As Boolean
            'Dim strFormat As String = "<?xml version=""1.0""?><config><data>{0}</data><customer>{1}</customer><vsn>{2}</vsn><computer>{3}</computer></config>"
            pError = ""
            pGetAwayBuffer = ""

            Dim licFile As New ConfigHandler

            licFile.SaltValue = CryptingKeys.C_SALT
            licFile.PassPhrase = CryptingKeys.C_PHRASE

            Dim strClearText As String = ""

            Try
                strClearText = licFile.Decrypt(pCryptedInfo)
            Catch ex As Exception

            End Try

            Try
                m_requestDate = licFile.GetDateTime(pCryptedInfo, request_struct.data.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "Data di richiesta: " + ex.Message
            End Try


            Try
                m_ComputerName = licFile.GetString(pCryptedInfo, request_struct.computer.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>Computer: " + ex.Message
            End Try

            Try
                m_Customer = licFile.GetString(pCryptedInfo, request_struct.customer.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>Cliente: " + ex.Message
            End Try


            Try
                m_VSN = licFile.GetString(pCryptedInfo, request_struct.vsn.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>VSN: " + ex.Message
            End Try


            Try
                m_EMail = licFile.GetString(pCryptedInfo, request_struct.email.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>e-mail: " + ex.Message
            End Try

            Try
                m_Telefono = licFile.GetString(pCryptedInfo, request_struct.telefono.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>telefono: " + ex.Message
            End Try

            Try
                m_StartupDir = licFile.GetString(pCryptedInfo, request_struct.app_dir.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>Cartella: " + ex.Message
            End Try

            Try

            Catch ex As Exception

            End Try

            'new to version 1.0
            Try
                m_OsVersionString = licFile.GetString(pCryptedInfo, request_struct.os.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>OS version: " + ex.Message
            End Try

            'new to version 1.0
            Try
                Me.m_LicVersion = licFile.GetString(pCryptedInfo, request_struct.lversion.ToString)
            Catch ex As Exception
                bRetval = False
                pError = "<br/>Lic version: " + ex.Message
            End Try

            pGetAwayBuffer = strClearText

            Return bRetval
        End Function
        'End Function

    End Class
End Namespace



