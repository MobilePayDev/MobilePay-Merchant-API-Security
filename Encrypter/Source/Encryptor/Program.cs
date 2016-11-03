using System;

namespace Encryptor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Arg 0: Merchant Subscription key
            // Arg 1: Merchant public key crt location
            // Arg 2: MobilePay public key crt location
            // Arg 3: Encrypted output file (Optional)

            if (args.Length < 3)
            {
                Console.WriteLine("Wrong parameter count");
                Console.WriteLine("Argument 1: Merchant Subscription Key");
                Console.WriteLine("Argument 2: Merchant public key crt file location");
                Console.WriteLine("Argument 3: MobilePay public key crt file location");
                Console.WriteLine("Argument 4: Encrypted output file location (optional)");
                return;
            }

            if (args.Length == 4)
                Encrypt.ToFile(args[0], args[1], args[2], args[3]);
            else
                Encrypt.ToFile(args[0], args[1], args[2]);
        }
    }
}
