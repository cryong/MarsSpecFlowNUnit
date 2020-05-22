Feature: ProfileEducation
	In order for buyers to view my education background
	As a Skill Trader
	I want to be able to add, update and delete my education background on my profile

@mytag
Scenario: Adding education with valid data
	Given I am on "Education" tab
	When I save 'Education' as follows:
	| InstituteName          | Country     | DegreeTitle | DegreeNAme          | YearOfGraduation |
	| University of Auckland | New Zealand | B.Sc        | Bachelor of Science | 2020             |
	Then my profile page displays the newly added 'Education'
	And the success popup message "Education has been added" is displayed
