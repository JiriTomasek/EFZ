using System.Globalization;
using System.Text;
using EFZ.Entities.Entities;

namespace EFZ.Domain.BusinessLogic
{
    public static class HtmlStringBuilder
    {
        public static string HtmlInvoiceDocument(Invoice invoice)
        {
            var result = string.Empty;


            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";
            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";
            result += "<table width=\"100%\">";
            result += "<tbody>";
            result += "<tr>";
            result += "<td width=\"100%\" colspan=\"2\" style=\"width:100.0%;padding:0cm 5.4pt 0cm 5.4p; height:49.6pt\">";
            result += $"<p class=\"MsoNormal\" align=\"right\"><b><span style=\"font-size:14.0pt\">Faktura – č. {invoice.InvoiceNumber}<o:p></o:p></span></b></p>";
            result += $"<p class=\"MsoNormal\" align=\"right\"><b><span style=\"font-size:14.0pt\">Objednávka – č. {invoice.OrderNumber}<o:p></o:p></span></b></p>";
            result += "</td>";
            result += "</tr>"; 
            result += "<tr>";
            result += "<td width=\"56%\" valign=\"top\" style=\"width 56.88%;padding:0cm 5.4pt 0cm 5.4pt\">";
            result += "<p class=\"MsoNormal\" align=\"left\">";
            result += "<b>";
            result += "<span style=\"font-size:12.0pt\">Dodavatel:<o:p></o:p></span>";
            result += "</b>";
            result += "</p>";

            result += $"<p class=\"MsoNormal\" align=\"left\">{invoice.Company.Name}</p>";
            if(invoice.Company.Address != null && string.IsNullOrEmpty(invoice.Company.Address.AlternativeStreetNumber))
                result += $"<p class=\"MsoNormal\" align=\"left\">{invoice.Company.Address.StreetName} {invoice.Company.Address.StreetNumber}</p>";
            else if(invoice.Company.Address != null)
            {
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.Company.Address.StreetName} {invoice.Company.Address.StreetNumber} / {invoice.Company.Address.AlternativeStreetNumber}</p>";
               

            }
            if (invoice.Company.Address != null)
            {
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.Company.Address.PostalCode} {invoice.Company.Address.City}</p>";
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.Company.Address.State}</p>";

            }
            result +=
                $"<p class=\"MsoNormal\" align=\"left\">IČ: {invoice.Company.IC}</p>";
            result +=
                $"<p class=\"MsoNormal\" align=\"left\">DIČ: {invoice.Company.DIC}</p>";

            result += "</td>";

            result += "<td width=\"43%\" valign=\"top\" style=\"width:43.12%;padding 0cm 5.4pt 0cm 5.4pt\">";
            result += "<p class=\"MsoNormal\" align=\"left\">";
            result += "<b>";
            result += "<span style=\"font-size:12.0pt\">Odběratel:<o:p></o:p></span>";
            result += "</b>";
            result += "</p>";

            if(string.IsNullOrEmpty(invoice.InvoiceAddress?.FirstName))
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.InvoiceAddress?.FirstName} {invoice.InvoiceAddress?.LAstName}</p>";
            if (string.IsNullOrEmpty(invoice.InvoiceAddress?.Company))
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.InvoiceAddress?.Company}</p>";
            if (invoice.InvoiceAddress != null && string.IsNullOrEmpty(invoice.InvoiceAddress.AlternativeStreetNumber))
                result += $"<p class=\"MsoNormal\" align=\"left\">{invoice.InvoiceAddress.StreetName} {invoice.InvoiceAddress.StreetNumber}</p>";
            else if (invoice.InvoiceAddress != null)
            {
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.InvoiceAddress.StreetName} {invoice.InvoiceAddress.StreetNumber} / {invoice.InvoiceAddress.AlternativeStreetNumber}</p>";
             

            }if (invoice.InvoiceAddress != null)
            {
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.InvoiceAddress.PostalCode} {invoice.InvoiceAddress.City}</p>";
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">{invoice.InvoiceAddress.State}</p>"; 
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">IČ: {invoice.InvoiceAddress.IC}</p>";
                result +=
                    $"<p class=\"MsoNormal\" align=\"left\">DIČ: {invoice.InvoiceAddress.DIC}</p>";

            }

            result += "</td>";
            result += "</tr>";
            result += "</tbody>";
            result += "</table>";

            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";
            result += "<p class=\"MsoNormal\" align=\"left\" style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><b><span style=\"font-size:12.0pt\">Platební podmínky:<o:p></o:p></span></b></p>";
            result += $"<p class=\"MsoNormal\"align=\"left\" style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">Datum vystavení: {invoice.InvoiceDate.ToShortDateString()}</p>";
            result += "<p class=\"MsoNormal\" style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";


            result += "<div align=\"center\">";
            result += "<table class=\"MsoTableGrid\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse: collapse;border: none;\">";
            result += "<thead>";
            result += "<tr>";

            result += "<th width=\"100%\" colspan=\"7\" style=\"width: 100.0%;padding: 0cm 5.4pt 0cm 5.4pt\">";
            result += "<p class=\"MsoNormal\"><b>Přehled fakturovaného zboží/služeb:<o:p></o:p></b></p>";
            result += "</th>";

            result += "</tr>";

            result += "<tr style=height: 33.8pt>";

            result += "<th width=\"25%\" style=\"width: 25.8%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 33.8pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\"><b>Zboží/ služba<o:p></o:p></b></p>";
            result += "</th>";
            result += "<th width=\"12%\"  style=\"width: 12.06%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 33.8pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\"><b>Množství<o:p></o:p></b></p>";
            result += "</th>";
            result += "<th width=\"15%\" style=\"width: 15.64%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 33.8pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\"><b>Cena za ks bez DPH<o:p></o:p></b></p>";
            result += "</th>";
            result += "<th width=\"13%\" style=\"width: 13.8%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 33.8pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\"><b>Sazba DPH<o:p></o:p></b></p>";
            result += "</th>";
            result += "<th width=\"15%\"  style=\"width: 15.64%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 33.8pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\"><b>DPH<o:p></o:p></b></p>";
            result += "</th>";
            result += "<th width=\"15%\"style=\"width: 15.64%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 33.8pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\"><b>Celkem s DPH<o:p></o:p></b></p>";
            result += "</th>";

            result += "</tr>";
            result += "</thead>";

            result += "<tbody>";
            foreach (var invoiceInvoiceItem in invoice.InvoiceItems)
            {
                result += "<tr style=height: 28.6pt>";

                result += "<td width=\"25%\" style=\"width: 25.8%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
                result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\">{invoiceInvoiceItem.Name}</p>";
                result += "</td>";
                result += "<td width=\"12%\"  style=\"width: 12.06%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
                result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\">{invoiceInvoiceItem.Quantity}</p>";
                result += "</td>";
                result += "<td width=\"15%\"  style=\"width: 15.64%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
                result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\">{invoiceInvoiceItem.UnitPrice.ToString("C", new CultureInfo("cs-CZ"))}</p>";
                result += "</td>";
                result += "<td width=\"13%\"style=\"width: 13.8%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
                result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\">{(invoiceInvoiceItem.TaxRate / 100):P}</p>";
                result += "</td>";
                result += "<td width=\"15 style=\"width: 15.64%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
                result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\">{invoiceInvoiceItem.TotalTax.ToString("C", new CultureInfo("cs-CZ"))}</p>";
                result += "</td>";
                result += "<td width=\"15%\" style=\"width: 15.64%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
                result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt; line-height: normal\">{invoiceInvoiceItem.TotalNet.ToString("C", new CultureInfo("cs-CZ"))}</p>";
                result += "</td>";

                result += "</tr>";
            }

            result += "</tbody>";
            result += "</table>";
            result += "</div>";
            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";
            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";

            result += "<p class=\"MsoNormal\" align=\"center\" style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\">";
            result += $"<b>CELKEM K ÚHRADĚ:</b> {invoice.ItemsTotal.ToString("C", new CultureInfo("cs-CZ"))}";
            result += "</p>";
            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";
            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";


            result += "<table class=\"MsoTableGrid\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100.0%;border-collapse: collapse;border: none;\">";
            result += "<tbody>";
            result += "<tr>";

            result += "<td width=\"74%\" valign=\"top\" style=\"width: 74.14%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt;text-align:right; line-height: normal\"><b>Celkem bez DPH<o:p></o:p></b></p>";
            result += "</td>";
            result += "<td width=\"25%\" valign=\"top\"  style=\"width: 35.86%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
            result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt;text-align:right; line -height: normal\">{invoice.ItemsNet.ToString("C", new CultureInfo("cs-CZ"))}</p>";
            result += "</td>";
           

            result += "</tr>";
            result += "<tr>";

            result += "<td width=\"74%\" valign=\"top\" style=\"width: 74.14%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt;text-align:right; line-height: normal\"><b>Celkem DPH<o:p></o:p></b></p>";
            result += "</td>";
            result += "<td width=\"25%\"valign=\"top\"  style=\"width: 35.86%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
            result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt;text-align:right; line -height: normal\">{invoice.ItemsTax.ToString("C", new CultureInfo("cs-CZ"))}</p>";
            result += "</td>";


            result += "</tr>";
            result += "<tr>";

            result += "<td width=\"74%\" valign=\"top\" style=\"width: 74.14%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt;text-align:right; line-height: normal\"><b>Celkem s DPH<o:p></o:p></b></p>";
            result += "</td>";
            result += "<td width=\"35%\" valign=\"top\"  style=\"width: 35.86%;padding: 0cm 5.4pt 0cm 5.4pt; ;height: 28.6pt\">";
            result += $"<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom: 0cm; margin-bottom: .0001pt;text-align:right; line -height: normal\">{invoice.ItemsTotal.ToString("C", new CultureInfo("cs-CZ"))}</p>";
            result += "</td>";


            result += "</tr>";
            result += "</tbody>";
            result += "</table>";


            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";
            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";
            result += "<p class=\"MsoNormal\"  style=\"margin-bottom:0cm;margin-bottom:.0001pt;line-height:normal\"><o:p>&nbsp;</o:p></p>";

            result += "<p class=\"MsoNormal\" align=\"right\" style=\"margin-bottom:0cm;margin-bottom:.0001pt;text-align:right;line-height:normal\">";

            result += "Vystavil:<u>";

            result += "<span>";
            result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";


            result += "</span>";
            result += "</u>";

            result += "</p>";
            return result;
        }
    }
}