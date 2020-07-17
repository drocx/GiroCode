using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;

namespace GiroCode
{
   public partial class GiroCodeReport : DevExpress.XtraReports.UI.XtraReport
   {
      public GiroCodeReport(string payload)
      {
         InitializeComponent();

         pPayload.Value = StringToByteArray(payload);
      }

      private byte[] StringToByteArray(string str)
      {
         System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
         return enc.GetBytes(str);
      }

   }
}
