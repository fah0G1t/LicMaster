Imports webcommons


Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub cmdLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLogin.Click
        If Me.txt_log.Text = "faho" AndAlso Me.txt_password.Text = "gobills" Then
            Me.Response.Redirect("ManagControlloPresenze.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.lblVer.Text = WebCommons.pageCommons.GetConfigValue("verAndDate")

        If WebCommons.pageCommons.IsDevelopmentEnv(Me) AndAlso Me.IsPostBack Then
            Me.txt_log.Text = "faho"
            Me.txt_password.Text = "gobills"
        End If

    End Sub
End Class
