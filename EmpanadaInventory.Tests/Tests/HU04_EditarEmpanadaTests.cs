using NUnit.Framework;
using OpenQA.Selenium;

namespace EmpanadaInventory.Tests
{
    [TestFixture]
    public class HU04_EditarEmpanadaTests : BaseTest
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
        public void Editar_CaminoFeliz_PrecioActualizado()
        {
            Test = Extent.CreateTest("HU-04 | Camino Feliz - Editar precio");
            IrAlInventario();

            EsperarClickeable(By.CssSelector("a.btn-editar")).Click();
            Esperar(By.Id("Precio"));
            AgregarCaptura("HU04_Feliz_01_FormularioEdicion");

            var campoPrecio = Driver.FindElement(By.Id("Precio"));
            Assert.That(campoPrecio.GetAttribute("value"), Is.Not.Empty,
                "El formulario debe precargarse con el precio actual");

            campoPrecio.Clear();
            campoPrecio.SendKeys("99.00");
            AgregarCaptura("HU04_Feliz_02_PrecioNuevo");

            EsperarClickeable(By.CssSelector("button[type='submit']")).Click();

            EsperarUrl("/Empanada");
            AgregarCaptura("HU04_Feliz_03_Resultado");

            Assert.That(Driver.Url, Does.Contain("/Empanada"),
                "Debe redirigir al inventario tras editar");
            Assert.That(Driver.PageSource, Does.Contain("99,00").Or.Contain("99.00"),
                "El nuevo precio debe reflejarse en la tabla");

            Test.Pass("Precio actualizado y visible en la tabla.");
        }

        [Test]
        public void Editar_Negativo_NombreVacioBloquea()
        {
            Test = Extent.CreateTest("HU-04 | Negativo - Nombre vacío bloquea guardado");
            IrAlInventario();

            EsperarClickeable(By.CssSelector("a.btn-editar")).Click();
            Esperar(By.Id("Sabor"));

            var campoSabor = Driver.FindElement(By.Id("Sabor"));
            campoSabor.Clear();
            AgregarCaptura("HU04_Neg_01_NombreVacio");

            EsperarClickeable(By.CssSelector("button[type='submit']")).Click();
            AgregarCaptura("HU04_Neg_02_Resultado");

            Assert.That(Driver.Url, Does.Contain("/Empanada/Edit"),
                "No debe guardar si el nombre está vacío");

            Test.Pass("Edición bloqueada correctamente con nombre vacío.");
        }

        [Test]
        public void Editar_Limites_FormularioPrecargado()
        {
            Test = Extent.CreateTest("HU-04 | Límites - Formulario precargado");
            IrAlInventario();

            EsperarClickeable(By.CssSelector("a.btn-editar")).Click();
            Esperar(By.Id("Sabor"));
            AgregarCaptura("HU04_Lim_01_Formulario");

            var sabor = Driver.FindElement(By.Id("Sabor")).GetAttribute("value");
            var precio = Driver.FindElement(By.Id("Precio")).GetAttribute("value");
            var cantidad = Driver.FindElement(By.Id("CantidadInventario")).GetAttribute("value");

            Assert.That(sabor, Is.Not.Empty, "Sabor debe estar precargado");
            Assert.That(precio, Is.Not.Empty, "Precio debe estar precargado");
            Assert.That(cantidad, Is.Not.Empty, "Cantidad debe estar precargada");

            Test.Pass("Formulario correctamente precargado con datos existentes.");
        }
    }
}