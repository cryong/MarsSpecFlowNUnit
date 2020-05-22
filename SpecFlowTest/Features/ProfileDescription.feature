Feature: ProfileDescription
	In order for buyers to view my profile description
	As a Skill Trader
	I want to be able to update my profile description

@mytag
Scenario: Modifying description with a leading special character
	When I modify description to "@This description should not be allowed" 
	Then description is not saved
	And the error popup message "First character can only be digit or letters" is displayed

