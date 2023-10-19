using OfficeOpenXml;

namespace Localidades.Application.Configurations;

public static class Settings
{
    public static string Secret { get; set; }
    public static string ConnectionString { get; set; }

    public static void ConfigureEPPlusLicense()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public static void SetConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public static void SetSecret(string secret)
    {
        Secret = secret;
    }
}

