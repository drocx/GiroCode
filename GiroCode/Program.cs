using System;
using System.Diagnostics;
using System.IO;

namespace GiroCode
{
   static class Program
   {
      /// <summary>
      /// Der Haupteinstiegspunkt für die Anwendung.
      /// </summary>
      [STAThread]
      static void Main()
      {
         //https://de.wikipedia.org/wiki/EPC-QR-Code
         GiroCodeGenerator.Girocode code = new GiroCodeGenerator.Girocode(iban: "DE33100205000001194700", bic: "BFSWDE33BER", name: "Wikimedia Foerdergesellschaft", amount: 123.45m, remittanceInformation: "Spende fuer Wikipedia");

         string path = Path.Combine(Path.GetTempPath(), "Girocode.pdf");
         GiroCodeReport report = new GiroCodeReport(code.ToString());
         report.ExportToPdf(path);
         Process.Start(path);
      }
   }
}