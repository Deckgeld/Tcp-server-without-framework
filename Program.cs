using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = null;

try 
{
    var port = 13000;
    IPAddress localAddr = IPAddress.Parse("127.0.0.1");

    server = new TcpListener(localAddr, port);

    server.Start();

    string data = null;

    while (true)
    {
        Console.WriteLine("Esperando por solicitudes...");
        
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Conectado!");

        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytes = stream.Read(buffer, 0, buffer.Length);

        String httpRequest = Encoding.UTF8.GetString(buffer, 0, bytes);
        Console.WriteLine("Mensaje recibido: " + httpRequest);

        //Lo primero indica que atraves de http se enviara un codigo 200 y un salto de linea
        //Lo segundo son headers, indica el tipo de contenido regresado
        //El charset
        //Y por ultimo muy importante, con DOBLE SALTO DE LINEA, el body con contenido html
        string httpResponse = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n<html><body><h1>¡Hola, soy un servidor!</h1></body></html>";
        byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse);
        stream.Write(responseBytes, 0, responseBytes.Length);

        client.Close();
    }
}
catch(Exception ex)
{
    Console.WriteLine("Error {0}", ex);
}
finally
{

}