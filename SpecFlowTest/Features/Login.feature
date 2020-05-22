Feature: Login
	In order to access the application
	As a user 
	I want to be able to login to the application

@NoLogin
Scenario: Login to website
	Given I am on the application homepage
	And I am a registered user
	When I login
	Then my profile page is displayed