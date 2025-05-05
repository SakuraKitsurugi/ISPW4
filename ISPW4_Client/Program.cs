// Application 3 – Verification (3 points):
//
// Receiving Data (1 point):
// Receive the public key, message, and signature from Application 2 via sockets.
// Verification (1 point):
// Validate the signature and display whether it is correct.
// Explanation (1 point):
// Explain the verification process and how tampering affects it.

using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace ISPW4_Client;

class Program {
    static void Main(string[] args) {
        // Opens a port and listens to it until information is received
        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5678);
        listener.Start();
        Console.WriteLine("Listener started...");

        // Once connection is detected the code starts
        using (TcpClient client = listener.AcceptTcpClient())
        using (NetworkStream stream = client.GetStream()) {
            // Reads received bytes from the buffer
            byte[] buffer = new byte[4096]; //Maybe possible to make it so it increases in size as data is received?
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Separates received data
            string[] parts = receivedData.Split('|');
            string message = parts[0];
            string signatureBase64 = parts[1];
            string publicKey = parts[2];

            Console.WriteLine($"Received Message: {message}");
            Console.WriteLine($"Received Signature: {signatureBase64}");

            // Signature verification
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                rsa.FromXmlString(publicKey);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                byte[] signatureBytes = Convert.FromBase64String(signatureBase64);

                // Computes the hash
                byte[] messageHash;
                using (SHA256 sha256 = SHA256.Create()) {
                    messageHash = sha256.ComputeHash(messageBytes);
                }

                // Verification
                if (rsa.VerifyHash(messageHash, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)) {
                    Console.WriteLine("Signature is VALID!");
                }
                else {
                    Console.WriteLine("Signature is INVALID!");
                }
            }
        }

        listener.Stop();
    }
}
