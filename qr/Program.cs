using Shared;

namespace qr
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var message = await APICalls.Get<RequestDTO>("/api/challenges/qr-code?isTest=false");

            var stringArray = GetBytesFromCode(message.keyCode);

            await APICalls.Post("/api/challenges/qr-code", new ResponseDTO {  answer = stringArray });
        }

        static string[] GetBytesFromCode(string keycode)
        {
            var stringArray = new string[13];
            var codes = new List<string>();
            for (int i = 0; i < keycode.Length - 1; i += 2)
            {
                codes.Add(keycode.Substring(i, 2));
            }

            for (int i = 0; i < codes.Count; i++)
            {
                var code = codes[i];

                var character = code[0];
                var number = Int32.Parse(code[1].ToString());

                var firstBinary = Convert.ToString(character, 2).PadLeft(8, '0');
                var secondBinary = Convert.ToString(number, 2).PadLeft(4, '0');

                var row = firstBinary.Substring(1) + "11" + secondBinary;
                stringArray[i] = row;
            }
            return stringArray;
        }
    }
}
