namespace GiroCode
{
   partial class GiroCodeReport
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
         this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
         this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
         this.Detail = new DevExpress.XtraReports.UI.DetailBand();
         this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
         this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
         this.pPayload = new DevExpress.XtraReports.Parameters.Parameter();
         ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
         // 
         // TopMargin
         // 
         this.TopMargin.HeightF = 128.125F;
         this.TopMargin.Name = "TopMargin";
         // 
         // BottomMargin
         // 
         this.BottomMargin.Name = "BottomMargin";
         // 
         // Detail
         // 
         this.Detail.Name = "Detail";
         // 
         // xrBarCode1
         // 
         this.xrBarCode1.AutoModule = true;
         this.xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BinaryData", "?pPayload")});
         this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(129.1667F, 10.00001F);
         this.xrBarCode1.Name = "xrBarCode1";
         this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
         this.xrBarCode1.ShowText = false;
         this.xrBarCode1.SizeF = new System.Drawing.SizeF(141.6667F, 128.9584F);
         qrCodeGenerator1.CompactionMode = DevExpress.XtraPrinting.BarCode.QRCodeCompactionMode.Byte;
         qrCodeGenerator1.ErrorCorrectionLevel = DevExpress.XtraPrinting.BarCode.QRCodeErrorCorrectionLevel.M;
         qrCodeGenerator1.Version = DevExpress.XtraPrinting.BarCode.QRCodeVersion.Version5;
         this.xrBarCode1.Symbology = qrCodeGenerator1;
         // 
         // ReportHeader
         // 
         this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode1});
         this.ReportHeader.HeightF = 161.4583F;
         this.ReportHeader.Name = "ReportHeader";
         // 
         // pPayload
         // 
         this.pPayload.Name = "pPayload";
         // 
         // GiroCodeReport
         // 
         this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader});
         this.Font = new System.Drawing.Font("Arial", 9.75F);
         this.Margins = new System.Drawing.Printing.Margins(100, 100, 128, 100);
         this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.pPayload});
         this.Version = "19.2";
         ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

      }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.Parameters.Parameter pPayload;
    }
}
