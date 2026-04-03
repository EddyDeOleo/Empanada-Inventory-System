using NUnit.Framework;
using OpenQA.Selenium;

namespace EmpanadaInventory.Tests
{
    [TestFixture]
    public class HU02_CrearEmpanadaTests : BaseTest
    {
        private void IrAlLogin()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Auth/Login");
            EsperarClickeable(By.Id("usuario")).SendKeys("admin");
            EsperarClickeable(By.Id("password")).SendKeys("admin123");
            EsperarClickeable(By.Id("btnEntrar")).Click();
            EsperarUrl("/Empanada");
        }

        [Test]
        public void Crear_CaminoFeliz_RegistroExitoso()
        {
            Test = Extent.CreateTest("HU-02 | Camino Feliz - Crear empanada");
            IrAlLogin();

            Driver.Navigate().GoToUrl($"{BaseUrl}/Empanada/Create");
            Esperar(By.Id("Sabor"));
            AgregarCaptura("HU02_Feliz_01_FormularioVacio");

            EsperarClickeable(By.Id("Sabor")).SendKeys("Carne Molida");
            Driver.FindElement(By.Id("Precio")).SendKeys("65.00");
            Driver.FindElement(By.Id("CantidadInventario")).SendKeys("25");
            AgregarCaptura("HU02_Feliz_02_FormularioLleno");

            EsperarClickeable(By.CssSelector("button[type='submit']")).Click();

            EsperarUrl("/Empanada");
            AgregarCaptura("HU02_Feliz_03_Resultado");

            Assert.That(Driver.Url, Does.Contain("/Empanada"),
                "Debe redirigir al inventario tras crear");
            Assert.That(Driver.PageSource, Does.Contain("Carne Molida"),
                "El nuevo registro debe aparecer en la tabla");

            Test.Pass("Empanada creada y visible en la tabla.");
        }

        [Test]
        public void Crear_Negativo_PrecioConLetras()
        {
            Test = Extent.CreateTest("HU-02 | Negativo - Precio con letras");
            IrAlLogin();

            Driver.Navigate().GoToUrl($"{BaseUrl}/Empanada/Create");
            Esperar(By.Id("Sabor"));

            EsperarClickeable(By.Id("Sabor")).SendKeys("Test");
            Driver.FindElement(By.Id("Precio")).SendKeys("abc");
            Driver.FindElement(By.Id("CantidadInventario")).SendKeys("5");
            AgregarCaptura("HU02_Neg_01_PrecioInvalido");

            var errorPrecio = Esperar(By.Id("error-precio"));
            Assert.That(errorPrecio.Displayed, Is.True,
                "Debe mostrarse error de precio inválido");

            Test.Pass("Validación de precio con letras funciona correctamente.");
        }

        [Test]
        public void Crear_Limites_PrecioNegativo()
        {
            Test = Extent.CreateTest("HU-02 | Límites - Precio negativo");
            IrAlLogin();

            Driver.Navigate().GoToUrl($"{BaseUrl}/Empanada/Create");
            Esperar(By.Id("Sabor"));

            EsperarClickeable(By.Id("Sabor")).SendKeys("Test Negativo");
            Driver.FindElement(By.Id("Precio")).SendKeys("-10");
            Driver.FindElement(By.Id("CantidadInventario")).SendKeys("5");
            AgregarCaptura("HU02_Lim_01_PrecioNegativo");

            var boton = Driver.FindElement(By.CssSelector("button[type='submit']"));
            Assert.That(boton.Enabled, Is.False,
                "El botón debe estar deshabilitado con precio negativo");

            Test.Pass("Precio negativo bloqueado correctamente.");
        }
    }
}