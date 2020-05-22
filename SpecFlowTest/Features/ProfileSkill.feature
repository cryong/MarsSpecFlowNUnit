Feature: ProfileSkill
	In order for buyers to view my skill sets
	As a Skill Trader
	I want to be able to add, update, and delete skill sets on my profile

@mytag
Scenario: Adding skill with valid data
	Given I am on "Skill" tab
	When I save 'Skill' as follows:
	| Name     | Level  |
	| Selenium | Expert |
	Then my profile page displays the newly added 'Skill'
	And the success popup message "Selenium has been added to your skills" is displayed
