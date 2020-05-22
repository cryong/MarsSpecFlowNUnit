Feature: ProfileAvailability
	In order for buyers to view my availability information
	As a Skill Trader
	I want to be able to update my availability information on my profile

@mytag
Scenario: Update available hours
	When I update available hours to "Full Time"
	Then available hours is updated with a message "Availability updated"
