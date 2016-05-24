using Common.Logging;
using Newtonsoft.Json;
using OCR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using WatcherCmd.Files.Interface;

namespace WatcherCmd.Files
{
    public class CertManager : IManager
    {
        private readonly ILog _logger;
        private readonly IWatcher _watcher;
        private APIClient _apiClient;
        private string _apiUrl;


        public CertManager(ILog logger, IWatcher watcher, APIClient client)
        {
            _logger = logger;
            _watcher = watcher;
            _apiClient = client;
            _apiUrl = ConfigurationManager.AppSettings["ApiURL"];

        }

        public void InitializeSystem()
        {

            _watcher.FileDetected += OnFileDetected;
            _watcher.Init(ConfigurationManager.AppSettings["CertFolder"]);

        }

        private void OnFileDetected(object sender, FileSystemEventArgs e)
        {
            ProcContributionFile(e.FullPath);
        }

        private void ProcContributionFile(string inputPath)
        {
            Console.WriteLine("PROCESANDO ARCHIVO: " + inputPath);
            string inputFile = Path.GetFileName(inputPath);
            int inputFilePages = PDFToImageConverter.GetPdfPages(inputPath);


            PDFToImageConverter coverter = new PDFToImageConverter();
            string firstContributor = string.Empty;

            string destinationPath = ConfigurationManager.AppSettings["DestinationPath"] + "\\" + firstContributor + "_" + System.DateTime.Today.ToString("ddMMyyyy") + ".pdf";

            for (int i = 0; i < inputFilePages; i++)
            {
                Console.WriteLine("pagina " + i);
                string outputFile = Path.GetFileNameWithoutExtension(inputPath) + "-" + i + ".tiff";
                string outputPngFile = Path.GetFileNameWithoutExtension(inputPath) + "-" + i + ".png";

                string fileOutputPath = Path.Combine(Path.GetTempPath(), outputFile);
                string fileOutputPngPath = Path.Combine(Path.GetTempPath(), outputPngFile);

                coverter.ConvertToImage(i, inputPath, fileOutputPath, 512, 512, ImageFormat.Tiff);

                coverter.ConvertToImage(i, inputPath, fileOutputPngPath, 50, 50, ImageFormat.Png);

                OCR.OcrTextReader reader = new OCR.OcrTextReader();
                OcrData data = reader.Read(fileOutputPath);

                ContributionPeriodsParser parser = new ContributionPeriodsParser();
                List<ContributionPeriod> results = parser.Parse(data.Text);

                if (results.Count == 0)
                {
                    break;
                }

                string uploadResult = UploadFile(fileOutputPngPath);

                string fileUrl = string.Empty;
                string fileAbsolutePath = string.Empty;

                if (!string.IsNullOrEmpty(uploadResult))
                {
                    fileUrl = uploadResult.Split('@')[0];
                    fileAbsolutePath = uploadResult.Split('@')[1];
                }

                ContributionPeriodDataDTO dataToSend = new ContributionPeriodDataDTO();
                dataToSend.ContributorId = ContributorPersonalData.ParseCotizationAccount(data.Text);
                destinationPath = ConfigurationManager.AppSettings["DestinationPath"] + "\\" + dataToSend.ContributorId + "_" + System.DateTime.Today.ToString("ddMMyyyy") + ".pdf";
                dataToSend.PathAbsoluto = destinationPath;
                dataToSend.CNAE = ContributorPersonalData.ParseCNAE(data.Text);
                dataToSend.SocialReason = ContributorPersonalData.ParseSocialReason(data.Text);
                dataToSend.NIF = ContributorPersonalData.ParseNIF(data.Text);

                foreach (ContributionPeriod period in results)
                {
                    dataToSend.ContributionPeriodsDTO.Add(new ContributionPeriodDTO()
                    {
                        MoneyContribution = period.MoneyContribution,
                        PeriodEnd = period.PeriodEnd,
                        PeriodStart = period.PeriodStart,
                        HighResFileId = fileUrl,
                        FileAbsolutePath = fileAbsolutePath
                    });
                }

                APIClient client = new APIClient(_apiUrl);
                client.Post("ContributionPeriods", dataToSend);

            }

            


            Console.WriteLine("moviendo archivo a " + destinationPath);
            if (File.Exists(destinationPath))
                File.Delete(destinationPath);

            File.Move(inputPath, destinationPath);

            Console.WriteLine("FIN PROCESO");
            Console.WriteLine("-----------");
        }

        private string UploadFile(string outputPath)
        {
            StreamReader sr = new StreamReader(outputPath, System.Text.Encoding.UTF8, true, 128);
            byte[] fileStream = ReadFully(sr.BaseStream);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(_apiUrl + "/UploadFile"));
            request.Method = "POST";
            request.ContentType = "application/octet-stream";

            var authorizationHeader = new AuthenticationHeaderValue( "Basic", APIContants.USER_PASSWORD_64BITS_ENCODED);

            request.Headers.Add(HttpRequestHeader.Authorization, authorizationHeader.ToString());

            Stream serverStream = request.GetRequestStream();
            serverStream.Write(fileStream, 0, fileStream.Length);
            serverStream.Close();

            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException a)
            {
                Console.WriteLine(a.Response);
                Console.WriteLine(a.Message);
                Console.WriteLine(a.InnerException);
                return "";
            }


            string result = string.Empty;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd(); // do something fun...
            }

            return result;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                if (input != null)
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}
