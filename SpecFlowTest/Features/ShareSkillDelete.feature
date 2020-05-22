Feature: ShareSkillDelete
	In order to retract services that I offer
	As a Skill Trader
	I want to be able to remove skills that I have shared

@mytag
Scenario: Remove a skill from the listing
	Given I already offer "Cucumber" for trade
	And I am on Listing Management page
	When I delete my "Cucumber" listing
	Then "Cucumber" is no longer found in my managed listings