using DAY_ONE.DAL;
using DAY_ONE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace DAY_ONE.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class

        public ActionResult Names()
        {
            return View();
        }
        public ActionResult HtmltoPDF()
        {
            return View();
        }
        public ActionResult HtmltoPDF3()
        {
            return View();
        }
        //public void ExportToPDF(object sender, EventArgs e)
        //{
        //    try

        //    {

        //        string strHtml = string.Empty;

        //        string htmlFileName = Server.MapPath("~") + "\\Views\\" + "\\Class\\" + "Names.cshtml";

        //        string pdfFileName = Request.PhysicalApplicationPath + "\\Views\\" + "\\Class\\" + "ConvertHTMLToPDF.pdf";

        //        FileStream fsHTMLDocument = new FileStream(htmlFileName, FileMode.Open, FileAccess.Read);

        //        StreamReader srHTMLDocument = new StreamReader(fsHTMLDocument);

        //        strHtml = srHTMLDocument.ReadToEnd();

        //        srHTMLDocument.Close();

        //        strHtml = strHtml.Replace("\r\n", "");

        //        strHtml = strHtml.Replace("\0", "");

        //        using (Stream fs = new FileStream(Request.PhysicalApplicationPath + "\\Views\\" + "\\Class\\" + "ConvertHTMLToPDF.pdf", FileMode.Create))

        //        {

        //            using (Document doc = new Document(PageSize.A4))

        //            {

        //                PdfWriter writer = PdfWriter.GetInstance(doc, fs);

        //                doc.Open();

        //                using (StringReader sr = new StringReader(strHtml))

        //                {

        //                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);

        //                }

        //                doc.Close();

        //                Response.ContentType = "application/pdf";

        //                Response.AddHeader("content-disposition", "attachment;filename=sample.pdf");

        //                Response.Cache.SetCacheability(HttpCacheability.NoCache);

        //                Response.Write(doc);

        //            }

        //        }

        //    }

        //    catch (Exception ex)

        //    {

        //        Response.Write(ex.Message);

        //    }


        //}

        //protected void ExportToPDF(object sender, EventArgs e)
        //{
        //    StringReader sr = new StringReader(Request.Form[hfGridHtml.UniqueID]);
        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();
        //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //    pdfDoc.Close();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=HTML.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Write(pdfDoc);
        //    Response.End();
        //}

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {


                StringReader sr = new StringReader(GridHtml);

                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "StudentDetails.pdf");
            }
        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult Export(string htmlContent)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader sr = new StringReader(htmlContent);
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //        pdfDoc.Close();
        //        return File(stream.ToArray(), "application/pdf", "StudentDetails.pdf");
        //    }
        //}
        [HttpPost]
        public JsonResult SaveData(ClassDBModel _dbModel)
        {
            int Result = 0;
            if (_dbModel.ClassID > 0)
            {
                Result = UpdateName(_dbModel);
            }
            else
            {
                Result = SaveName(_dbModel);
            }
            if (Result > 0)
                return Json(new { Success = true });
            else
                return Json(new { Success = false });
        }

        [HttpGet]
        public JsonResult LoadAllData()
        {
            List<ClassDBModel> _dbModelList = new List<ClassDBModel>();
            _dbModelList = LoadDataFromDataSet();
            return this.Json(_dbModelList, JsonRequestBehavior.AllowGet);
        }
        private List<ClassDBModel> LoadDataFromDataSet()
        {
            List<ClassDBModel> _modelList = new List<ClassDBModel>();
            SqlConnection conn = new SqlConnection(DBConnection.GetConnection());
            conn.Open();
            SqlCommand dAd = new SqlCommand("SELECT * FROM Class ORDER BY ClassID DESC", conn);
            SqlDataAdapter sda = new SqlDataAdapter(dAd);
            dAd.CommandType = CommandType.Text;
            DataTable dt = new DataTable();
            try
            {
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    _modelList = (from DataRow row in dt.Rows
                                  select new ClassDBModel
                                  {
                                      ClassID = Convert.ToInt32(row["ClassID"].ToString()),
                                      ClassName = row["ClassName"].ToString()
                                  }).ToList();
                }
                return _modelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }


        private int SaveName(ClassDBModel _dbModel)
        {
            SqlConnection conn = new SqlConnection(DBConnection.GetConnection());

            conn.Open();
            SqlCommand dCmd = new SqlCommand(@"INSERT INTO Class (ClassName, AddedBy)
                                              VALUES(@ClassName, @AddedBy)", conn);
            dCmd.CommandType = System.Data.CommandType.Text;

            try
            {
                dCmd.Parameters.AddWithValue("@ClassID", 0);
                dCmd.Parameters.AddWithValue("@ClassName", _dbModel.ClassName);
                dCmd.Parameters.AddWithValue("@AddedBy", "Iqbal");
                return dCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dCmd.Dispose();
                conn.Close();
                conn.Dispose();

            }
        }
        [HttpPost]
        public JsonResult DeleteSelectedClass(ClassDBModel _dbModel)
        {
            int _result = 0;
            _result = DeleteClassName(_dbModel);
            if (_result > 0)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        private int DeleteClassName(ClassDBModel dbModel)
        {
            SqlConnection conn = new SqlConnection(DBConnection.GetConnection());
            conn.Open();
            SqlCommand dCmd = new SqlCommand("DELETE FROM Class WHERE ClassID = @ClassID", conn);
            dCmd.CommandType = CommandType.Text;
            try
            {
                dCmd.Parameters.AddWithValue("@ClassID", dbModel.ClassID);
                return dCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dCmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        [HttpPost]
        public JsonResult LoadSelectedClass(ClassDBModel _dbModel)
        {
            List<ClassDBModel> _dbModelList = new List<ClassDBModel>();
            _dbModelList = LoadSelectedClassName(_dbModel);
            return this.Json(_dbModelList, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Info(int ClassID, string reportTypes)
        //{
        //    try
        //    {


        //        //string reportTypes = "EXCELOPENXML";

        //        DataSet ds = new DataSet();
        //        LocalReport localReport = new LocalReport();

        //        ds = LoadAllCG01Report(ClassID);
        //        localReport.ReportPath = Server.MapPath("~/Reports/Report1.rdlc");


        //        ReportDataSource reportDataSource1 = new ReportDataSource();

        //        reportDataSource1.Name = "DataSet1";
        //        reportDataSource1.Value = ds.Tables[0];

        //        //ReportDataSource reportDataSource2 = new ReportDataSource();
        //        //reportDataSource2.Name = "Info1";
        //        //reportDataSource2.Value = ds.Tables[0];

        //        ReportParameter ClassName = new ReportParameter("ClassName", ds.Tables[0].Rows[0]["ClassName"].ToString());
        //        //ReportParameter PCostingRef = new ReportParameter("CostingRef", ds.Tables[0].Rows[0]["CostingRef"].ToString());
        //        //ReportParameter PCompanyName = new ReportParameter("CompanyName", ds.Tables[0].Rows[0]["CompanyName"].ToString());
        //        //ReportParameter PBuyerName = new ReportParameter("BuyerName", ds.Tables[0].Rows[0]["BuyerName"].ToString());
        //        //ReportParameter PStyleID = new ReportParameter("StyleID", " ");


        //        localReport.SetParameters(parameters: new ReportParameter[] { ClassName });
        //        //PCostingRef, PCompanyName, PBuyerName,
        //        ////PStyleID,PStyleName, PProductTypeName, PSAM, PCM,PFOB, POrderQty, PTargetPerDay, PEfficiency,PRemarks,Commission,
        //        ////NegoFabCost,NegoTrimCost,NegoWashCost,NegoArtCost,NegoTestCost,NegoOtherCost,TotalFabCost,TotalTrimCost,
        //        ////TotalWashCost,TotalEmbCost,TotalOtherCost});

        //        localReport.Refresh();
        //        localReport.DataSources.Add(reportDataSource1);
        //        //localReport.DataSources.Add(reportDataSource2);
        //        string reportType = reportTypes;
        //        string mimeType;
        //        string encoding;
        //        string fileNameExtension = "";
        //        //The DeviceInfo settings should be changed based on the reportType            
        //        //http://msdn2.microsoft.com/en-us/library/ms155397.aspx            
        //        string deviceInfo = "<DeviceInfo>" +
        //            "  <OutputFormat>PDF</OutputFormat>" +
        //                "  <PageWidth>8.50in</PageWidth>" +
        //                "  <PageHeight>11.69in</PageHeight>" +
        //                "  <MarginTop>.2in</MarginTop>" +
        //                "  <MarginLeft>.5in</MarginLeft>" +
        //                "  <MarginRight>.5in</MarginRight>" +
        //                "  <MarginBottom>.2in</MarginBottom>" +
        //            "</DeviceInfo>";
        //        Warning[] warnings;
        //        string[] streams;
        //        byte[] renderedBytes;

        //        renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        //        if (reportType == "EXCELOPENXML")
        //        {
        //            return File(renderedBytes, mimeType, "Info.xlsx");
        //        }
        //        else
        //        {
        //            return File(renderedBytes, mimeType, "Info.pdf");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public DataSet LoadAllCG01Report(int ClassID)
        //{
        //    List<ClassDBModel> _modelList = new List<ClassDBModel>();
        //    SqlConnection conn = new SqlConnection(DBConnection.GetConnection());
        //    conn.Open();
        //    SqlCommand dAd = new SqlCommand("SELECT * FROM Class WHERE ClassID=@ClassID", conn);
        //    SqlDataAdapter sda = new SqlDataAdapter(dAd);
        //    dAd.CommandType = CommandType.Text;
        //    dAd.Parameters.AddWithValue("@ClassID", ClassID);
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        sda.Fill(ds);


        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        //UtilityOptions.ErrorLog(ex.ToString(), MethodBase.GetCurrentMethod().Name);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //        dAd.Dispose();
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}

        private List<ClassDBModel> LoadSelectedClassName(ClassDBModel dbModel)
        {
            List<ClassDBModel> _modelList = new List<ClassDBModel>();
            SqlConnection conn = new SqlConnection(DBConnection.GetConnection());
            conn.Open();
            SqlCommand dAd = new SqlCommand("SELECT * FROM Class WHERE ClassID=@ClassID", conn);
            SqlDataAdapter sda = new SqlDataAdapter(dAd);
            dAd.CommandType = CommandType.Text;
            dAd.Parameters.AddWithValue("@ClassID", dbModel.ClassID);
            DataTable dt = new DataTable();
            try
            {
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    _modelList = (from DataRow row in dt.Rows
                                  select new ClassDBModel
                                  {
                                      ClassID = Convert.ToInt32(row["ClassID"].ToString()),
                                      ClassName = row["ClassName"].ToString(),
                                      AddedBy = row["AddedBy"].ToString()
                                  }).ToList();
                }
                return _modelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt.Dispose();
                dAd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }
        private int UpdateName(ClassDBModel dbModel)
        {
            SqlConnection conn = new SqlConnection(DBConnection.GetConnection());
            conn.Open();
            SqlCommand dCmd = new SqlCommand("UPDATE Class SET ClassName=@ClassName,AddedBy=@AddedBy WHERE ClassID=@ClassID ", conn);
            dCmd.CommandType = CommandType.Text;
            try
            {
                dCmd.Parameters.AddWithValue("@ClassName", dbModel.ClassName);
                dCmd.Parameters.AddWithValue("@ClassID", dbModel.ClassID);
                dCmd.Parameters.AddWithValue("@AddedBy", "Forhad");
                return dCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dCmd.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }
        //public byte[] GeneratePdf(string finalHtml)
        //{
        //    Byte[] bytes;

        //    // do some additional cleansing to handle some scenarios that are out of control with the html data 
        //    finalHtml = finalHtml.Replace("<br>", "<br />");

        //    // Create a stream that we can write to, in this case a MemoryStream
        //    using (var ms = new MemoryStream())
        //    {
        //        // Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
        //        using (var doc = new Document())
        //        {
        //            // Create a writer that's bound to our PDF abstraction and our stream
        //            using (var writer = PdfWriter.GetInstance(doc, ms))
        //            {
        //                // Open the document for writing
        //                doc.Open();

        //                using (var srHtml = new StringReader(finalHtml))
        //                {
        //                    // Parse the HTML
        //                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
        //                }

        //                doc.Close();
        //            }
        //        }

        //        // After all of the PDF "stuff" above is done and closed but **before** we
        //        // close the MemoryStream, grab all of the active bytes from the stream
        //        bytes = ms.ToArray();

        //        return bytes;
        //    }
        //}
        //public ActionResult DownloadPDFIText(string htmlContent)
        //{
        //    try
        //    {
        //        TestProject.Helper.ITextSharp iText = new ITextSharp();
        //        byte[] bytes = iText.GeneratePdf(htmlContent);

        //        var filePath = Server.MapPath("~/Pdf/Report.pdf");
        //        System.IO.File.WriteAllBytes(filePath, bytes);
        //        return Json(new
        //        {
        //            Valid = true,
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            Valid = false,
        //        });
        //    }
        //}


        //public void DownloadPDF(string GridHtml)
        //{
        //    string HTMLContent = GridHtml;
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.BinaryWrite(GetPDF(HTMLContent));
        //    Response.End();
        //}

        //public byte[] GetPDF(string pHTML)
        //{
        //    byte[] bPDF = null;

        //    MemoryStream ms = new MemoryStream();
        //    TextReader txtReader = new StringReader(pHTML);

        //    // 1: create object of a itextsharp document class  
        //    Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

        //    // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
        //    PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

        //    // 3: we create a worker parse the document  
        //    HTMLWorker htmlWorker = new HTMLWorker(doc);

        //    // 4: we open document and start the worker on the document  
        //    doc.Open();
        //    htmlWorker.StartDocument();


        //    // 5: parse the html into the document  
        //    htmlWorker.Parse(txtReader);

        //    // 6: close the document and the worker  
        //    htmlWorker.EndDocument();
        //    htmlWorker.Close();
        //    doc.Close();

        //    bPDF = ms.ToArray();

        //    return bPDF;
        //}
        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportPDF(string GridHtml2)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string GridHtml = GridHtml2.Replace("replace", "/").Replace("<br>", "<br />");
                StringReader sr = new StringReader(GridHtml);

                //string imageURL = Server.MapPath("~/UploadFile/Pakiza.png");
                ////Chunk c1 = new Chunk("A chunk represents an isolated string. ");


                //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                //jpg.ScaleToFit(70f, 80f);



                //jpg.SpacingBefore = 10f;

                ////Give some space after the image

                //jpg.SpacingAfter = 1f;
                //jpg.SetAbsolutePosition(700, 700);
                //jpg.Alignment = Element.ALIGN_LEFT;

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
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();

                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }

    }
}