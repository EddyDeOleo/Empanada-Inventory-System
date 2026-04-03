using NUnit.Framework;
using OpenQA.Selenium;

namespace EmpanadaInventory.Tests
{
    [TestFixture]
    public class HU03_LeerInventarioTests : BaseTest
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
        public void Leer_CaminoFeliz_TablaConRegistros()
        {
            Test = Extent.CreateTest("HU-03 | Camino Feliz - Tabla carga registros");
            IrAlInventario();
            AgregarCaptura("HU03_Feliz_01_Tabla");

            var filas = Driver.FindElements(By.CssSelector("table tbody tr"));
            Assert.That(filas.Count, Is.GreaterThan(0),
                "Debe haber al menos un registro en la tabla");

            Test.Pass($"Tabla cargada con {filas.Count} registros.");
        }

        [Test]
        public void Leer_Negativo_ColumnasVisibles()
        {
            Test = Extent.CreateTest("HU-03 | Negativo - Columnas Sabor y Precio visibles");
            IrAlInventario();

            var columnas = Driver.FindElements(By.CssSelector("table thead th"));
            var textos = columnas.Select(c => c.Text.ToLower()).ToList();

            Assert.That(textos, Does.Contain("sabor"),
                "La columna Sabor debe estar visible");
            Assert.That(textos, Does.Contain("precio"),
                "La columna Precio debe estar visible");

            AgregarCaptura("HU03_Neg_01_Columnas");
            Test.Pass("Columnas Sabor y Precio visibles correctamente.");
        }

        [Test]
        public void Leer_Limites_ContenidoCeldasNoVacio()
        {
            Test = Extent.CreateTest("HU-03 | Límites - Celdas no vacías");
            IrAlInventario();

            var celdas = Driver.FindElements(
                By.CssSelector("table tbody tr td:first-child"));

            foreach (var celda in celdas)
            {
                Assert.That(celda.Text.Trim(), Is.Not.Empty,
                    "Ninguna celda de Sabor debe estar vacía");
            }

            AgregarCaptura("HU03_Lim_01_Celdas");
            Test.Pass("Todas las celdas de Sabor tienen contenido.");
        }
    }
}