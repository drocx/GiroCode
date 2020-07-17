using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GiroCode
{
   public static class GiroCodeGenerator
   {
      public class Girocode
      {
         //Keep in mind, that the ECC level has to be set to "M" when generating a Girocode!
         //Girocode specification: http://www.europeanpaymentscouncil.eu/index.cfm/knowledge-bank/epc-documents/quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer/epc069-12-quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer1/

         private string br = "\n";
         private readonly string iban, bic, name, purposeOfCreditTransfer, remittanceInformation, messageToGirocodeUser;
         private readonly decimal amount;
         private readonly GirocodeVersion version;
         private readonly GirocodeEncoding encoding;
         private readonly TypeOfRemittance typeOfRemittance;


         /// <summary>
         /// Generates the payload for a Girocode (QR-Code with credit transfer information).
         /// Attention: When using Girocode payload, QR code must be generated with ECC level M!
         /// </summary>
         /// <param name="iban">Account number of the Beneficiary. Only IBAN is allowed.</param>
         /// <param name="bic">BIC of the Beneficiary Bank.</param>
         /// <param name="name">Name of the Beneficiary.</param>
         /// <param name="amount">Amount of the Credit Transfer in Euro.
         /// (Amount must be more than 0.01 and less than 999999999.99)</param>
         /// <param name="remittanceInformation">Remittance Information (Purpose-/reference text). (optional)</param>
         /// <param name="typeOfRemittance">Type of remittance information. Either structured (e.g. ISO 11649 RF Creditor Reference) and max. 35 chars or unstructured and max. 140 chars.</param>
         /// <param name="purposeOfCreditTransfer">Purpose of the Credit Transfer (optional)</param>
         /// <param name="messageToGirocodeUser">Beneficiary to originator information. (optional)</param>
         /// <param name="version">Girocode version. Either 001 or 002. Default: 001.</param>
         /// <param name="encoding">Encoding of the Girocode payload. Default: ISO-8859-1</param>
         public Girocode(string iban, string bic, string name, decimal amount, string remittanceInformation = "", TypeOfRemittance typeOfRemittance = TypeOfRemittance.Unstructured, string purposeOfCreditTransfer = "", string messageToGirocodeUser = "", GirocodeVersion version = GirocodeVersion.Version1, GirocodeEncoding encoding = GirocodeEncoding.ISO_8859_1)
         {
            this.version = version;
            this.encoding = encoding;
            if (!IsValidIban(iban))
               throw new GirocodeException("The IBAN entered isn't valid.");
            this.iban = iban.Replace(" ", "").ToUpper();
            if (!IsValidBic(bic))
               throw new GirocodeException("The BIC entered isn't valid.");
            this.bic = bic.Replace(" ", "").ToUpper();
            if (name.Length > 70)
               throw new GirocodeException("(Payee-)Name must be shorter than 71 chars.");
            this.name = name;
            if (amount.ToString().Replace(",", ".").Contains(".") && amount.ToString().Replace(",", ".").Split('.')[1].TrimEnd('0').Length > 2)
               throw new GirocodeException("Amount must have less than 3 digits after decimal point.");
            if (amount < 0.01m || amount > 999999999.99m)
               throw new GirocodeException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
            this.amount = amount;
            if (purposeOfCreditTransfer.Length > 4)
               throw new GirocodeException("Purpose of credit transfer can only have 4 chars at maximum.");
            this.purposeOfCreditTransfer = purposeOfCreditTransfer;
            if (typeOfRemittance == TypeOfRemittance.Unstructured && remittanceInformation.Length > 140)
               throw new GirocodeException("Unstructured reference texts have to shorter than 141 chars.");
            if (typeOfRemittance == TypeOfRemittance.Structured && remittanceInformation.Length > 35)
               throw new GirocodeException("Structured reference texts have to shorter than 36 chars.");
            this.typeOfRemittance = typeOfRemittance;
            this.remittanceInformation = remittanceInformation;
            if (messageToGirocodeUser.Length > 70)
               throw new GirocodeException("Message to the Girocode-User reader texts have to shorter than 71 chars.");
            this.messageToGirocodeUser = messageToGirocodeUser;
         }

         public override string ToString()
         {
            var girocodePayload = "BCD" + br;
            girocodePayload += ((version == GirocodeVersion.Version1) ? "001" : "002") + br;
            girocodePayload += (int)encoding + 1 + br;
            girocodePayload += "SCT" + br;
            girocodePayload += bic + br;
            girocodePayload += name + br;
            girocodePayload += iban + br;
            girocodePayload += $"EUR{amount:0.00}".Replace(",", ".") + br;
            girocodePayload += purposeOfCreditTransfer + br;
            girocodePayload += ((typeOfRemittance == TypeOfRemittance.Structured)
                ? remittanceInformation
                : string.Empty) + br;
            girocodePayload += ((typeOfRemittance == TypeOfRemittance.Unstructured)
                ? remittanceInformation
                : string.Empty) + br;
            girocodePayload += messageToGirocodeUser;

            return ConvertStringToEncoding(girocodePayload, encoding.ToString().Replace("_", "-"));
         }

         public enum GirocodeVersion
         {
            Version1,
            Version2
         }

         public enum TypeOfRemittance
         {
            Structured,
            Unstructured
         }

         public enum GirocodeEncoding
         {
            UTF_8,
            ISO_8859_1,
            ISO_8859_2,
            ISO_8859_4,
            ISO_8859_5,
            ISO_8859_7,
            ISO_8859_10,
            ISO_8859_15
         }

         public class GirocodeException : Exception
         {
            public GirocodeException()
            {
            }

            public GirocodeException(string message)
                : base(message)
            {
            }

            public GirocodeException(string message, Exception inner)
                : base(message, inner)
            {
            }
         }
      }

      private static bool IsValidIban(string iban)
      {
         //Clean IBAN
         var ibanCleared = iban.ToUpper().Replace(" ", "").Replace("-", "");

         //Check for general structure
         var structurallyValid = Regex.IsMatch(ibanCleared, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");

         //Check IBAN checksum
         var sum = $"{ibanCleared.Substring(4)}{ibanCleared.Substring(0, 4)}".ToCharArray().Aggregate("", (current, c) => current + (char.IsLetter(c) ? (c - 55).ToString() : c.ToString()));
         decimal sumDec;
         if (!decimal.TryParse(sum, out sumDec))
            return false;
         var checksumValid = (sumDec % 97) == 1;

         return structurallyValid && checksumValid;
      }

      private static bool IsValidBic(string bic)
      {
         return Regex.IsMatch(bic.Replace(" ", ""), @"^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
      }

      private static string ConvertStringToEncoding(string message, string encoding)
      {
         Encoding iso = Encoding.GetEncoding(encoding);
         Encoding utf8 = Encoding.UTF8;
         byte[] utfBytes = utf8.GetBytes(message);
         byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);

         return iso.GetString(isoBytes, 0, isoBytes.Length);
      }

      public static bool ChecksumMod10(string digits)
      {
         if (string.IsNullOrEmpty(digits) || digits.Length < 2)
            return false;
         int[] mods = new int[] { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };

         int remainder = 0;
         for (int i = 0; i < digits.Length - 1; i++)
         {
            var num = Convert.ToInt32(digits[i]) - 48;
            remainder = mods[(num + remainder) % 10];
         }
         var checksum = (10 - remainder) % 10;
         return checksum == Convert.ToInt32(digits[digits.Length - 1]) - 48;
      }
   }
}