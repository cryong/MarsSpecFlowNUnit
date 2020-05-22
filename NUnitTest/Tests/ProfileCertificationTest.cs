using System;
using System.Collections.Generic;
using MarsCommonFramework.DataSetup;
using MarsFramework.Factory;
using MarsFramework.Utilities;
using MarsWebService.Model;
using MarsWebService.Pages.Profile;
using NUnit.Framework;
using NUnitTest.SetUp;

namespace NUnitTest.Tests
{
    [TestFixture]
    public class ProfileCertificationTest : LoginSetUp
    {

        [Test]
        [TestCaseSource(typeof(ProfileCertificationTest), nameof(MyData))]
        public void When_ValidCertficationDetails_Expect_AdddSuccessful(Certification certification)
        {
            try
            {
                DataSetUpHelper helper = new DataSetUpHelper(ValidCredentials.Username, ValidCredentials.Password);

                // act
                ProfilePage profilePage = new ProfilePage(Driver); // reload
                profilePage.Open();
                profilePage.MainSection.EnterCertificationDetails(certification);
                Driver.WaitForAjax();
                certification.Id = helper.GetOrAdd(certification);
                _setUpContext.Add(certification);

                // assert
                Driver.WaitForAjax();
                Assert.IsTrue(profilePage.MainSection.SearchForRow(certification));
                Assert.That(
                    $"{certification.Name} has been added to your certification",
                    Is.EqualTo(profilePage.GetSuccessPopUpMessage()));
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                {
                    throw;
                }
                Assert.Fail($"Error has occurred\nMessage : {e.Message}\nStackTrace : {e.StackTrace}");
            }
        }

        private static Certification ReadFromExcel(string key)
        {
            ExcelDataReaderUtil.LoadWorsheet(PathUtil.GetCurrentPath($"{TestContext.Parameters["TestDataPath"]}Mars.xlsx"), "ProfileCertification");
            ExcelData data = ExcelDataReaderUtil.FetchRowUsingKey(key);
            return ObjectFactory.CreateInstance<Certification>(data);
        }
        public static IEnumerable<TestCaseData> MyData()
        {
            yield return new TestCaseData(ReadFromExcel("1"));
            yield return new TestCaseData(ReadFromExcel("2"));
        }
    }
}
