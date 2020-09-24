using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PracticeManagementSystem.Core
{
    public class Logger
    {
        public static void Addlog(Exception ex,string apiName)
        {
            string LogFileName = string.Empty;
            switch (apiName)
            {
                case "User":
                    LogFileName = "UserLog";
                    break;
                case "Practice":
                    LogFileName = "PracticeLog";
                    break;
                case "Patient":
                    LogFileName = "PatientLog";
                    break;
                case "HCP":
                    LogFileName = "HCPLog";
                    break;


            }
            string filePath = @"C:\Logs\"+LogFileName+".txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.Message);
                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                    ex = ex.InnerException;
                }
            }
        }
    }
}
