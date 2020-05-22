using System.Collections.Generic;
using NUnit.Framework;
using MarsWebService.Model;
using MarsFramework.Utilities;
using MarsFramework.Factory;
using MarsWebService.Pages.Home;
using MarsWebService.Pages.Profile;
using NUnit.Framework.Internal;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    /// <summary>
    /// Summary description for SignInTest
    /// </summary>
    [TestFixture]
    [Category("SignIn")]
    public class SignInTest : BaseSetUp
    {
        // https://stackoverflow.com/questions/58552843/is-there-a-way-a-test-can-have-its-testcasesource-read-data-from-outside-source
        // http://bgrva.github.io/blog/2015/05/14/nunit-theories-for-integration-tests/
        // https://stackoverflow.com/questions/4220943/how-to-pass-dynamic-objects-into-an-nunit-testcase-function
        // https://stackoverflow.com/questions/40825197/nunit-theory-replaces-test
        // http://bgrva.github.io/blog/2015/05/14/nunit-theories-for-integration-tests/
        // https://haacked.com/archive/2012/01/02/structuring-unit-tests.aspx/
        // https://stackoverflow.com/questions/46727966/why-onetimesetup-in-setupfixture-called-after-test-testcasesource
        // https://stackoverflow.com/questions/44030447/why-nunit3-onetimesetup-is-called-after-test-and-not-before?rq=1
        // https://stackoverflow.com/questions/49683601/how-can-i-implement-static-test-local-data-in-an-nunit-test for custom context per test

        [Test]
        [TestCaseSource(typeof(SignInTest), nameof(MyData))]
        public void When_InvalidCredentials_Expect_LoginFailure(Credentials credentials)
        {
            SignInPage signInPage = new HomePage(Driver).OpenLoginForm();
            signInPage.EnterCredentials(credentials.Username, credentials.Password);
            signInPage.ClickLogInButton();
            Driver.WaitForAjax();
            // exepct
            Assert.That(signInPage.IsOpen(), Is.True);
        }

        [Test]
        public void When_ValidCredentials_Expect_LoginSuccess()
            // REMOVE LATER NO NEED TO HAVE A SEPARATE TEST WAS JUST ADDED TO TEST STATIC CREDENTIALS LOADED AS PART OF ONTIMETSETUP
        {
            SignInPage signInPage = new HomePage(Driver).OpenLoginForm();
            signInPage.EnterCredentials(ValidCredentials.Username, ValidCredentials.Password);
            ProfilePage profilePage = signInPage.ClickLogInButton();
            Driver.WaitForAjax();
            // exepct
            Assert.That(profilePage.Url, Is.EqualTo(Driver.GetCurrentUrl()));
        }

        private static Credentials ReadFromExcel(string key)
        {
            // Testcase source gets evaluated FIRST
            // at this point, FrameContext has not been initialised
            // maybe consider using .runsettings? for Nunit?
            // have to load for every case...
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "Login");
            var data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<Credentials>(data);
        }
        public static IEnumerable<TestCaseData> MyData()
        {
            yield return new TestCaseData(ReadFromExcel("InvalidUsername"));
            yield return new TestCaseData(ReadFromExcel("InvalidPassword"));
            //yield return new TestCaseData("Fox").SetProperty(PropertyA, "ValueA").SetProperty(PropertyA, "ValueB");
            //yield return new TestCaseData("Rabbit").SetProperty(PropertyA, "ValueA").SetProperty(PropertyA, "ValueB");
            //yield return new TestCaseData("Hound").SetProperty(PropertyA, "ValueA").SetProperty(PropertyA, "ValueB");
        }

    }
}
