using NUnit.Framework;
using OpenQA.Selenium;

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
            EsperarUrl("/Empanada");
            Esperar(By.CssSelector("table tbody"));
        }

        [Test]
        public void Eliminar_CaminoFeliz_RegistroDesaparece()
        {
            Test = Extent.CreateTest("HU-05 | Camino Feliz - Eliminar registro");
            IrAlInventario();

            var filasAntes = Driver.FindElements(
                By.CssSelector("table tbody tr")).Count;
            AgregarCaptura("HU05_Feliz_01_Antes");

            EsperarClickeable(By.CssSelector("a.btn-eliminar")).Click();

            // Acepta el confirm() del navegador
            Driver.SwitchTo().Alert().Accept();

            Esperar(By.CssSelector("table tbody"));
            AgregarCaptura("HU05_Feliz_02_Despues");

            var filasDespues = Driver.FindElements(
                By.CssSelector("table tbody tr")).Count;

            Assert.That(filasDespues, Is.LessThan(filasAntes),
                "La tabla debe tener un registro menos tras eliminar");

            Test.Pass($"Registro eliminado. Antes: {filasAntes}, Después: {filasDespues}.");
        }

        [Test]
        public void Eliminar_Negativo_CancelarMantiene()
        {
            Test = Extent.CreateTest("HU-05 | Negativo - Cancelar mantiene registro");
            IrAlInventario();

            var filasAntes = Driver.FindElements(
                By.CssSelector("table tbody tr")).Count;
            AgregarCaptura("HU05_Neg_01_Antes");

            EsperarClickeable(By.CssSelector("a.btn-eliminar")).Click();

            // Cancela el confirm() del navegador
            Driver.SwitchTo().Alert().Dismiss();

            Esperar(By.CssSelector("table tbody"));
            AgregarCaptura("HU05_Neg_02_Cancelado");

            var filasDespues = Driver.FindElements(
                By.CssSelector("table tbody tr")).Count;

            Assert.That(filasDespues, Is.EqualTo(filasAntes),
                "La tabla debe mantener el mismo número de registros al cancelar");

            Test.Pass("Cancelación correcta. Registro intacto.");
        }

        [Test]
        public void Eliminar_Limites_VerificaAntesDespues()
        {
            Test = Extent.CreateTest("HU-05 | Límites - Captura Antes y Después");
            IrAlInventario();

            var filasAntes = Driver.FindElements(
                By.CssSelector("table tbody tr")).Count;
            Test.Info($"Registros antes: {filasAntes}");
            AgregarCaptura("HU05_Lim_01_Antes");

            EsperarClickeable(By.CssSelector("a.btn-eliminar")).Click();
            Driver.SwitchTo().Alert().Accept();

            Esperar(By.CssSelector("table tbody"));
            var filasDespues = Driver.FindElements(
                By.CssSelector("table tbody tr")).Count;
            Test.Info($"Registros después: {filasDespues}");
            AgregarCaptura("HU05_Lim_02_Despues");

            Assert.That(filasDespues, Is.EqualTo(filasAntes - 1),
                "Exactamente un registro debe haber sido eliminado");

            Test.Pass("Evidencia Antes/Después capturada correctamente.");
        }
    }
}