using System.Collections.Generic;
using System.IO;

namespace EFZ.Domain.BusinessLogic.Interface
{
    public interface IAttachmentScanHelperProvider
    {
        List<string> GetBarcodeFromStream(Stream stream);
        MemoryStream ConvertImageToPdf(FileStream stream);

    }
}