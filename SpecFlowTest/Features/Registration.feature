Feature: Registration
	In order to login to the application
	As a new user 
	I want to be able to create an account

@NoLogin
Scenario: User registration with already registered email address
	Given I am on the application homepage
	When I register with a registered email address
	Then registration is not successful
	And the failure message "This email has already been used to register an account." is displayed

