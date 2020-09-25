
using System;
using System.Collections.Generic;
using System.Text;

namespace PracticeManagementSystem.Core
{
    public class Report
    {
        public static void GeneratePDF(HCPInteractionInfo hCPInteractionInfo,DocumentInfo documentInfo,PatientInfo pobj,UserInfo dobj)
        {
            // Render any HTML fragment or document to HTML
            var Renderer = new IronPdf.HtmlToPdf();

            string ReportDetails = "<h1 &quot; color:#4A235A;text-align: center;font-size:32px &quot;><i>Document Name :"+ documentInfo.DocName+"</i></h1>"
              
                + "<p > Patient Name     :   " + pobj.PatientName + "</p><br/>"
                + "<p>  HCP Name         :   " + dobj.UserName + "</p><br/>"
                + "<p> VisitDate         :   " + hCPInteractionInfo.VisitDate + "</p><br/>"         
                + "<p> DiagnosisDetails  :   " + hCPInteractionInfo.DiagnosisDetails + "</p><br/>"
                + "<p> HCPComments       :   " + hCPInteractionInfo.HCPComments + "</p><br/>"
                + "<h3>Signature Attestation: " + documentInfo.HCPSignature + "</h3><br/>";
                
            var PDF = Renderer.RenderHtmlAsPdf(ReportDetails);
            var OutputPath = @"C:\PDF\Report.pdf";
            PDF.SaveAs(OutputPath);
            // This neat trick opens our PDF file so we can see the result in our default PDF viewer
            //System.Diagnostics.Process.Start(OutputPath);
            
        }
    }
}


