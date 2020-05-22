Feature: ChangePassword
	As a user
	I want to be able to change my password

Scenario: Changing password to previous password is not allowed
	Given I navigate to "Change Password" page
	When I reuse my current password
	Then password is not saved
	And the error popup message "Current Password and New Password should not be same" is displayed
