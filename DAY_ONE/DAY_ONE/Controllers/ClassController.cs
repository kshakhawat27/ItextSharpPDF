using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System.Web.UI;
using System.Drawing;
using iTextSharp.text.pdf.codec.wmf;
using System.Text;
using iTextSharp.tool.xml.pipeline;
using System.Collections.Generic;

namespace DAY_ONE.Controllers
{
    public class ClassController : Controller
    {
        //
        // GET: /Class/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HtmltoPDFBG()
        {
            return View();
        }
        public ActionResult HtmltoPDFull()
        {
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
        public ActionResult test2()
        {
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportPDF(string GridHtml2)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string GridHtml = GridHtml2.Replace("replace", "/").Replace("<br>", "<br />");
                StringReader sr = new StringReader(GridHtml);



                string imageURL = Server.MapPath("~/UploadFile/bcb_logo.png");
                ////Chunk c1 = new Chunk("A chunk represents an isolated string. ");


                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(620f, 1000f);



                //jpg.SpacingBefore = 10f;

                ////Give some space after the image

                //jpg.SpacingAfter = 10f;
                //jpg.SetAbsolutePosition(0,0);
                jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

                //Another use
                //Document pdfDoc = new Document(PageSize.A4, 8f, 8f, 20f, 0f);
                //PdfWriter PdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                //htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                //ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                //cssResolver.AddCssFile(Server.MapPath("~/css/styles.css"), true);
                //IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(pdfDoc, PdfWriter)));
                //var worker = new XMLWorker(pipeline, true);
                //var xmlParse = new XMLParser(true, worker);
                //pdfDoc.Open();
                ////for (int i = 1; i < 4; i++)
                ////{

                ////    pdfDoc.Add(c1);

                ////}
                ////pdfDoc.Add(jpg);


                //xmlParse.Parse(sr);
                //xmlParse.Flush();
                //pdfDoc.Close();

                //--- Another ProcessInfo


                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                pdfDoc.Open();

                pdfDoc.Add(jpg);
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();

                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportPDFAgain(string GridHtml2)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string GridHtml = GridHtml2.Replace("replace", "/").Replace("<br>", "<br />");
                StringReader sr = new StringReader(GridHtml);



                string imageURL = Server.MapPath("~/UploadFile/bcb_logo.png");
                ////Chunk c1 = new Chunk("A chunk represents an isolated string. ");


                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScaleToFit(620f, 1000f);



                //jpg.SpacingBefore = 10f;

                ////Give some space after the image

                //jpg.SpacingAfter = 10f;
                //jpg.SetAbsolutePosition(0,0);
                jpg.Alignment = iTextSharp.text.Image.UNDERLYING;

                //Another use
                //Document pdfDoc = new Document(PageSize.A4, 8f, 8f, 20f, 0f);
                //PdfWriter PdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                //htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                //ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                //cssResolver.AddCssFile(Server.MapPath("~/css/styles.css"), true);
                //IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(pdfDoc, PdfWriter)));
                //var worker = new XMLWorker(pipeline, true);
                //var xmlParse = new XMLParser(true, worker);
                //pdfDoc.Open();
                ////for (int i = 1; i < 4; i++)
                ////{

                ////    pdfDoc.Add(c1);

                ////}
                ////pdfDoc.Add(jpg);


                //xmlParse.Parse(sr);
                //xmlParse.Flush();
                //pdfDoc.Close();

                //--- Another ProcessInfo


                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                pdfDoc.Open();

                pdfDoc.Add(jpg);
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();

                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }



        public class PDFHeaderFooter : PdfPageEventHelper
        {
            public int count;
            protected ElementList header, footer;

            public PDFHeaderFooter(string headerHtml, string FooterHtml)
            {
                header = XMLWorkerHelper.ParseToElementList(headerHtml, null);
                footer = XMLWorkerHelper.ParseToElementList(FooterHtml, null);
            }

            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);

                PdfPTable headerTbl = new PdfPTable(1);
                PdfPTable footerTbl = new PdfPTable(1);
                //headerTbl.WidthPercentage = 100f;
                headerTbl.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]
                footerTbl.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                PdfPCell cell = new PdfPCell();
                cell.Border = 0;
                PdfPCell cell1 = new PdfPCell();
                cell1.Border = 0;
                //foreach (IElement e in header)
                //{
                //    if (count < 2)
                //    {
                //        cell.AddElement(e);
                //    }
                //    count++;
                //}
                foreach (IElement e in header)
                {


                    cell.AddElement(e);

                }


                foreach (IElement e in footer)
                {
                    cell1.AddElement(e);
                }

                headerTbl.AddCell(cell);
                headerTbl.WriteSelectedRows(0, -1, 20, 820, writer.DirectContent);
                footerTbl.AddCell(cell1);
                footerTbl.WriteSelectedRows(0, -1, 20, 40, writer.DirectContent);
            }

        }




        [HttpPost]
        [ValidateInput(false)]

        public FileResult Render(string contentHtml, string headerHtml2, string FooterHtml2)
        {


            string GridHtml = contentHtml.Replace("replace", "/").Replace("<br>", "<br />");
            string headerHtml = headerHtml2.Replace("replace", "/").Replace("<br>", "<br />");
            string FooterHtml = FooterHtml2.Replace("replace", "/").Replace("<br>", "<br />");
            //List<string> cssFiles = new List<string>();

            //var input = new MemoryStream(Encoding.UTF8.GetBytes(headerHtml));
            //cssFiles.Add(@"~/Content/themes/TestStyle.css");
            iTextSharp.text.pdf.BarcodeQRCode qrcode = new BarcodeQRCode("Pakiza Technavation Limited", 50, 50, null);
            iTextSharp.text.Image img1 = qrcode.GetImage();

            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.A4, 10f, 10f, 50f, 70f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                    
                    writer.PageEvent = new PDFHeaderFooter(headerHtml, FooterHtml);

                    doc.Open();
                    StringReader sr = new StringReader(GridHtml);
                    //HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                    //htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                    //ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                    //cssFiles.ForEach(i => cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath(i),(true)));
                    //IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(doc, writer)));

                    //var worker = new XMLWorker(pipeline, true);
                    //var xmlParse = new XMLParser(true, worker);
                    //xmlParse.Parse(input);
                    //xmlParse.Flush();
                    doc.Add(img1);
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);


                }

                //return data;
                return File(ms.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public static byte[] RenderTest(string contentHtml, string headerHtml2, string FooterHtml2)
        {


            string GridHtml = contentHtml.Replace("replace", "/").Replace("<br>", "<br />");
            string headerHtml = headerHtml2.Replace("replace", "/").Replace("<br>", "<br />");
            string FooterHtml = FooterHtml2.Replace("replace", "/").Replace("<br>", "<br />");
            List<string> cssFiles = new List<string>();
    

            var input = new MemoryStream(Encoding.UTF8.GetBytes(headerHtml));
            cssFiles.Add(@"~/Content/themes/TestStyle.css");

            byte[] Result;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.A4, 10f, 10f, 50f, 70f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);


                    writer.PageEvent = new PDFHeaderFooter(headerHtml, FooterHtml);

                    doc.Open();
                    StringReader sr = new StringReader(GridHtml);
                    HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                    ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                    cssFiles.ForEach(i => cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath(i), (true)));
                    IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(doc, writer)));

                    var worker = new XMLWorker(pipeline, true);
                    var xmlParse = new XMLParser(true, worker);
                    xmlParse.Parse(input);
                    xmlParse.Flush();
                  
                    doc.Close();
                     Result = ms.GetBuffer();


                }

                //return data;
                return Result;
            }
        }


    }
}
