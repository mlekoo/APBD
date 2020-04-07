ALTER PROCEDURE PromoteStudent @Studies VARCHAR(25), @Semester INT
AS
BEGIN
    IF (SELECT COUNT(IdEnrollment) FROM Enrollment e JOIN Studies s ON e.IdStudy = s.IdStudy WHERE s.Name = @Studies AND e.Semester = @Semester + 1) = 0
        BEGIN
            INSERT INTO Enrollment
            VALUES((SELECT MAX(IdEnrollment)+1 FROM Enrollment), @Semester+1, (SELECT IdStudy FROM Studies WHERE name = @Studies), GetDate())
        END;
    UPDATE Student
    SET IdEnrollment = (SELECT IdEnrollment FROM Enrollment WHERE Semester = @Semester + 1 AND IdStudy = (SELECT IdStudy FROM Studies WHERE name = @Studies))
    WHERE IdEnrollment = (SELECT IdEnrollment FROM Enrollment WHERE Semester = @Semester AND IdStudy = (SELECT IdStudy FROM Studies WHERE name = @Studies))
    
    SELECT e.IdEnrollment, s.Name, e.Semester FROM Enrollment e JOIN Studies s ON e.IdStudy = s.IdStudy WHERE s.Name = @Studies AND e.Semester = @Semester + 1
END