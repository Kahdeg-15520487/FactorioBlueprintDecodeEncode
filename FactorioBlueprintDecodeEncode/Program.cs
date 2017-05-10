using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ionic.Zlib;
using CommandLine;
using System.IO;

namespace FactorioBlueprintDecodeEncode
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<EncodeOption, DecodeOption, Option>(args)
                .WithParsed<Option>(parsedoption => { handleNormalOption(parsedoption); })
                .WithParsed<EncodeOption>(parsedoption => { handleEncodeOption(parsedoption); })
                .WithParsed<DecodeOption>(parsedoption => { handleDecodeOption(parsedoption); });

            end:
            Console.WriteLine("press enter to exit...");
            Console.ReadLine();
        }

        static void handleNormalOption(Option option)
        {
            string data = option.data;
            char firstchar = data[0];
            data = data.Remove(0, 1);
            if (firstchar == '{')
            {
                //json string confirmed
                Console.WriteLine();
                Console.WriteLine(encode(data));
            }
            else
            {
                //blueprint string confirmed
                Console.WriteLine();
                Console.WriteLine(decode(data));
            }
        }

        static void handleEncodeOption(EncodeOption option)
        {
            Console.WriteLine("encode");
            Console.WriteLine(option.jsonstring?.ToString());
            Console.WriteLine(option.filename?.ToString());
        }

        static void handleDecodeOption(DecodeOption option)
        {
            Console.WriteLine("decode");
            Console.WriteLine(option.blueprintstring?.ToString());
            Console.WriteLine(option.filename?.ToString());

            Console.WriteLine();
            Console.WriteLine(decode(option.blueprintstring.Remove(0, 1)));
        }

        static string encode(string data)
        {
            string result = string.Empty;
            string base64encoded;
            string zlibCompressed;

            return result;
        }

        static string decode(string data)
        {
            string result = string.Empty;
            string base64decoded;
            string zlibDecompressed;

            base64decoded = Base64Decode(data);

            byte[] resultbyte;

            MemoryStream outputstream = new MemoryStream();
            using (Stream target = GenerateStreamFromString(data))
            {
                using (ZlibStream zlibstream = new ZlibStream(target, CompressionMode.Decompress))
                {
                    zlibstream.CopyTo(outputstream);
                }
            }
            resultbyte = outputstream.ToArray();

            result = Encoding.UTF8.GetString(resultbyte);

            return result;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }

    class Option
    {
        [Value(0)]
        public string data { get; set; }
    }

    [Verb("encode", HelpText = "Encode Json to Factorio's blueprint string")]
    class EncodeOption
    {
        [Option('s')]
        public string jsonstring { get; set; }
        [Option('f')]
        public string filename { get; set; }
    }

    [Verb("decode", HelpText = "Decode Factorio's blueprint string to Json")]
    class DecodeOption
    {
        [Option('s')]
        public string blueprintstring { get; set; }
        [Option('f')]
        public string filename { get; set; }
    }
}
