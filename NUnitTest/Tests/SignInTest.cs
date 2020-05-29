using System.Collections.Generic;
using NUnit.Framework;
using MarsWebService.Model;
using MarsFramework.Utilities;
using MarsFramework.Factory;
using MarsWebService.Pages.Home;
using NUnit.Framework.Internal;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    [Category("SignIn")]
    public class SignInTest : BaseSetUp
    {
        [TestCaseSource(nameof(TestData))]
        public void When_InvalidCredentials_Expect_LoginFailure(Credentials credentials)
        {
            SignInPage signInPage = new HomePage(Driver).OpenLoginForm();
            signInPage.EnterCredentials(credentials.Username, credentials.Password);
            signInPage.ClickLogInButton();
            Driver.WaitForAjax();
            Assert.That(signInPage.IsOpen(), Is.True);
        }

        private static Credentials ReadFromExcel(string key)
        {
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "Login");
            var data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<Credentials>(data);
        }
        public static IEnumerable<TestCaseData> TestData()
        {
            yield return new TestCaseData(ReadFromExcel("InvalidUsername"));
            yield return new TestCaseData(ReadFromExcel("InvalidPassword"));
        }

    }
}
