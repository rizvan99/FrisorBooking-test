Feature: CreateAssignment
in order to create an assignment
i want to make sure that the assignment date
is not longer than a year in the future
and that my assignment date is not set in the past

@tag1
Scenario: Create assignment
	Given the assignment date is <assignmentDate_AddDays>
	When the assignment date exceeds one year or is earlier than today
	Then the outcome should be <result>
	  

	Examples: 
	| assignmentDate_AddDays | result |
	| 1                      | true	  |
	| -1                     | false  |
	| 10                     | true	  |
	| 366                    | false  |
	| 500                    | false  |
	| 100                    | true	  |

