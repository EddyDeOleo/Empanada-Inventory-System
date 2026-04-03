using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace EmpanadaInventory.Tests
{
    [SetUpFixture]
    public class ReporteManager
    {
        private static readonly string ReportPath = Path.Combine(
    Directory.GetCurrentDirectory(), "..", "..", "..",
    "Evidencias", "Reportes", "Reporte.html");

        private static ExtentReports? _extent;
        private static readonly object _lock = new object();

        public static ExtentReports Extent
        {
            get
            {
                lock (_lock)
                {
                    if (_extent == null)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(ReportPath)!);
                        var reporter = new ExtentSparkReporter(ReportPath);
                        reporter.Config.DocumentTitle = "Reporte QA - Empanada Inventory";
                        reporter.Config.ReportName = "Pruebas Automatizadas";
                        _extent = new ExtentReports();
                        _extent.AttachReporter(reporter);
                    }
                    return _extent;
                }
            }
        }

        [OneTimeTearDown]
        public void CerrarReporteGlobal()
        {
            _extent?.Flush();
            Console.WriteLine("Reporte global generado correctamente.");
        }
    }
}