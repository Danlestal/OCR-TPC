using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatcherCmd.Configuration
{
    public interface IConfigurationProvider
    {
        void Refresh();
        string GetMeasuresLoaderCronExpression();
        string GetDirectoryPathToWatch();
        string GetConnnectionString();
        string GetFilenamePattern();
        string GetDirectoryPathForMeasures();
        string GetNetProductionSign();
        string GetPlantName();
        string GetFtpUsername();
        string GetFtpAddress();
        string GetMeteologicaId();
        string GetFtpPassword();
        string GetForecastPath();
        string GetFtpRemotePath();
        string GetPlantUF();
        string GetProcessedFilesPath();
        string GetProcessedFileTextToAppend();
        string GetWebApiPlantBaseUri();
        string GetWebApiMeasurePutUri();
        string GetWebApiPlantPowerUri();
        string GetWebApiUri();
        double GetEDPItalyToGnarumOffsetHours();
        string GetMeasureSourceFor1HResolution();
        double GetMeasureValueMultiplier();
        double GetBanziConversionFactor();
        string GetDataVariable();
        string GetHourlyCronExpression();
        char GetHourlyPlantsSeparator();
        string GetHourlyPlantsString();
        string GetHourlyDataVariable();
        string GetResolution();
        string GetProcessMeasuresFilesListPath();
        int GetHourlyNumberOfHoursToProcess();

    }
}
