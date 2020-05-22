using System;
using System.Collections.Generic;
using System.Linq;
using MarsFramework.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using MarsWebService.Model;

namespace MarsWebService.Pages.Sections.Profile.Sections
{
    public class MainSection
    {
        private readonly Driver _driver;
        public MainSection(Driver driver)
        {
            _driver = driver;
        }

        private By TableDataRows => By.XPath("//div[contains(@class, 'active')]/div[@class='row']/div[contains(@class, 'scrollTable')]/div/table/tbody/tr");
        private By LanguagesTabButton => By.XPath("//form/div[contains(@class, 'menu')]/a[.='Languages']");
        private By SkillsTabButton => By.XPath("//form/div[contains(@class, 'menu')]/a[.='Skills']");
        private By EducationTabButton => By.XPath("//form/div[contains(@class, 'menu')]/a[.='Education']");
        private By CertificationTabButton => By.XPath("//form/div[contains(@class, 'menu')]/a[.='Certifications']");
        private By AddNewButton => By.XPath("//div[contains(@class, 'active')]/div/div/div/table/thead/tr/th/div[. = 'Add New']");
        private By SaveButton => By.XPath("//form/div[contains(@class, 'active')]/div/div/div[@class='form-wrapper']/div/descendant::*/input[@type='button' and @value='Add']");
        private LanguageTab LanguageSection => new LanguageTab();
        private SkillTab SkillSection => new SkillTab();
        private CertificationTab CertificationSection => new CertificationTab();
        private EducationTab EducationSection => new EducationTab();
        private class LanguageTab
        {
            public By Name => By.XPath("//form/div[contains(@class, 'active')]/div/div/div[@class='form-wrapper']/div/div/input[@name='name']");
            public By Level => By.XPath("//form/div[contains(@class, 'active')]/div/div/div[@class='form-wrapper']/div/div/select[@name='level']");
        }

        private class SkillTab
        {
            public By Name => By.XPath("//div[contains(@class,'active')]/div/div/div/div[@class='fields']/div/input[@name='name']");
            public By Level => By.XPath("//div[contains(@class,'active')]/div/div/div/div[@class='fields']/div/select[@name='level']");
        }

        private class CertificationTab
        {
            public By Name => By.Name("certificationName");
            public By Organisation => By.Name("certificationFrom");
            public By Year => By.Name("certificationYear");
        }

        private class EducationTab
        {
            public By InstituteName => By.Name("instituteName");
            public By Country => By.Name("country");
            public By DegreeTitle => By.Name("title");
            public By DegreeName => By.Name("degree");
            public By GraudationYear => By.Name("yearOfGraduation");
        }

        public enum ProfileInfoType
        {
            Language,
            Skill,
            Education,
            Certification
        }

        public void ClickTab(ProfileInfoType type)
        {
            switch (type)
            {
                case ProfileInfoType.Language:
                    _driver.Click(LanguagesTabButton);
                    return;
                case ProfileInfoType.Skill:
                    _driver.Click(SkillsTabButton);
                    return;
                case ProfileInfoType.Education:
                    _driver.Click(EducationTabButton);
                    return;
                case ProfileInfoType.Certification:
                    _driver.Click(CertificationTabButton);
                    return;
                default:
                    throw new ArgumentException($"Illegal argument was passed '{type:G}'");
            }
        }
        public void SaveProfileDetail(SearchableItem item)
        {
            if (item is Language language)
            {
                EnterLanguageDetails(language);
            }
            else if (item is Skill skill)
            {
                EnterSkillDetails(skill);
            }
            else if (item is Education education)
            {
                EnterEducationDetails(education);
            }
            else if (item is Certification certification)
            {
                EnterCertificationDetails(certification);
            }
            else
            {
                throw new ArgumentException($"Unknown item type : {item.GetType()} with its values : {item}");
            }
        }
        public void SaveProfileDetails(IEnumerable<SearchableItem> items)
        {
            foreach (var item in items)
            {
                SaveProfileDetail(item);
            }
        }

        public void EnterLanguageDetails(Language language)
        {
            ClickTab(ProfileInfoType.Language);
            _driver.Click(AddNewButton);
            _driver.FindElement(LanguageSection.Name).SendKeys(language.Name);
            _driver.SelectOption(SelectDropDownOption.VALUE, language.Level, LanguageSection.Level);
            // handle exception in the test if such option does not exist
            _driver.Click(SaveButton);
        }
        public void EnterEducationDetails(Education education)
        {
            ClickTab(ProfileInfoType.Education);
            _driver.Click(AddNewButton);
            _driver.FindElement(EducationSection.InstituteName).SendKeys(education.InstituteName);
            _driver.SelectOption(SelectDropDownOption.VALUE, education.Country, EducationSection.Country);
            _driver.SelectOption(SelectDropDownOption.VALUE, education.DegreeTitle, EducationSection.DegreeTitle);
            _driver.FindElement(EducationSection.DegreeName).SendKeys(education.DegreeName);
            _driver.SelectOption(SelectDropDownOption.VALUE, education.YearOfGraduation, EducationSection.GraudationYear);
            // handle exception in the test if such option does not exist
            _driver.Click(SaveButton);
        }
        public void EnterSkillDetails(Skill skill)
        {
            ClickTab(ProfileInfoType.Skill);
            _driver.Click(AddNewButton);
            _driver.FindElement(SkillSection.Name).SendKeys(skill.Name);
            _driver.SelectOption(SelectDropDownOption.VALUE, skill.Level, SkillSection.Level);
            _driver.Click(SaveButton);
        }
        public void EnterCertificationDetails(Certification certification)
        {
            ClickTab(ProfileInfoType.Certification);
            _driver.Click(AddNewButton);
            _driver.FindElement(CertificationSection.Name).SendKeys(certification.Name);
            _driver.FindElement(CertificationSection.Organisation).SendKeys(certification.Organisation);
            _driver.SelectOption(SelectDropDownOption.VALUE, certification.Year, CertificationSection.Year);
            _driver.Click(SaveButton);
        }
        public bool IsAddRowButtonEnbaled()
        {
            try
            {
                if (_driver.FindElement(SaveButton).Enabled)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SearchForRow<T>(T row) where T : SearchableItem
        {
            Type rowType = row.GetType();
            Enum.TryParse(rowType.Name, out ProfileInfoType detailType);
            ClickTab(detailType);
            var rows = _driver.FindElements(TableDataRows);
            if (rows != null)
            {
                foreach (var rowCandidate in _driver.FindElements(TableDataRows))
                {
                    // get all data cells except last one for buttons
                    // order of the cells and constructor parameters are VERY CRUCIAL for this to work
                    var cellData = rowCandidate.FindElements(By.XPath("td[position() <last()]"));
                    var rowToCompare = rowType.GetConstructor(cellData.Select(i => typeof(string)).ToArray()).Invoke(cellData.Select(j => j.Text).ToArray());
                    if (rowToCompare.Equals(row))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
