Feature: ProfileCertifications
	In order for buyers to view my certifcations
	As a Skill Trader
	I want to be able to add, update, and delete certifications on my profile

@mytag
Scenario: Adding certification with valid data
	Given I am on "Certification" tab
	When I save 'Certification' as follows:
		| Name             | Organisation | Year |
		| Foundation Level | ISTQB        | 2020 |
	Then my profile page displays the newly added 'Certification'
	And the success popup message "Foundation Level has been added to your certification" is displayed