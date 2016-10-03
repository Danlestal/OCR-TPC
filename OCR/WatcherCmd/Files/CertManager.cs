using OCR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using WatcherCmd.Files.Interface;


namespace WatcherCmd.Files
{
    public class CertManager : IManager
    {
        private readonly IWatcher _watcher;
        private APIClient _apiClient;
        private string _apiUrl;
        private ILogger _logger;

        public CertManager(ILogger logger, IWatcher watcher, APIClient client)
        {
            _logger = logger;
            _watcher = watcher;
            _apiClient = client;
            _apiUrl = ConfigurationManager.AppSettings["ApiURL"];

        }

        public void InitializeSystem()
        {
            if (!Directory.Exists(ConfigurationManager.AppSettings["CertFolder"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["CertFolder"]);
            }

            string errorFolder = ConfigurationManager.AppSettings["CertFolder"] + "/ERROR";
            if (!Directory.Exists(errorFolder))
            {
                Directory.CreateDirectory(errorFolder);
            }

            _watcher.FileDetected += OnFileDetected;
            _watcher.Init(ConfigurationManager.AppSettings["CertFolder"]);
           
        }

        private void OnFileDetected(object sender, FileSystemEventArgs e)
        {
            try
            {
                // Lanzamos el proceso de borrado cada vez que se escanea.
                Util.DeleteFilesToDelete();

                ProcContributionFile(e.FullPath);
            }catch(Exception a)
            {
                _logger.Log("error en archivo: " + e.FullPath + "\n Excepcion: " + a.Message);
                MoveToErrorFolder(e.FullPath);
            }

            _logger.Log("FIN PROCESO");
        }

        private void MoveToErrorFolder(string fullPath)
        {
            string destination = Path.GetFileName(fullPath);
            string destinationFullPath = ConfigurationManager.AppSettings["CertFolder"] + "/ERROR/" + destination;
            if (!File.Exists(destinationFullPath))
            {
                File.Move(fullPath, destinationFullPath);
            }
        }

        private void ProcContributionFile(string inputPath)
        {
            Thread.Sleep(500);
            PDFToImageConverter coverter = new PDFToImageConverter();

            
            _logger.Log("PROCESANDO ARCHIVO: " + inputPath);
            string inputFile = Path.GetFileName(inputPath);
            int inputFilePages = coverter.GetPdfPages(inputPath);
            
            string firstContributor = string.Empty;

            string destinationPath = string.Empty;
            string destinationPathAbsoluto = string.Empty;

            for (int i = 0; i < inputFilePages; i++)
            {
                _logger.Log("pagina " + i);
                ContributionPeriodDataDTO dataToSend = new ContributionPeriodDataDTO();


                string outputFile = Path.GetFileNameWithoutExtension(inputPath) + "-" + i + ".tiff";
                string outputPngFile = Path.GetFileNameWithoutExtension(inputPath) + "-" + i + ".png";

                string fileOutputPath = Path.Combine(Path.GetTempPath(), outputFile);
                string fileOutputPngPath = Path.Combine(Path.GetTempPath(), outputPngFile);

                coverter.ConvertToImage(i, inputPath, fileOutputPath, 512, 512, ImageFormat.Tiff);

                coverter.ConvertToImage(i, inputPath, fileOutputPngPath, 50, 50, ImageFormat.Png);

                OCR.OcrTextReader reader = new OCR.OcrTextReader();
                OcrData data = reader.Read(fileOutputPath);

                ContributionPeriodsParser parser = new ContributionPeriodsParser();

               

                string uploadResult = UploadFile(fileOutputPngPath);

                string fileUrl = string.Empty;
                string fileAbsolutePath = string.Empty;

                if (!string.IsNullOrEmpty(uploadResult))
                {
                    fileUrl = uploadResult.Split('@')[0];
                    fileAbsolutePath = uploadResult.Split('@')[1];
                }


                var parseCAccount = new Tuple<bool, string>(false,"error");
                try
                {
                    parseCAccount = ContributorPersonalData.ParseCotizationAccount(data.Text);
                }
                catch (ArgumentException)
                {
                    _logger.Log("ERROR CRITICO, no se ha podido parsear la CC del archivo " + inputPath + ", revisar el archivo.");
                    destinationPath = ConfigurationManager.AppSettings["DestinationPath"] + "\\error_" + System.DateTime.Today.ToString("ddMMyyyy") + ".pdf";
                    destinationPathAbsoluto = ConfigurationManager.AppSettings["DestinationPathBBDD"] + "\\" + dataToSend.ContributorId + "_" + System.DateTime.Today.ToString("ddMMyyyy") + ".pdf";
                    continue;
                }

                dataToSend.ContributorId = parseCAccount.Item2;
                dataToSend.Valid = parseCAccount.Item1;


                destinationPath = ConfigurationManager.AppSettings["DestinationPath"] + "\\" + dataToSend.ContributorId + "_" + System.DateTime.Today.ToString("ddMMyyyy") + ".pdf";
                destinationPathAbsoluto = ConfigurationManager.AppSettings["DestinationPathBBDD"] + "\\" + dataToSend.ContributorId + "_" + System.DateTime.Today.ToString("ddMMyyyy")  + ".pdf";
                dataToSend.PathAbsoluto = destinationPathAbsoluto;

                var cnaeParsingResult = ContributorPersonalData.ParseCNAE(data.Text);
                dataToSend.CNAE = cnaeParsingResult.Item2;
                dataToSend.Valid &= cnaeParsingResult.Item1;

                var socialReasonParsingResult = ContributorPersonalData.ParseSocialReason(data.Text);
                dataToSend.SocialReason = socialReasonParsingResult.Item2;
                dataToSend.Valid &= socialReasonParsingResult.Item1;

                var nifParsingResult = ContributorPersonalData.ParseNIF(data.Text);
                dataToSend.NIF = nifParsingResult.Item2;
                dataToSend.Valid &= nifParsingResult.Item1;


                List<ContributionPeriod> results;
                results = parser.Parse(data.Text);
                if ( results.Any(s => s.ValidPeriod == false))
                {
                    dataToSend.Valid = false;
                }
                results.RemoveAll(s => s.ValidPeriod == false);

                foreach (ContributionPeriod period in results)
                {
                    dataToSend.ContributionPeriodsDTO.Add(new ContributionPeriodDTO()
                    {
                        MoneyContribution = period.MoneyContribution,
                        PeriodEnd = period.PeriodEnd,
                        PeriodStart = period.PeriodStart,
                        HighResFileId = fileUrl,
                        FileAbsolutePath = destinationPathAbsoluto,
                        Valid = period.ValidPeriod
                    });
                }

                APIClient client = new APIClient(_apiUrl);
                client.Post("ContributionPeriods", dataToSend);
            }

            _logger.Log("moviendo archivo a " + destinationPath);

            File.Move(inputPath, destinationPath);

            _logger.Log("FIN PROCESO");
            _logger.Log("-----------");
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
                if (a.Response != null)
                {
                    _logger.Log(a.Response.ToString());
                }

                if (a.Message != null)
                {
                    _logger.Log(a.Message.ToString());
                }

                if (a.InnerException != null)
                {
                    _logger.Log(a.InnerException.ToString());
                }
                
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
