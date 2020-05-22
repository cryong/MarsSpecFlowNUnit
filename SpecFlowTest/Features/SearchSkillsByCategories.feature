Feature: SearchSkillsByCategories
	In order to find specific services 
	As a user
	I want to be able to use categories and subcateogries to filter search results

@mytag
Scenario: Search skills using a specific category
	Given the skill search results for "SpecFlow" are shown
	When I filter the results by category "Programming & Tech"
	Then only the matching skills are displayed