using System;
using System.Collections.Generic;
using System.Text.Json;

namespace mod7
{
    class Program
    {
        public static void Main(string[] args)
        {
            int amount = 0;
            int fee;
            string message, info_fee, info_amount, method, confirm, confirm_message_fail, confirm_message_success, met_num, check;

            Config config = new Config();

            if (config.conf.lang == "en")
            {
                message = "Please insert the amount of money to transfer: ";
                info_fee = "Transfer fee = ";
                info_amount = "Total amount = ";
                method = "Select transfer method: ";
                confirm = "Please type '" + config.conf.confirmation.en + "' to confirm the transaction: ";
                confirm_message_fail = "Transfer is cancelled";
                confirm_message_success = "The transfer is completed";
                check = config.conf.confirmation.en;
            }
            else
            {
                message = "Masukkan jumlah uang yang akan ditransfer: ";
                info_fee = "Biaya transfer = ";
                info_amount = "Total biaya = ";
                method = "Pilih metode transfer: ";
                confirm = "Ketik '" + config.conf.confirmation.id + "' untuk mengkonfirmasi transaksi: ";
                confirm_message_fail = "Transfer dibatalkan";
                confirm_message_success = "Proses transfer berhasil";
                check = config.conf.confirmation.id;
            }

            Console.WriteLine(message);
            amount = Int32.Parse(Console.ReadLine());

            if (amount <= config.conf.transfer.threshold)
            {
                fee = config.conf.transfer.low_fee;
            }
            else
            {
                fee = config.conf.transfer.high_fee;
            }

            Console.WriteLine(info_fee + fee);
            Console.WriteLine(info_amount + (amount + fee));
            Console.WriteLine(method);
            for (int i = 0; i < config.conf.methods.Count; i++)
            {
                Console.WriteLine(i + 1 + ". " + config.conf.methods[i]);
            }
            met_num = Console.ReadLine();
            Console.WriteLine(confirm);
            met_num = Console.ReadLine();
            if (met_num == check)
            {
                Console.WriteLine(confirm_message_success);
            }
            else
            {
                Console.WriteLine(confirm_message_fail);
            }
        }
    }

    class Config
    {
        public BankTransferConfig conf;
        public string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        public string file = "bank_transfer_config.json";
        public Config ()
        {
            try
            {
                ReadConfigFile();
            }
            catch (Exception)
            {
                SetDefault();
                WriteConfigFile();
            }
        }
        private BankTransferConfig ReadConfigFile()
        {
            string jsonStringFromFile = File.ReadAllText(path + "/" + file);
            conf = JsonSerializer.Deserialize<BankTransferConfig>(jsonStringFromFile);
            return conf;
        }

        private void SetDefault ()
        {
            TransferConfig objTransfer = new TransferConfig(
                25000000,
                6500,
                15000);

            ConfirmationConfig objConfirm = new ConfirmationConfig(
                "yes",
                "ya");

            List<string> methods = new List<string>() { "RTO", "SKN", "RTG", "BI FAST" };

            conf = new BankTransferConfig("en", objTransfer, methods, objConfirm);

        }

        private void WriteConfigFile ()
        {
            JsonSerializerOptions option = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize (conf, option);
            string fullPath = path + "/" + file;
            File.WriteAllText(fullPath, jsonString);
        }

    }
    public class BankTransferConfig
    {
        public string lang { get; set; }
        public TransferConfig transfer { get; set; }
        public List<string> methods { get; set; }
        public ConfirmationConfig confirmation { get; set; }

        public BankTransferConfig () { }

        public BankTransferConfig (string lang, TransferConfig transfer, List<string> methods, ConfirmationConfig confirmation)
        {
            this.lang = lang;
            this.transfer = transfer;
            this.methods = methods;
            this.confirmation = confirmation;
        }

    }

    public class TransferConfig
    {
        public int threshold { get; set; }
        public int low_fee { get; set; }
        public int high_fee { get; set; }

        public TransferConfig(int threshold, int low_fee, int high_fee)
        {
            this.threshold = threshold;
            this.low_fee = low_fee;
            this.high_fee = high_fee;
        }
    }

    public class ConfirmationConfig
    {
        public string en { get; set; }
        public string id { get; set; }

        public ConfirmationConfig(string en, string id)
        {
            this.en = en;
            this.id = id;
        }
    }
}