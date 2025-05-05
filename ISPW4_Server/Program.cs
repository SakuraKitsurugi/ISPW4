// Application 1 – Signing & Sending (5 points):
//
// Message Signing (2 points):
// Input a message and generate its RSA signature.
// Communication (2 points):
// Send the message, signature, and public key to Application 2 via sockets.
// Explanation (1 point):
// Explain how the signature is created.

using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace ISPW4;

class Program {
    static void Main(string[] args) {
        Console.Write("Enter a plaintext message to sign and send: ");
        string plaintext = Console.ReadLine();

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) {
            // Gets a public key
            string publicKey = rsa.ToXmlString(false);

            // Computes a hash from plaintext
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] plaintextHash;
            using (SHA256 sha256 = SHA256.Create()) {
                plaintextHash = sha256.ComputeHash(plaintextBytes);
            }

            // Signs the hash and converts it into base64
            byte[] signature = rsa.SignHash(plaintextHash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            string signatureBase64 = Convert.ToBase64String(signature);

            // Sends the data over socket to, depending on port, middleman or client
            Console.WriteLine("\nAttemping to send information...");
            SendDataOverPort(plaintext, signatureBase64, publicKey);
        }
    }

    static void SendDataOverPort(string plaintext, string signature, string publicKey) {
        try {
            // Connects to a local ip with a specific open port and sends the message, signature and public key
            using (TcpClient client = new TcpClient("127.0.0.1", 1234))
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
