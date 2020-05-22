Feature: ProfileLanguage
	In order for buyers to view my languages capabilities
	As a Skill Trader
	I want to be able to add, update, and delete my language capabilities on my profile

@mytag
Scenario: Adding more than 4 languages is not allowed
	Given I already have 3 'Language(s)' as follows:
		| Name    | Level  |
		| Spanish | Fluent |
		| French  | Fluent |
		| German  | Fluent |
	And I am on "Language" tab
	When I save another 'Language' as follows:
		| Name    | Level  |
		| English | Fluent |
	Then my profile page displays the newly added 'Language'
	And the success popup message "English has been added to your languages" is displayed
	But I can no longer add another 'Language'