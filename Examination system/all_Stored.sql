use Examination_System
Go
/******************* Student_Exam ******************/
-- Delete All student Exams
Create Proc P_StudentExam_DeleteStudent @student_id int
AS
	delete from Student_Exam where Student_id = @student_id
GO

-- select student answers for Exam
Create Proc P_StudentExam_SelectAnswers @examID int, @studentID int, @answers varchar(50) out
AS
	select @answers = answers from Student_Exam where Exam_id = @examID and Student_id = @studentID
GO

-- insert student answer
Create Proc P_StudentExam_InsertAnswer @examID int, @studentID int, @answers nvarchar(100)
AS
	insert into Student_Exam(Exam_id, Student_id, answers) values(@examID, @studentID, @answers)
GO

-- update student result
Create proc P_StudentExam_UpdateResult @result int, @examID int, @studentID int
AS
	update Student_Exam set results = @result where Exam_id = @examID and Student_id = @studentID
GO

/******************* Question Procedures ******************/

-- Procedure returns all Questions
Create Proc P_Question_Select
AS
	Select * From Exam.Questions
GO

-- Procedure returns all MCQ Questions 
Create Proc P_Question_SelectMCQ @mcq_count int ,@course_id int, @exam_id int
AS
	Select top(@mcq_count)@exam_id, ID
	From Exam.Questions
	Where type ='MCQ' AND Course_id = @course_id
	order by newid()
GO

-- Procedure returns all True-False Questions 
Create Proc P_Question_SelectTF @tf_count int, @course_id int, @exam_id int
AS
	Select top(@tf_count)@exam_id, ID
	From Exam.Questions
	Where type ='TF' AND Course_id = @course_id
	order by newid()
GO

-- Procedure to update a question using its id
-- returns 1 if update sucsess, 0 if question Exists in Exam.
Create Proc P_Question_Update @id int, @question_statment nvarchar(50), @type nvarchar(10), @model_answer nchar(2), @option1 nvarchar(50), @option2 nvarchar(50), @option3 nvarchar(50), @option4 nvarchar(50), @course_id int
AS
	if Exists(select Question_id from Exam.Exam_Questions where Question_id = @id)
		return 0
	else
	BEGIN
		Update [Exam].Questions
		set question_Statment = @question_statment,
			type = @type,
			modelAnswer = @model_answer,
			option1 = @option1,
			option2 = @option2,
			option3 = @option3,
			option4 = @option4,
			Course_id = @course_id
		Where ID = @id
		return 1
	END
GO

-- Procedure to inserts a MCQ Question
Create Proc P_Question_insertMCQ  @question_statment nvarchar(50), @model_answer nchar(2), @option1 nvarchar(50), @option2 nvarchar(50), @option3 nvarchar(50), @option4 nvarchar(50), @course_id int
AS
	Insert into [Exam].Questions
	values(@question_statment,'MCQ',@model_answer,@option1,@option2,@option3,@option4,@course_id)
GO

-- Procedure to inserts a True-False Question
Create Proc P_Question_insertTF  @question_statment nvarchar(50), @model_answer nchar(2), @course_id int
AS
	Insert into [Exam].Questions(question_Statment,[type], modelAnswer,Course_id)
	values(@question_statment,'TF',@model_answer,@course_id)
GO

-- Procedure to delete a Question returns 1 if sucsess, 0 if exists in Exam_Questions
Create Proc P_Question_delete @id int
AS
	if Exists(select Question_id from Exam.Exam_Questions where Question_id = @id)
		return 0
	else
	BEGIN
		Delete from [Exam].Questions
		where ID = @id
		return 1
	end
GO
/************** Exam_Question *********************/
-- delete all references for a specific exam 
create Proc P_ExamQuestions_deleteExam @exam_id int
AS
	delete from Exam.Exam_Questions where Exam_id = @exam_id
GO

-- insert questions to exam
Create Proc P_ExamQuestions_Insert @exam_id int, @mcq_count int, @tf_count int, @course_id int
AS
	insert into Exam.Exam_Questions
		exec P_Question_SelectMCQ @mcq_count, @course_id, @exam_id
	insert into Exam.Exam_Questions
		exec P_Question_SelectTF @tf_count, @course_id, @exam_id
GO


/************** Exam Procedures ********************/

-- Returns all exams
Create Proc P_Exam_Select
AS
	Select * From Exam.Exams
GO

