using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.InteropServices;
using System.IO;
using VolPro.Core.Extensions;
using gridreport;
using VolPro.Core.Report.PInvokeWindows;
using BinaryObject = VolPro.Core.Report.PInvokeWindows.BinaryObject;
using E2IMGOption = VolPro.Core.Report.PInvokeWindows.E2IMGOption;

namespace VolPro.Core.common
{
    public class ReportGeneratorWindows
    {
        private ReportWindows report;
        public ReportWindows Report => report;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReportGeneratorWindows(IWebHostEnvironment hostingEnvironment)
        {
              _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 初始化報表對象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ReportWindows InitReport()
        {
            report = new ReportWindows();
            if (report?.NativeHandle == null) throw new Exception("Create report failed.");
            return report;
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="reportID"></param>
        /// <exception cref="Exception"></exception>
        public void LoadReport(string reportID)
        {
            string reportPathFile = Path.Combine(_hostingEnvironment.ContentRootPath, reportID).ReplacePath();
            if (!File.Exists(reportPathFile)) throw new Exception("模板文件不存在:" + reportPathFile);
            bool success = report.LoadFromFile(reportPathFile);
            if (!success) throw new Exception(string.Format("载入報表模板 '{0}' 失败！", reportPathFile));
            Console.WriteLine(string.Format("载入報表模板 '{0}' 成功！", reportPathFile));
        }

        /// <summary>
        /// 加载報表數據
        /// </summary>
        /// <param name="DataText"></param>
        /// <exception cref="Exception"></exception>
        public void LoadReportData(string DataText)
        {
            bool success = report.LoadDataFromXML(DataText);
            if (!success)   throw new Exception(string.Format("载入報表數據:\r\n '{0}' \r\n失败！", DataText));
        }
        public FileData Generate(FileParameter fileParameter)
        {
            return Generate(fileParameter.type, fileParameter.filename, fileParameter.img);
        }
        private FileData Generate(string TypeText, string FileName, string ImageTypeText)
        {
            //确定导出數據類型及數據的ContentType
            ReportGenerateInfo GenerateInfo = new ReportGenerateInfo();
            GenerateInfo.Build(TypeText, ImageTypeText);

           BinaryObject ResultDataObject;
            if (GenerateInfo.IsGRD)
            {
               // Core.Report.PInvokeWindows.Report.GenerateDocumentData();
                ResultDataObject = report.GenerateDocumentData();
            }
            else
            {
                Report.PInvokeWindows.ExportOption ExportOption = report.PrepareExport(GenerateInfo.ExportType);

                if (GenerateInfo.ExportType == ExportType.IMG)
                {
                    E2IMGOption E2IMGOption = ExportOption.AsE2IMGOption;
                    E2IMGOption.ImageType = GenerateInfo.ImageType;
                    E2IMGOption.AllInOne = true; //所有页產生在一個图像文件中
                    //E2IMGOption.VertGap = 20;    //页之间設置20個像素的间距
                }

                ResultDataObject = report.ExportToBinaryObject();
                report.UnprepareExport();
            }



            //如果参數中没指定文件名，则用報表模板中的“標题”屬性設置一個默認文件名
            if (string.IsNullOrWhiteSpace(FileName))
            {
                FileName = report.Title;
                if (string.IsNullOrWhiteSpace(FileName)) FileName = "gridreport";
                FileName += "." + GenerateInfo.ExtFileBame;
            }
            if (ResultDataObject == null || ResultDataObject.DataSize <= 0) throw new Exception("未產生報表數據");


            byte[] managedArray = new byte[ResultDataObject.DataSize];
            Marshal.Copy(ResultDataObject.DataBuf, managedArray, 0, ResultDataObject.DataSize);

            FileData data = new FileData { FileContent = managedArray, FileName = FileName, ContentType = GenerateInfo.ContentType };
            return data;
        }
    }
}
