using System;
using System.Collections.Generic;
using IOServices.Models;
using System.IO;
using iText.Kernel.Pdf;

namespace IOServices.Services
{
    public class PdfService
    {
        public static List<PdfModel> GetPdfList(string path)
        {
            DirectoryInfo di = new(path);
            List<PdfModel> Pdflist = new();
            var files = di.GetFiles("*.pdf", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                PdfDocument pdfDocument = new(new PdfReader(file.FullName));
                PdfModel pdfModel = new();
                pdfModel.FileName = file.Name;
                pdfModel.FullPath = file.FullName;
                pdfModel.NumberOfPages = pdfDocument.GetNumberOfPages();
                Pdflist.Add(pdfModel);
            }
            return Pdflist;
        }

    }
}
