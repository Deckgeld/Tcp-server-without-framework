using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = null;

try 
{
    var port = 13000;
    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
    //Inicialización del servidor TCP
    server = new TcpListener(localAddr, port);

    server.Start();

    //Ciclo infinito para esperar solicitudes
    while (true)
    {
        Console.WriteLine("Esperando por solicitudes...");
        
        //Acepta la solicitud de conexión de un cliente
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Conectado!");

        //Lee el mensaje del cliente desde el stream
        NetworkStream stream = client.GetStream();
        // Se crea un buffer para almacenar los datos recibidos del cliente
        byte[] buffer = new byte[1024];
        // Lee los datos del flujo de red en el buffer y devuelve el número de bytes leídos
        int bytes = stream.Read(buffer, 0, buffer.Length);

        //Se convierte el mensaje a string
        String httpRequest = Encoding.UTF8.GetString(buffer, 0, bytes);
        // Imprime el mensaje recibido del cliente en la consola
        Console.WriteLine("Mensaje recibido: " + httpRequest);

        // Devuelve una repuesta al cliente
            //1) indica que atraves de http se enviara un codigo 200 y un salto de linea
            //2) header con el tipo de contenido y charset
            //3) con DOBLE SALTO DE LINEA, el body (contenido html que se mostrara en el navegador)
        string httpResponse = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n<html><body><h1>¡Hola, soy un servidor!</h1></body></html>";
        byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse);
        stream.Write(responseBytes, 0, responseBytes.Length);

        client.Close();
    }
}
catch(Exception ex)
{
    //Imprime el error en la consola
    Console.WriteLine("Error {0}", ex);
}
finally
{
    // Finalmente, detiene el servidor si no es null
    server?.Stop();
}