Feature: CreateAppointment
in order to create an assignment
i want to make sure that the assignment date
is not longer than a year in the future
and that my assignment date is not set in the past

@tag1
Scenario: Create appointment succesfully
	Given the appointment date is <assignmentDate_AddDays>
	When we call CreateAppointment
	Then the outcome should be <result>
	  

	Examples: 
	| assignmentDate_AddDays | result |
	| 1                      | true   |
	| 10                     | true   |
	| 100                    | true   |

Scenario: Create appointment unsuccesfully
	Given the appointment date is <appointmentDateFail_AddDays>
	When we call CreateAppointment
	Then the outcomse should throw an <error>

	Examples: 
	| appointmentDateFail_AddDays | error                                               |
	| -1                      | Tidspunkt for behandling kan ikke være i fortiden |
	| 366                     | Du kan højest booke din tid et år frem            |
	| 500                     | Du kan højest booke din tid et år frem            |