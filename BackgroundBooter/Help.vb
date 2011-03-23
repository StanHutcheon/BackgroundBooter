Public Class Help

    Private Sub Help_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'WebBrowser1.Url = Tetheredbootdir & "\help\main.html"
        WebBrowser1.Navigate(New Uri(Tetheredbootdir & "\help\main.html"))
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        WebBrowser1.Navigate(New Uri(Tetheredbootdir & "\help\boot.html"))
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        WebBrowser1.Navigate(New Uri(Tetheredbootdir & "\help\bgservice.html"))
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
        WebBrowser1.Navigate(New Uri(Tetheredbootdir & "\help\bgservicehelp.html"))
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        WebBrowser1.Navigate(New Uri("http://twitter.com/share?_=1300207059381&count=none&text=%40StanTheRipper%20BackgroundBooter%20Question%3A%20%26lt%3Bquestion%26gt%3B&url=http%3A%2F%2Fstantheripper.com"))
    End Sub
End Class