-- return model anwser for  specific exam
Create Proc P_Exams_SelectModelAnswer @exam_id int, @answers varchar(50) out
AS
	select @answers = ModelAnswer from Exam.Exams where ID = @exam_id
GO

-- Returns all exams related to a specific course
Create Proc P_Exam_Select_With_Course_id @course_id int
AS
	Select ID From Exam.Exams where Course_id = @course_id
GO

-- Add new Exam returns Exam ID
Create Proc P_Exam_Insert @mcq_count int, @tf_count int, @course_id int, @examID int out
AS
	insert into Exam.Exams(MCQ_Count,TF_Count,Course_id)
	Values(@mcq_count,@tf_count,@course_id)
	select @examID = @@IDENTITY
GO

-- Generate model answer for exam
Create Proc P_Exam_UpdateModelAnswer @examID int
as
	declare c1 Cursor
	for select modelAnswer from Exam.Exam_Questions eq join Exam.Questions q on eq.Question_id = q.ID  where Exam_id = @examID
	for read only

	declare @Answer char,@ModelAnswers varchar(100)=''
	open c1
	fetch c1 into @Answer
	while @@FETCH_STATUS=0
		begin
			set @ModelAnswers=concat(@ModelAnswers,@Answer)
			fetch c1 into @Answer
		end

	update Exam.Exams set ModelAnswer = @ModelAnswers where ID = @examID
	close c1
	deallocate c1
GO

-- reGenerate exam and update it in exams table
-- input(ExamID, TF_Count, MCQ_Count)
Create Proc P_reGenerateExam @examID int, @TF_Count int, @MCQ_Count int, @Course_id int
as
	exec P_ExamQuestions_Insert @examID, @MCQ_Count, @TF_Count,@Course_id	
	exec P_Exam_UpdateModelAnswer @examID
	select @examID as [Exam ID]
GO

-- Update an Exam
Create Proc P_Exam_Update @id int, @mcq_count int, @tf_count int, @course_id int
AS
	if Exists( select Exam_id from Student_Exam where Exam_id = @id)
		return 0
	else
	begin
		Update Exam.Exams
		set MCQ_Count = @mcq_count,
		TF_Count = @tf_count,
		Course_id = @course_id
		Where ID = @id
		-- Call Proc to delete from Exam_Question where exam = exam
		exec P_ExamQuestions_deleteExam @id
		-- regenerateExam @exam_id int, @mcq_count in, @tf_count int
		 exec P_reGenerateExam @id, @mcq_count, @tf_count, @course_id
		return 1
	end 
GO

-- delete an Exam
Create Proc P_Exam_Delete @id int
AS
	if Exists ( select Exam_id from Student_Exam where Exam_id = @id)
		return 0
	else
	Begin
		exec P_ExamQuestions_deleteExam @id
		Delete from Exam.Exams
		Where ID = @id
		return 1
	end
GO

-- Generate New Exam
Create Proc P_GenerateExam @courseID int, @TF_Count int, @MCQ_Count int
as
	declare @examID int

	exec P_Exam_Insert @mcq_count,@tf_count,@courseID,@examID out

	exec P_ExamQuestions_Insert @examID, @MCQ_Count, @TF_Count,@courseID
	
	exec P_Exam_UpdateModelAnswer @examID
	select @examID as [Exam ID]
GO

-- take string like 'ABCDE' and return 1 column with each char in row
Create function split_string_to_rows(@string varchar(100))
returns table
as
return
(
	WITH CTE AS 
	(
		SELECT
			1 as CharacterPosition,
			SUBSTRING(@string,1,1) as Character
		UNION ALL
		SELECT
			CharacterPosition + 1,
			SUBSTRING(@string,CharacterPosition + 1,1)
		FROM
			CTE
		WHERE CharacterPosition < LEN(@string)
	)
	SELECT CharacterPosition as id,Character FROM CTE
)

Go
-- Correct an Exam
Create Proc P_CorrectExam @examID int, @studentID int
as
	declare @studentAnswer varchar(100), @modelAnswer varchar(100)

	exec P_StudentExam_SelectAnswers @examID , @studentID, @studentAnswer out
	exec P_Exams_SelectModelAnswer @examID, @modelAnswer out

	declare @t table(id int,modelAnswer char, studentAnswer char)

	insert into @t
	select modelAnswer.id ,modelAnswer.Character, studentAnswer.Character
	from split_string_to_rows(@modelAnswer) as modelAnswer join split_string_to_rows(@studentAnswer) as studentAnswer
	on modelAnswer.id = studentAnswer.id

	declare @questionCount int
	select @questionCount = count(id) from @t
	
	declare @result int
	select @result = ceiling(count(t1.id) * 100.0 / @questionCount) from @t t1 join @t t2 on t1.id = t2.id and t1.modelAnswer = t2.studentAnswer
	
	exec P_StudentExam_UpdateResult @result, @examID , @studentID
