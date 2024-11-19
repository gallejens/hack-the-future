using System.Text.Json;
using System.Text;
using Shared;

namespace protocol
{
    internal class Program
    {
        static string authToken = "Team 17BC43C7-5CE5-4FBF-9AE5-1D3C1C66AA87";

        static async Task Main(string[] args)
        {
            var encodedMessage = await APICalls.Get<RequestDTO>("/api/challenges/protocol?isTest=true");
            var decodedMessage = BinaryToString(encodedMessage.protocolMessage);
            var messageInfo = MessageInfo.Build(decodedMessage);

            var responseMessage = messageInfo.Serialize();
            var decodedResponseMessage = StringToBinary(responseMessage);
            await APICalls.Post("/api/challenges/protocol", new ResponseDTO { answer = decodedResponseMessage});
        }

        static string BinaryToString(string binary)
        {
            // Check if the binary string length is a multiple of 8
            if (binary.Length % 8 != 0)
            {
                throw new ArgumentException("Binary string length must be a multiple of 8.");
            }

            StringBuilder text = new StringBuilder();

            for (int i = 0; i < binary.Length; i += 8)
            {
                string byteString = binary.Substring(i, 8);
                int charCode = Convert.ToInt32(byteString, 2);
                text.Append((char)charCode);
            }

            return text.ToString();
        }

        static string StringToBinary(string text)
        {
            StringBuilder binary = new StringBuilder();

            foreach (char c in text)
            {
                // Convert each character to its ASCII value and then to 8-bit binary
                string binaryChar = Convert.ToString(c, 2).PadLeft(8, '0');
                binary.Append(binaryChar);
            }

            return binary.ToString();
        }
    }
}
