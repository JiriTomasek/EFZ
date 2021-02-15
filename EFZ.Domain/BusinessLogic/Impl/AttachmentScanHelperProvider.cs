using System;
using EFZ.Domain.BusinessLogic.Interface;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.IO;

namespace EFZ.Domain.BusinessLogic.Impl
{
    public class AttachmentScanHelperProvider : IAttachmentScanHelperProvider
    {
        public AttachmentScanHelperProvider()
        {

        }
        public List<string> GetBarcodeFromStream(Stream stream)
        {
            var results = new List<string>();
            var reader = Spire.Barcode.BarcodeScanner.Scan(stream, true);

            if (reader != null)
            {
               results.AddRange(reader);
            }

            return results;

        }


        public MemoryStream ConvertImageToPdf(FileStream stream)
        {
            var resultStream = new MemoryStream();
            var doc = new PdfDocument();
            doc.Pages.Add(new PdfPage());
            var xgr = XGraphics.FromPdfPage(doc.Pages[0]);
            var img = XImage.FromStream(stream);

            xgr.DrawImage(img, 0, 0);
            doc.Save(resultStream);
            doc.Close();
            return resultStream;
        }

        //private void UpdateReaderHints()
        //{

        //        _readerHints.BarcodeRotation = BarcodeRotation.None;
        //   }
           
        //    _readerHints.MaxNumOfBarcodes = (int)nudMaxNumOfBarcodes.Value;

        //    _readerHints.ScanlineThickness = (int)nudScanlineThickness.Value;
        //    _readerHints.ScanlineDistance = (int)nudScanlineDistance.Value;
        //    _readerHints.ScanDirection = (ScanDirection)Enum.Parse(typeof(ScanDirection), cboScanDirection.SelectedItem.ToString());
        //    _readerHints.ScanTimeout = (int)nudScanTimeout.Value;

        //    int minSymbolLength = (int)nudMinNumOfDataChars.Value;
        //    _readerHints.CodabarMinSymbolLength = minSymbolLength;
        //    _readerHints.Code128MinSymbolLength = minSymbolLength;
        //    _readerHints.Code39MinSymbolLength = minSymbolLength;
        //    _readerHints.Code93MinSymbolLength = minSymbolLength;
        //    _readerHints.Industrial2of5MinSymbolLength = minSymbolLength;
        //    _readerHints.Interleaved2of5MinSymbolLength = minSymbolLength;

        //}
    }
}