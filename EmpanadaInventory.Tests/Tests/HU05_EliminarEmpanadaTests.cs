using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EmpanadaInventory.Tests
{
    [TestFixture]
    public class HU05_EliminarEmpanadaTests : BaseTest
    {
        private void IrAlInventario()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Auth/Login");
            EsperarClickeable(By.Id("usuario")).SendKeys("admin");
            EsperarClickeable(By.Id("password")).SendKeys("admin123");
            EsperarClickeable(By.Id("btnEntrar")).Click();
            // Vuelve a usar Contains que sí funciona para el login
            EsperarUrl("/Empanada");
            Esperar(By.CssSelector("table"));
        }

        private void EsperarUrlExacta(string urlFinal, int segundos = 15)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(segundos));
            wait.Until(driver =>
                driver.Url.Contains(urlFinal) &&
                !driver.Url.Contains("/Delete") &&
                !driver.Url.Contains("/Edit") &&
                !driver.Url.Contains("/Create") &&
                !driver.Url.Contains("/DeleteConfirmed"));
        }

        private void CrearRegistroTemporal(string sabor)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Empanada/Create");
            Esperar(By.Id("Sabor"));
            EsperarClickeable(By.Id("Sabor")).SendKeys(sabor);
            Driver.FindElement(By.Id("Precio")).SendKeys("10.00");
            Driver.FindElement(By.Id("CantidadInventario")).SendKeys("1");
            EsperarClickeable(By.CssSelector("button[type='submit']")).Click();
            EsperarUrl("/Empanada");
            Esperar(By.CssSelector("table"));
        }

        private int ContarFilas()
        {
            var filas = Driver.FindElements(By.CssSelector("table tbody tr"));
            if (filas.Count == 1 &&
                filas[0].FindElements(By.CssSelector(".empty-state")).Count > 0)
                return 0;
            return filas.Count;
        }

        [Test]
        public void Eliminar_CaminoFeliz_RegistroDesaparece()
        {
            Test = Extent.CreateTest("HU-05 | Camino Feliz - Eliminar registro");
            IrAlInventario();
            CrearRegistroTemporal("Empanada Test Feliz");

            int filasAntes = ContarFilas();
            Test.Info($"Registros antes: {filasAntes}");
            AgregarCaptura("HU05_Feliz_01_Antes");

            EsperarClickeable(By.CssSelector("button.btn-eliminar")).Click();
            Driver.SwitchTo().Alert().Accept();

            // Espera que regrese exactamente al Index
            EsperarUrlExacta("/Empanada");
            Esperar(By.CssSelector("table"));
            AgregarCaptura("HU05_Feliz_02_Despues");

            int filasDespues = ContarFilas();
            Test.Info($"Registros después: {filasDespues}");

            Assert.That(filasDespues, Is.LessThan(filasAntes),
                "La tabla debe tener un registro menos tras eliminar");

            Test.Pass($"Registro eliminado. Antes: {filasAntes}, Después: {filasDespues}.");
        }

        [Test]
        public void Eliminar_Negativo_CancelarMantiene()
        {
            Test = Extent.CreateTest("HU-05 | Negativo - Cancelar mantiene registro");
            IrAlInventario();
            CrearRegistroTemporal("Empanada Test Negativo");

            int filasAntes = ContarFilas();
            AgregarCaptura("HU05_Neg_01_Antes");

            EsperarClickeable(By.CssSelector("button.btn-eliminar")).Click();
            Driver.SwitchTo().Alert().Dismiss();

            Esperar(By.CssSelector("table"));
            AgregarCaptura("HU05_Neg_02_Cancelado");

            int filasDespues = ContarFilas();

            Assert.That(filasDespues, Is.EqualTo(filasAntes),
                "La tabla debe mantener el mismo número de registros al cancelar");

            Test.Pass("Cancelación correcta. Registro intacto.");
        }

        [Test]
        public void Eliminar_Limites_VerificaAntesDespues()
        {
            Test = Extent.CreateTest("HU-05 | Límites - Captura Antes y Después");
            IrAlInventario();
            CrearRegistroTemporal("Empanada Test Limites");

            int filasAntes = ContarFilas();
            Test.Info($"Registros antes: {filasAntes}");
            AgregarCaptura("HU05_Lim_01_Antes");

            EsperarClickeable(By.CssSelector("button.btn-eliminar")).Click();
            Driver.SwitchTo().Alert().Accept();

            // Espera URL exacta del index, no solo que contenga /Empanada
            EsperarUrlExacta("/Empanada");
            Esperar(By.CssSelector("table"));

            int filasDespues = ContarFilas();
            Test.Info($"Registros después: {filasDespues}");
            AgregarCaptura("HU05_Lim_02_Despues");

            Assert.That(filasDespues, Is.EqualTo(filasAntes - 1),
                "Exactamente un registro debe haber sido eliminado");

            Test.Pass("Evidencia Antes/Después capturada correctamente.");
        }
    }
}