GO

-- save student exam answers
-- input(examID, studentID, answers)
Create Proc P_submitAnswers @examID int, @studentID int, @answers nvarchar(100)
as
	exec P_StudentExam_InsertAnswer @examID, @studentID, @answers
	exec P_CorrectExam @examID, @studentID
GO

-- Get Exam
Create Proc P_Report_GetExam @examID int
as
	select q.[type], q.ID, q.question_Statment, q.option1, option2, option3, option4
	from Exam.Exam_Questions eq join Exam.Questions q on eq.Question_id = q.ID
	where eq.Exam_id = @examID
GO

-- Get Exam with Student Answers
Create Proc P_Report_GetExamWithStudentAnswers @examID int, @studentID int
as
	declare @studentAnswer varchar(100)
	set @studentAnswer = (select answers  from Student_Exam where Exam_id = @examID and Student_id = @studentID)

	select [type], main.ID, question_Statment, option1, option2, option3, option4, [Character] as [student answer] from 
	(
	select q.[type], q.ID, q.question_Statment, q.option1, option2, option3, option4, ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS rownum
	from Exam.Exam_Questions eq join Exam.Questions q on eq.Question_id = q.ID 
	where eq.Exam_id = @examID
	) as main
	join split_string_to_rows(@studentAnswer) sa on sa.id = main.rownum
GO

/***stored procedure for topic ***/

--select the data from topic table
create proc p_topic_select
as
	select * from course.Topics
GO

-- get all topics related to course
create proc p_topic_selectCourseTopics @course_id int
as
	select * from course.Topics where Course_id = @course_id
GO

--insert new topic
create proc p_topic_insert @id int,@name varchar(50)
as
	begin try
		insert into Course.Topics values(@id ,@name )
		return 1 -- sucsess
	end try
	begin catch
		return 0 -- failed
	end catch
GO

-- update name of the topic
-- IMPORTANT:- Save the old Name and old ID in the UI before Updating
create proc p_topics_update @oldId int, @newId int,@oldname varchar(50),@newname varchar(50)
as
	update Course.Topics 
	set [name] = @newname,
		Course_id = @newId
	where course_id=@oldId and name = @oldname
GO

--delete topic depending on id and name
create proc p_topics_delete @id int,@name varchar(50)
as
	delete from course.topics where Course_id = @id and [name] = @name
GO

-- delete all topics related to a course
Create proc P_Topic_DeleteCourseTopics @course_id int
AS
	delete from Course.Topics where Course_id = @course_id
GO

/************** Course Procedures ********************/

--select the data from course table
create proc p_courses_select 
as
	select * from course.Courses
GO

--insert name,duration and instructor_id in the course table and returns course id
create proc p_courses_insert @name varchar(50),@duration int,@ins_id int
as
	begin try
		insert into Course.Courses(name,duration,instructor_id) 
		values(@name,@duration,@ins_id)
		return @@IDENTITY
	end try
	begin catch
		return 0 -- Can't insert Course
	end catch
GO

--update name,duration and instructor_id in the course table
create proc p_courses_update @id int,@name varchar(50),@duration int ,@ins_id int
as
	begin try
		update Course.Courses 
		set [name]=@name,
			duration=@duration,
			Instructor_id=@ins_id 
		where id=@id
		return @id
	end try
	begin catch
		return 0
	end catch
GO

--delete row from course table with id
create proc p_course_delete @id int
as
	if Exists(select Course_id from Exam.Exams where Course_id = @id)
		return 0 --can't delete because its in Exam table
	else
	Begin
		exec P_Topic_DeleteCourseTopics @id
		delete from Course.Courses 
		where id=@id
		return 1 -- deleted sucsessfully
	END
GO

/************** stored procedure for student ***********************/

--select the data from student table
create proc p_students_select
as
	select * from school.students
GO

