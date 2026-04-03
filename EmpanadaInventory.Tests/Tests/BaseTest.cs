using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;

namespace EmpanadaInventory.Tests
{
    public class BaseTest
    {
        protected IWebDriver Driver = null!;
        protected ExtentReports Extent = null!;
        protected ExtentTest Test = null!;
        protected string BaseUrl = "http://localhost:5000";
        private static string ReportPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Reportes", "Reporte.html");

        [OneTimeSetUp]
        public void IniciarReporte()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ReportPath)!);
            var reporter = new ExtentSparkReporter(ReportPath);
            reporter.Config.DocumentTitle = "Reporte QA - Empanada Inventory";
            reporter.Config.ReportName = "Pruebas Automatizadas";
            Extent = new ExtentReports();
            Extent.AttachReporter(reporter);
        }

        [SetUp]
        public void IniciarDriver()
        {
            var options = new EdgeOptions();
            Driver = new EdgeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void CerrarDriver()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }

        [OneTimeTearDown]
        public void CerrarReporte()
        {
            Extent.Flush();
            Console.WriteLine($"Reporte generado en: {ReportPath}");
        }

        // Espera explícita: espera hasta que un elemento sea visible
        protected IWebElement Esperar(By locator, int segundos = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(segundos));
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        // Espera explícita: espera hasta que un elemento sea clickeable
        protected IWebElement EsperarClickeable(By locator, int segundos = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(segundos));
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        // Espera explícita: espera hasta que la URL contenga un texto
        protected void EsperarUrl(string fragmento, int segundos = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(segundos));
            wait.Until(ExpectedConditions.UrlContains(fragmento));
        }

        protected string CapturarPantalla(string nombrePrueba)
        {
            string carpeta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Capturas");
            Directory.CreateDirectory(carpeta);

            string archivo = Path.Combine(carpeta,
                $"{nombrePrueba}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            screenshot.SaveAsFile(archivo);
            return archivo;
        }

        protected void AgregarCaptura(string nombrePrueba)
        {
            string ruta = CapturarPantalla(nombrePrueba);
            Test.AddScreenCaptureFromPath(ruta);
        }
    }
}