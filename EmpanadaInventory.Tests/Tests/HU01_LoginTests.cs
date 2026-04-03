using NUnit.Framework;
using OpenQA.Selenium;

namespace EmpanadaInventory.Tests
{
    [TestFixture]
    public class HU01_LoginTests : BaseTest
    {
        private void IrAlLogin()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Auth/Login");
            Esperar(By.Id("usuario"));
        }

        [Test]
        public void Login_CaminoFeliz_CredencialesCorrectas()
        {
            Test = Extent.CreateTest("HU-01 | Camino Feliz - Login correcto");

            IrAlLogin();
            Test.Info("Navegando al Login");
            AgregarCaptura("HU01_Feliz_01_Login");

            EsperarClickeable(By.Id("usuario")).SendKeys("admin");
            EsperarClickeable(By.Id("password")).SendKeys("admin123");
            AgregarCaptura("HU01_Feliz_02_Credenciales");

            EsperarClickeable(By.Id("btnEntrar")).Click();

            EsperarUrl("/Empanada");
            AgregarCaptura("HU01_Feliz_03_Resultado");

            Assert.That(Driver.Url, Does.Contain("/Empanada"),
                "Debería redirigir al inventario tras login exitoso");

            Test.Pass("Login exitoso. Redirigido a Inventario.");
        }

        [Test]
        public void Login_Negativo_CredencialesIncorrectas()
        {
            Test = Extent.CreateTest("HU-01 | Negativo - Credenciales incorrectas");

            IrAlLogin();
            Test.Info("Navegando al Login");
            AgregarCaptura("HU01_Neg_01_Login");

            EsperarClickeable(By.Id("usuario")).SendKeys("usuarioFalso");
            EsperarClickeable(By.Id("password")).SendKeys("claveErronea");
            AgregarCaptura("HU01_Neg_02_Credenciales");

            EsperarClickeable(By.Id("btnEntrar")).Click();

            var errorMsg = Esperar(By.Id("error-msg"));
            AgregarCaptura("HU01_Neg_03_Resultado");

            Assert.That(errorMsg.Displayed, Is.True,
                "Debe mostrarse el mensaje de error");
            Assert.That(errorMsg.Text, Does.Contain("Usuario no encontrado"),
                "El mensaje debe indicar que el usuario no fue encontrado");

            Test.Pass("Mensaje de error mostrado correctamente.");
        }

        [Test]
        public void Login_Limites_CamposVacios()
        {
            Test = Extent.CreateTest("HU-01 | Límites - Campos vacíos");

            IrAlLogin();
            Test.Info("Navegando al Login con campos vacíos");
            AgregarCaptura("HU01_Lim_01_Login");

            var boton = Esperar(By.Id("btnEntrar"));
            AgregarCaptura("HU01_Lim_02_Resultado");

            Assert.That(boton.Enabled, Is.False,
                "El botón debe estar deshabilitado con campos vacíos");

            Assert.That(Driver.Url, Does.Contain("/Auth"),
                "No debe avanzar si los campos están vacíos");

            Test.Pass("Botón deshabilitado con campos vacíos correctamente.");
        }
    }
}