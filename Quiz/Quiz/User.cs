// See https://aka.ms/new-console-template for more information


using Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class User
{

    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime BirthDay { get; set; }
    private List<QuizResult> QuizResults { get; set; }


    public User()
    {
        QuizResults = new List<QuizResult>();
    }

    public void AddQuizResult(QuizResult quizResult)
    {
        QuizResults.Add(quizResult);
    }

    internal List<QuizResult> GetQuizResults()
    {
        return QuizResults;
    }
}