﻿using System.Globalization;

namespace protocol
{
    class MessageInfo
    {
        public string SignalType { get; set; }
        public string MessageType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"SignalType: {SignalType}\nMessageType: {MessageType}\nLatitude: {Latitude}\nLongitude: {Longitude}\nMessage: {Message}";
        }

        public static MessageInfo Build(string message)
        {
            try
            {
                var messageInfo = new MessageInfo();
                string[] parts = message.Split('#');

                if (parts.Length != 6)
                {
                    throw new FormatException("Invalid message format: incorrect number of segments");
                }

                if (!parts[parts.Length - 1].Equals("END"))
                {
                    throw new FormatException("Invalid message format: does not end in END");
                }

                messageInfo.SignalType = parts[0];
                messageInfo.MessageType = parts[1];
                messageInfo.Latitude = double.Parse(parts[2], CultureInfo.InvariantCulture);
                messageInfo.Longitude = double.Parse(parts[3], CultureInfo.InvariantCulture);

                messageInfo.Message = parts[4];

                return messageInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }

            return null;
        }

        public string Serialize()
        {
            var signalMeaning = GetSignalMeaning();
            string responseMessage = $"ACK#MSG#1{DoubleToString(Latitude)}#1{DoubleToString(Longitude)}#{signalMeaning}#END";
            return responseMessage;
        }

        string DoubleToString(double value)
        {
            var stringVariant = value.ToString();
            return stringVariant.Replace(',', '.');
        }

        string GetSignalMeaning()
        {
            switch (SignalType)
            {
                case "EMG":
                    switch (MessageType)
                    {
                        case "SOS":
                            return "Dispatching rescue crews";
                        case "PAN":
                            return "Dispatching rapid-response team";
                        case "FIR":
                            return "Dispatching firefighting vessel";
                        case "INT":
                            return "Tracking unidentified craft";
                        case "MED":
                            return "Dispatching medical team";
                        default:
                            return "/";
                    }

                case "MNT":
                    switch (MessageType)
                    {
                        case "CHK":
                            return "Dispatching inspection crew";
                        case "REP":
                            return "Dispatching repair crew";
                        case "REF":
                            return "Dispatching tanker vessel";
                        case "CLN":
                            return "Dispatching specialized cleaning crew";
                        default:
                            return "Maintenance: Unknown type";
                    }
                case "INF":
                    return "Asteroid trajectory noted";
                case "REQ":
                    return "Dispatching emergency-supply vessel";
                default:
                    return "Unknown message";
            }
        }
    }
}