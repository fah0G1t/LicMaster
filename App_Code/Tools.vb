Imports Microsoft.VisualBasic
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mime



Namespace Mailing


    Public Class mail
        Private _senderName As String = ""
        Private _sender As String = ""
        Private _subject As String = ""
        Private _body_html As String = ""
        Private _body_txt As String = ""
        Private _path As String = ""
        Private _recipientTo As New ArrayList
        Private _bccTo As New ArrayList
        Private _attachment As Net.Mail.Attachment

        Private _authUser As String = "faho"
        Private _authPass As String = "jakeandfinn"
        Private _authHost As String = "mail.ingeniosoftware.it"


        Public Sub New(ByVal senderName As String, ByVal sender_e_mail As String)
            _senderName = senderName '"InTempo"
            _sender = sender_e_mail '"licenze@ingeniosoftware.it"
        End Sub



        Property pathImage() As String
            Get
                Return _path
            End Get
            Set(ByVal value As String)
                _path = value
            End Set
        End Property

        Property body_html() As String
            Get
                Return _body_html
            End Get
            Set(ByVal value As String)
                '_body = "<img src='cid:IMGTOP'>"
                _body_html = value
                '_body &= "<img src='cid:IMGBOTTOM'>"
            End Set
        End Property

        Property body_txt() As String
            Get
                Return _body_txt
            End Get
            Set(ByVal value As String)
                '_body = "<img src='cid:IMGTOP'>"
                _body_txt = value
                '_body &= "<img src='cid:IMGBOTTOM'>"
            End Set
        End Property

        Property subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal value As String)
                _subject = value
            End Set
        End Property

        Property attachment() As Net.Mail.Attachment
            Get
                Return _attachment
            End Get
            Set(ByVal value As Net.Mail.Attachment)
                _attachment = value
            End Set
        End Property

        ''' <summary>
        ''' lista degli indirizzi in bcc
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property bccTo() As ArrayList
            Get
                Return _bccTo
            End Get
            Set(ByVal value As ArrayList)
                _bccTo = value
            End Set
        End Property

        ''' <summary>
        ''' lista degli indirizzi destinatari della mail
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property recipientTo() As ArrayList
            Get
                Return _recipientTo
            End Get
            Set(ByVal value As ArrayList)
                _recipientTo = value
            End Set
        End Property

        ''' <summary>
        ''' Invia la mail. Prima di chiamare questa funzione bisogna impostare body e subject e recipientTo
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub send()
            Dim mail As New MailMessage
            Dim i As Integer

            mail.From = New MailAddress(_sender, _senderName)
            For i = 0 To _recipientTo.Count - 1
                mail.To.Add(_recipientTo(i))
            Next
            For i = 0 To _bccTo.Count - 1
                mail.Bcc.Add(_bccTo(i))
            Next
            mail.Bcc.Add("tamara@conmet.it")
            mail.Bcc.Add("faho@conmet.it")
            'mail.Bcc.Add("emiliano@sismelfirenze.it")
            'mail.Bcc.Add("lucia.pinelli@sismelfirenze.it")
            'mail.Bcc.Add("anna.vallaro@sismelfirenze.it")
            If _attachment IsNot Nothing Then
                mail.Attachments.Add(_attachment)
            End If
            mail.Subject = Me._subject

            If _body_html <> "" Then
                Dim htmlVIew As AlternateView = AlternateView.CreateAlternateViewFromString(Me._body_html, System.Text.Encoding.Default, MediaTypeNames.Text.Html) '"text/html"
                mail.AlternateViews.Add(htmlVIew)
            End If
            If _body_txt <> "" Then
                Dim textVIew As AlternateView = AlternateView.CreateAlternateViewFromString(Me._body_txt, System.Text.Encoding.Default, MediaTypeNames.Text.Plain)
                mail.AlternateViews.Add(textVIew)
            End If

            'Dim topImage As New LinkedResource(pathImage & "\mail_header.jpg")

            'topImage.ContentType.Name = "topImage.jpg"
            'topImage.ContentType.MediaType = "image/jpeg"
            'topImage.ContentId = "IMGTOP"

            'Dim bottomImage As New LinkedResource(pathImage & "\mail_bottom.jpg")

            'bottomImage.ContentType.Name = "bottomImage.jpg"
            'bottomImage.ContentType.MediaType = "image/jpeg"
            'bottomImage.ContentId = "IMGBOTTOM"

            'htmlVIew.LinkedResources.Add(topImage)
            'htmlVIew.LinkedResources.Add(bottomImage)

            ' A questo punto puoi spedire il messaggio utilizzando l'autenticazione SMTP

            Dim smtpServer As New SmtpClient(_authHost)
            Dim smtpUserInfo As NetworkCredential = New NetworkCredential(_authUser, _authPass)
            smtpServer.UseDefaultCredentials = False
            smtpServer.Credentials = smtpUserInfo

            smtpServer.Send(mail)
        End Sub

    End Class


End Namespace

