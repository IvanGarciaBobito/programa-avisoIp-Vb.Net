Imports System.IO
Imports System.Net.Mail

Module Module1

    Sub Main()
        Console.WriteLine("Creado por Ivan Garcia para XXXXXXXX")
        Threading.Thread.Sleep(1500)

        If Not File.Exists(Environment.CurrentDirectory + "\ip.txt") Then 'Comprueba que exista el archivo ip.txt donde se guarda la ip
            File.Create(Environment.CurrentDirectory + "\ip.txt").Close() 'Si no existe el archivo ip.txt crea uno nuevo
        End If

        Dim ipanterior As String
        ipanterior = My.Computer.FileSystem.ReadAllText(Environment.CurrentDirectory + "\ip.txt").ToString() 'Lee del archivo ip.txt la ip que se escribio la ultima vez
        Dim peticion As Net.HttpWebRequest = Net.WebRequest.Create("http://www.cualesmiip.com") 'se hace una peticion a cualesmiip
        peticion.UserAgent = "Mozilla/5.0" 'si no se pone el useragent no nos da acceso
        Dim respuesta As Net.HttpWebResponse = peticion.GetResponse() 'se guarda la respuesta de la peticion

        'Recorremos la respuesta y parseamos lo que nos interesa y guardamos la parte que nos interesa con la ip
        Dim sr As StreamReader = New StreamReader(respuesta.GetResponseStream())
        Dim codigoFuente As String = sr.ReadToEnd()
        codigoFuente = codigoFuente.Remove(0, InStr(codigoFuente, "Tu IP real es") + 13)
        codigoFuente = codigoFuente.Substring(0, InStr(codigoFuente, " "))


        Console.WriteLine("La ip del servidor es: " & codigoFuente) 'Mostramos la ip
        Threading.Thread.Sleep(1500) 'Paramos la ejecucion para que el usuario vea la informacion

        'Comparamos la ip desde la ultima ejecucion y la nueva,si son iguales informamos de ello y se termina la ejecucion
        If ipanterior = codigoFuente & vbCrLf Then
            Console.WriteLine("la ip sigue siendo la misma que la ultima vez,no se enviara el email a xxxxx")
            Console.WriteLine("IP: " & ipanterior)
            Threading.Thread.Sleep(6000)
            'Si la ip desda la ultima ejecucion no es la misma que la nueva escribimos la ip nueva en el archivo y mandamos un email informando de la nueva
        Else
            File.Delete(Environment.CurrentDirectory + "\ip.txt")
            File.Create(Environment.CurrentDirectory + "\ip.txt").Close()
            Dim sw As New StreamWriter(Environment.CurrentDirectory + "\ip.txt")
            sw.WriteLine(codigoFuente)
            sw.Close()
            Console.WriteLine("la ip a cambiado desde la ultima vez,enviando email al garaje con la nueva ip")

            Threading.Thread.Sleep(2000)
            envioCorreo(codigoFuente)
        End If
    End Sub

    Private Sub envioCorreo(ByVal codigo As String)
        Dim correo As New MailMessage
        Dim smtp As New SmtpClient()

        'Definimos los datos del mandatario
        correo.From = New MailAddress("xxxx@gmail.com", "ivan", Text.Encoding.UTF8)
        'Definimos los datos del receptor
        correo.To.Add("xxxx@xxxx.com")
        'Definimos el contenido del email informando de que la ip ha cambiado
        correo.SubjectEncoding = Text.Encoding.UTF8
        correo.Subject = "Nueva ip del servidor"
        correo.Body = "La nueva ip del servidor es " & codigo

        smtp.Credentials = New Net.NetworkCredential("xxxxx@gmail.com", "Password") 'Añadimos las credenciales de la cuenta del mandatario
        smtp.Port = 587
        smtp.Host = "xxxxx.com"
        smtp.EnableSsl = True
        'Mandamos el correo,manejamos las excepciones e informamos al usuario del resultado
        Try
            smtp.Send(correo)
            Console.WriteLine("Email enviado")
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
        Threading.Thread.Sleep(8000)
    End Sub
End Module
