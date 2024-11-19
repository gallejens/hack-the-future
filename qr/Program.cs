using Shared;

namespace qr
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var message = await APICalls.Get<RequestDTO>("/api/challenges/qr-code?isTest=false");

            var codes = SplitKeyCode(message.keyCode);
            var byteStrings = GetByteStringsFromCodes(codes);

            await APICalls.Post("/api/challenges/qr-code", new ResponseDTO {  answer = byteStrings });
        }

        static List<string> SplitKeyCode(string keyCode)
        {
            var codes = new List<string>();
            for (int i = 0; i < keyCode.Length - 1; i += 2)
            {
                codes.Add(keyCode.Substring(i, 2));
            }
            return codes;
        }

        static List<string> GetByteStringsFromCodes(List<string> codes)
        {
            var strings = new List<string>();
            foreach (var code in codes)
            {
                var character = code[0];
                var number = Int32.Parse(code[1].ToString());

                var firstBinary = Convert.ToString(character, 2).PadLeft(7, '0');
                var secondBinary = Convert.ToString(number, 2).PadLeft(4, '0');

                var row = firstBinary + "11" + secondBinary;
                strings.Add(row);
            }
            return strings;
        }
    }
}
