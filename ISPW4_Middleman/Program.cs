// Application 2 – Tampering Proxy (2 points):
//
// Data Handling (1 point):
// Receive the message, signature, and public key from Application 1.
// Allow the user to modify the digital signature manually.
// Forwarding (1 point):
// Send the (possibly modified) data to Application 3.


using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ISPW4_Middleman;

class Program {
    static void Main(string[] args) {
        // Opens a port and listens to it until information is received
        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);
        listener.Start();
        Console.WriteLine("Listener started...");

        string message;
        string signatureBase64;
        string publicKey;

        using (TcpClient client = listener.AcceptTcpClient())
        using (NetworkStream stream = client.GetStream()) {
            // Reads received bytes from the buffer
            byte[] buffer = new byte[4096]; //Maybe possible to make it so it increases in size as data is received?
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Separates received data
            string[] parts = receivedData.Split('|');
            message = parts[0];
            signatureBase64 = parts[1];
            publicKey = parts[2];
        }

        listener.Stop();
        
        Console.WriteLine($"Received Message: {message}");
        Console.WriteLine($"Received Signature: {signatureBase64}");

        string modifiedSignature;
        while (true) {
            Console.WriteLine("Enter modified signature (Leave empty for no modification):");
            modifiedSignature = Console.ReadLine();
            
            if (modifiedSignature.Length == signatureBase64.Length) {
                SendDataOverPort(message, modifiedSignature, publicKey);
                break;
            }
            else if (string.IsNullOrEmpty(modifiedSignature)) {
                SendDataOverPort(message, signatureBase64, publicKey);
                break;
            }
            else {
                Console.Clear();
                Console.WriteLine("The modified signature has to be of similar length to unmodified one!");
            }
        }
    }

    static void SendDataOverPort(string plaintext, string signature, string publicKey) {
        try {
            // Connects to a local ip with a specific open port and sends the message, signature and public key
            using (TcpClient client = new TcpClient("127.0.0.1", 5678))
            using (NetworkStream stream = client.GetStream()) {
                string data = $"{plaintext}|{signature}|{publicKey}";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("Message sent!");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Error sending data: {ex.Message}");
        }
    }
}
