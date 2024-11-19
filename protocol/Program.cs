using System.Text.Json;
using System.Text;
using Shared;

namespace protocol
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var response = await APICalls.Get<RequestDTO>("/api/challenges/protocol?isTest=true");
            var decodedMessage = BinaryToString(response.protocolMessage);
            var messageInfo = MessageInfo.Build(decodedMessage);

            var responseMessage = messageInfo.Serialize();
            var decodedResponseMessage = StringToBinary(responseMessage);
            await APICalls.Post("/api/challenges/protocol", new ResponseDTO { answer = decodedResponseMessage});
        }

        static string BinaryToString(string binary)
        {
            if (binary.Length % 8 != 0)
            {
                throw new ArgumentException("binary string must be multiple of 8");
            }

            var text = new StringBuilder();

            for (int i = 0; i < binary.Length; i += 8)
            {
                var byteString = binary.Substring(i, 8);
                var charCode = Convert.ToInt32(byteString, 2);
                text.Append((char)charCode);
            }

            return text.ToString();
        }

        static string StringToBinary(string text)
        {
            var binary = new StringBuilder();

            foreach (char c in text)
            {
                var binaryChar = Convert.ToString(c, 2).PadLeft(8, '0');
                binary.Append(binaryChar);
            }

            return binary.ToString();
        }
    }
}
