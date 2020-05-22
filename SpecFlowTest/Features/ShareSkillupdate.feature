Feature: ShareSkillUpdate
	In order for buyers to to view the updated information of my listings
	As a Skill Trader
	I want to be able to update skills that I have shared

@mytag
Scenario: Update listed skill information
	Given I already offer "Cypress" for trade
	And I am on Listing Management page
	When I update my "Cypress" listing as follows:
	| ServiceType          | LocationType | SkillTrade | Credit | Active |
	| Hourly basis service | Online       | Credit     | 10     | Active |
	Then "Cypress" is updated successfully 