--insert new student
create proc p_students_insert @name varchar(50),@level int ,@depID int
as 
	insert into school.students values(@name,@level,@depID)
GO


--updatet name,level and departmentr_id in the students table
create proc p_stuents_update @id int,@name varchar(50),@level int ,@depID int
as
	begin try
		update School.Students 
		set [name]=@name,
			[level]=@level,
			departmentID=@depID 
		where id=@id
		return 1 -- success
	end try	
	begin catch
		return 0 --fail
	end catch
GO

-- delete Student
create proc p_students_delete @id int
as
	-- Student_Exam, Student_Course
	if Exists(select Student_id from Student_Exam)
		exec P_StudentExam_DeleteStudent @id
	delete from School.Students 
	where id=@id
GO

/*****************Instructor********************/

create proc p_Instructors_Insert @name nvarchar(50),@departmentID int
as
	INSERT INTO School.Instructors VALUES(@name,@departmentID)
GO
--------------------------------------------------------------------------------------------------------------
create proc p_Instructors_Select
as
	select * from School.Instructors
--------------------------------------------------------------------------------------------------------------
GO
create proc p_Instructors_UpdateDepartmentID @instructorID int, @departmentID int
as
	if EXISTS (select ID from School.Instructors where ID=@instructorID )
	BEGIN
		update School.Instructors set departmentID=@departmentID where ID=@instructorID
		return 1
	END
	ELSE
		return 0
GO
--------------------------------------------------------------------------------------------------------------

create proc p_Instructors_Update @instructorID int,@departmentID int,@instructorName nvarchar(50) 
as
	if EXISTS (select ID from School.Instructors where ID=@instructorID )
	BEGIN
		update School.Instructors set departmentID=@departmentID, name=@instructorName
		where ID = @instructorID
		return 1
	END
	ELSE
		return 0
GO
--------------------------------------------------------------------------------------------------------------

create proc p_Instructors_UpdateName @instructorID int,@name nvarchar(50)
as
	if EXISTS (select ID from School.Instructors where ID=@instructorID )
	BEGIN
		update School.Instructors set name=@name where ID=@instructorID
		return 1
	END
	ELSE
		return 0
GO
--------------------------------------------------------------------------------------------------------------
Create proc p_Instructor_Delete  @id int
as
	if EXISTS (select ManagerID from School.Departments where ManagerID=@id )
	begin
		update School.Departments
		set ManagerID =NULL
		Where ManagerID = @id	
	end

	delete from School.Instructors
	where ID = @id
	return 1
GO
--******************Departments************************

-- insert new department
create Proc P_Department_insert @Name nvarchar(50), @mangerID int
as
	insert into School.Departments(name, ManagerID) values(@Name, @mangerID)

GO
-- update department info
create Proc P_Department_Update @id int, @Name nvarchar(50), @mangerID int
as
	if EXISTS (select ID from School.Departments where ID=@id )
	BEGIN
		update School.Departments set name = @Name, ManagerID = @mangerID where ID = @id
		return 1
	end
	else
		return 0

GO

-- select all departments
create Proc P_Department_select @id int
as
	select * from School.Departments
GO

-- delete department
create Proc P_Department_delete @id int
as
	if Exists(select departmentID from School.Instructors)
	delete from School.Departments where ID = @id
GO

------------------------------------------------------------
create Proc P_Report_GetStudentsInfo @departmentID int
as
	select s.name, s.[level], d.name
	from School.Students s join School.Departments d on s.departmentID = d.ID where departmentID = @departmentID

Go

create Proc  P_Report_GetStudentGrades @studentID int
as
	select s.name as [student name], c.name as [course name], se.results as grade from School.Students s join Student_Exam se on se.Student_id = s.ID
	join Exam.Exams e on se.Exam_id = e.ID join Course.Courses c on e.Course_id = c.ID
	where se.Student_id = @studentID

Go

create Proc  P_Report_GetInstructorCourses @instructorID int
as
	select i.name as [instructor name], c.name as [course name], count(sc.Student_id) as [student count] from Course.Courses c join Student_Course sc on sc.Course_id = c.ID
	join School.Instructors i on c.Instructor_id = i.ID where i.ID = @instructorID group by c.ID,i.name,c.name
	 
Go

create Proc  P_Report_GetCourseTopics @courseID int
as
	select c.name as [course name], t.name as [topic name] from Course.Courses c join Course.Topics t on t.Course_id = c.ID where c.ID = @courseID

Go