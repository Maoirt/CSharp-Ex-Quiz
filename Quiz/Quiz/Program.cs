using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Xml;
using Quiz;

string loggedInUser = "";
string filePath = @"C:\Users\Maoirt\source\repos\Quiz\Quiz\Records.csv";
string filePathMath = @"C:\Users\Maoirt\source\repos\Quiz\Quiz\Math.xml";
string filePathHistory = @"C:\Users\Maoirt\source\repos\Quiz\Quiz\History.xml";
string filePathCoding = @"C:\Users\Maoirt\source\repos\Quiz\Quiz\Coding.xml";
User userLogged = null;
Entry();

//вход в систему
void Entry()
{
    int input = 0;
    bool isInputValid = false;
    do
    {
        Console.WriteLine("Добро пожаловать в викторину!\n\t [1] - Войти\n\t [2] - Зарегистироваться");
        isInputValid = int.TryParse(Console.ReadLine(), out input);
        if (isInputValid && input == 1)
        {
            Login();
            //return 1;
        }
        else if (isInputValid && input == 2)
        {
            Registration();
            //return 2;
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
        }
    }
    while (!isInputValid || (input != 1 && input != 2));

    //return 0;
}

//Авторизация
void Login()
{
    Console.WriteLine("Введите свое Имя");
    var login = Convert.ToString(Console.ReadLine());
    Console.WriteLine("Введите пароль");
    var password = Convert.ToString(Console.ReadLine());

    //сопоставить вводимые данные с данными в csv файле
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };
    using (var reader = new StreamReader(filePath))
    using (var csv = new CsvReader(reader, config))
    {
        var users = csv.GetRecords<User>();
        foreach(var user in users)
        {
            if (user.Login == login && user.Password == password)
            {
                // пользователь найден
                loggedInUser = user.Login;
                userLogged = user;
                Menu();
                break;
            }

        }
        //пользователь не найден
        Console.WriteLine("Такого аккаунта не существует! Пожалуйста, пройдите регистрация");
        Registration();

        //сделать проверку на верность пароля и введенного логина
    }
}

//Регистрация
void Registration()
{
    Console.WriteLine("Введите свое логин");
    var login = "";
    var loginIsValid = false;
    do
    {
        login = Console.ReadLine();
        if (string.IsNullOrEmpty(login))
        {
            Console.WriteLine("Пожалуйста, введите логин.");
            continue;
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        // Проверка на пустоту файла
        bool isFileEmpty;
        using (var reader = new StreamReader(filePath))
        {
            isFileEmpty = reader.ReadLine() == null;
        }

        if (!isFileEmpty)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var users = csv.GetRecords<User>();
                foreach (var user in users)
                {
                    if (user.Login == login)
                    {
                        Console.WriteLine("Этот логин уже используется! Выберите другой!");
                        loginIsValid = false;
                        userLogged = user;
                        break;
                    }
                    else
                    {
                        loginIsValid = true;
                    }
                }
            }
        }
        else
        {
            loginIsValid = true; // Если файл пустой, то логин считается валидным
        }

        if (!loginIsValid)
        {
            continue;
        }
    } while (!loginIsValid);

    var password = " ";
    var passwordIsValid = false;
    do
    {
        Console.WriteLine("Введите пароль");
        password = Convert.ToString(Console.ReadLine());

        if(password == null)
        {
            Console.WriteLine("Пожалуйста, введите пароль.");
        }
        else if(password.Length < 8)
        {
            Console.WriteLine("Пароль должен состоять больше, чем 8 символов");
        }
        string specialChars = "!#№$%&?*";
        bool containsSpecialChars = password.IndexOfAny(specialChars.ToCharArray()) != -1;
        if (!containsSpecialChars)
        {
            Console.WriteLine("Пароль должен содержать специальные символы! (!,#,№,$,%,&,?,*)");
        }
        else
        {
            passwordIsValid = true;
        }

    }
    while(passwordIsValid == false);


    DateTime birthDay;
    var birthDayString = " ";
    do
    {
        Console.WriteLine("Введите дату рождения в формате dd.MM.yyyy");
        birthDayString = Console.ReadLine();

        if (DateTime.TryParse(birthDayString, out birthDay))
        {
            Console.WriteLine("Вы успешно зарегистрировались!");
        }
    }
    while (!DateTime.TryParse(birthDayString, out birthDay));

    //реализовать запись данных в csv файл
    //var config1 = new CsvConfiguration(CultureInfo.InvariantCulture)
    //{
    //    HasHeaderRecord = false
    //};
    //using (var writer = new StreamWriter(@"C:\Users\Maoirt\source\repos\Quiz\Quiz\Records.csv", true))
    //using (var csv = new CsvWriter(writer, config1))
    //{


    //    csv.WriteRecord(new User { Login = login, Password = password, BirthDay = birthDay });
    //    writer.WriteLine();
    //}
    var config1 = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };
    using (var writer = new StreamWriter(filePath, true))
    using (var csv = new CsvWriter(writer, config1))
    {
        var user = new User { Login = login, Password = password, BirthDay = birthDay };
        userLogged = user;
        csv.WriteField(user.Login);
        csv.WriteField(user.Password);
        csv.WriteField(user.BirthDay);
        csv.NextRecord();
    }
    Menu();

}

//Меню
void Menu()
{
    int input = 0;
    bool isInputValid = false;
    do
    {
        Console.WriteLine("Меню:\n\t [1] - Стартовать новую викторину\n\t [2] - Посмотреть результаты своих прошлых викторин" +
            "\n\t [3] - Посмотреть топ-20 по конкретной тематике\n\t [4] - Изменить настройки\n\t [5] - Выйти\n\t [6] - Редактирование");
        isInputValid = int.TryParse(Console.ReadLine(), out input);
        if (isInputValid && input == 1)
        {
            StartGame();
        }
        else if (isInputValid && input == 2)
        {
            PastResultsQuiz();
        }
        else if (isInputValid && input == 3)
        {
            Console.WriteLine("Выберите тематику викторины:\n\t [1] - История\n\t [2] - Математика" +
    "\n\t [3] - Программирование\n\t [4] - Смешанная \n\t Скоро...");
            var input3 = Console.ReadLine();
            if (int.TryParse(input3, out int input2))
            {
                Topic topic = (Topic)input2;
                Console.WriteLine("Выбранная тема: " + topic.ToString());
                Top20Players(topic.ToString());
            }
            else
            {
                Console.WriteLine("Неверный ввод");
            }
        }
        else if (isInputValid && input == 4)
        {
            ChangeSettings();
        }
        else if (isInputValid && input == 5)
        {
            Console.WriteLine("Мы будем вас ждать! До свидания");
            break;
        }
        else if (isInputValid && input == 6)
        {
            //проверка на админа
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
        }
    }
    while (!isInputValid || (input > 0 && input < 6));
}

//Начала игры
void StartGame()
{
    int input = 0;
    bool isInputValid = false;
    do
    {
        Console.WriteLine("Выберите тематику викторины:\n\t [1] - История\n\t [2] - Математика" +
            "\n\t [3] - Программирование\n\t [4] - Смешанная \n\t Скоро...");
        isInputValid = int.TryParse(Console.ReadLine(), out input);
        if (isInputValid && input == 1)
        {
            HistoryGame();
        }
        else if (isInputValid && input == 2)
        {
            MathGame();
        }
        else if (isInputValid && input == 3)
        {
            CodingGame();
        }
        else if (isInputValid && input == 4)
        {
            MixedGame();
        }
        else
        {
            Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
        }
    }
    while (!isInputValid || (input > 0 && input < 4));
}

void Game(string filePath, string topic)
{
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.Load(filePath);
    XmlNodeList questions = xmlDoc.SelectNodes("//question"); // Получение списка вопросов

    int score = 0;

    foreach (XmlNode question in questions)
    {
        Console.WriteLine(question.SelectSingleNode("text").InnerText);

        XmlNodeList options = question.SelectNodes("options/option"); // Получение списка вариантов ответов на вопрос
        List<int> correctAnswers = new List<int>();


        XmlNodeList correctAnswerNodes = question.SelectNodes("correctAnswer/answer"); // Получение списка правильных ответов на вопрос
        foreach (XmlNode answerNode in correctAnswerNodes)
        {
            int.TryParse(answerNode.InnerText, out int correctAnswer);
            correctAnswers.Add(correctAnswer);
        }
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i].InnerText}");
        }

        Console.Write("Введите номер(а) правильного(ых) ответа(ов): ");
        string input = Console.ReadLine();

        List<int> userAnswers = new List<int>();
        foreach (char answer in input)
        {
            if (int.TryParse(answer.ToString(), out int answerIndex))
            {
                userAnswers.Add(answerIndex);
            }
        }

        bool answeredCorrectly = true;
        foreach (int answer in correctAnswers)
        {
            if (!userAnswers.Contains(answer))
            {
                answeredCorrectly = false;
                break;
            }
        }

        if (answeredCorrectly && correctAnswers.Count == userAnswers.Count)
        {
            score++;
            Console.WriteLine("Правильно!\n");
        }
        else
        {
            Console.WriteLine("Неправильно!\n");
        }
    }
    Console.WriteLine($"Викторина завершена. Количество правильных ответов: {score}");


    QuizResult quizResult = new QuizResult
    {
        PlayerName = Convert.ToString(userLogged),
        QuizName = topic,
        Score = score
    };
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };
    userLogged.AddQuizResult(quizResult);

    Menu();
}


//Математика тематика игры
void MathGame()
{

    Game(filePathMath, "Math");

}

//История тематика игры
void HistoryGame()
{
    Game(filePathHistory, "History");
}

//Программирование тематика игры
void CodingGame()
{
    Game(filePathCoding, "Coding");
}

//Смешанная тематика игры
void MixedGame()
{
    List<string> files = new List<string> { filePathMath, filePathCoding, filePathHistory };
    Random random = new Random();
    int score = 0;
    int totalQuestions = 5; //должно быть 20, но в xml файлах написано только 5
    List<XmlNode> selectedQuestions = new List<XmlNode>();

    while (selectedQuestions.Count < totalQuestions/* && files.Count > 0*/)
    {
        int randomFileIndex = random.Next(0, files.Count);
        string file = files[randomFileIndex];

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(file);
        XmlNodeList questions = xmlDoc.SelectNodes("//question");

        if (questions.Count > 0)
        {
            int randomQuestionIndex = random.Next(0, questions.Count);
            selectedQuestions.Add(questions[randomQuestionIndex]);
            //files.RemoveAt(randomFileIndex);
        }
    }
    foreach (XmlNode question in selectedQuestions)
    {
        Console.WriteLine(question.SelectSingleNode("text").InnerText);

        XmlNodeList options = question.SelectNodes("options/option");
        List<int> correctAnswers = new List<int>();

        XmlNodeList correctAnswerNodes = question.SelectNodes("correctAnswer/answer");
        foreach (XmlNode answerNode in correctAnswerNodes)
        {
            int.TryParse(answerNode.InnerText, out int correctAnswer);
            correctAnswers.Add(correctAnswer);
        }

        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i].InnerText}");
        }

        Console.Write("Введите номер(а) правильного(ых) ответа(ов): ");
        string input = Console.ReadLine();

        List<int> userAnswers = new List<int>();
        foreach (char answer in input)
        {
            if (int.TryParse(answer.ToString(), out int answerIndex))
            {
                userAnswers.Add(answerIndex);
            }
        }

        bool answeredCorrectly = true;
        foreach (int answer in correctAnswers)
        {
            if (!userAnswers.Contains(answer))
            {
                answeredCorrectly = false;
                break;
            }
        }

        if (answeredCorrectly && correctAnswers.Count == userAnswers.Count)
        {
            score++;
            Console.WriteLine("Правильно!\n");
        }
        else
        {
            Console.WriteLine("Неправильно!\n");
        }
    }

    Console.WriteLine($"Викторина завершена. Количество правильных ответов: {score}");

    QuizResult quizResult = new QuizResult
    {
        PlayerName = Convert.ToString(userLogged),
        QuizName = "Mixed",
        Score = score
    };
    userLogged.AddQuizResult(quizResult);
}



//Прошлые результаты
void PastResultsQuiz()
{
    List<QuizResult> pastResults = userLogged.GetQuizResults();

    foreach (QuizResult result in pastResults)
    {
        Console.WriteLine($"Quiz: {result.QuizName}, Score: {result.Score}");
    }
}

//Топ 20 игроков по XX тематике
void Top20Players(string QuizTopic)
{
    List<QuizResult> allQuizResults = userLogged.GetQuizResults();
    List<QuizResult> topicQuizResults = allQuizResults.Where(s => s.QuizName == QuizTopic).ToList();
    List<QuizResult> top20Players = topicQuizResults.OrderByDescending(s => s.Score).Take(20).ToList();

    for (int i = 0; i < top20Players.Count; i++)
    {
        QuizResult playerResult = top20Players[i];
        Console.WriteLine($"{i + 1}. {playerResult.PlayerName} - {playerResult.Score} очков");
    }
}

//Изменить настройки
//возникает ошибка
void ChangeSettings()
{
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };
    User user = null;
    List<User> users = new List<User>();
    try
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            users = csv.GetRecords<User>().ToList();
            user = users.FirstOrDefault(u => u.Login == loggedInUser);
            userLogged = user;
            if (user != null)
            {
                int input = 0;
                bool isInputValid = false;
                do
                {
                    Console.WriteLine($"Добро пожаловать, {user.Login}!");

                    Console.WriteLine("Какие настройки вы хотите изменить: \n\t[1] - Изменить дату рождения\n\t[2] - Изменить пароль");
                    isInputValid = int.TryParse(Console.ReadLine(), out input);
                    if (isInputValid && input == 1)
                    {
                        Console.WriteLine("Введите дату рождения в формате dd.MM.yyyy");
                        var birthDayString = Console.ReadLine();

                        if (DateTime.TryParse(birthDayString, out var newBirthDay))
                        {
                            user.BirthDay = newBirthDay;
                            isInputValid = true;
                        }
                        //else
                        //{
                        //    Console.WriteLine("Некорректный формат даты. Попробуйте еще раз.");
                        //}

                    }
                    else if (isInputValid && input == 2)
                    {
                        Console.WriteLine("Введите новый пароль:");
                        string newPassword = Convert.ToString(Console.ReadLine());
                        user.Password = newPassword;
                        isInputValid = true;
                        Console.WriteLine("Пароль успешно изменен!");
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
                    }
                }
                while (!isInputValid || (input != 1 && input != 2));
            }
            else
            {
                Console.WriteLine("Не удалось найти данные пользователя.");
            }
        }
        using (var writer = new StreamWriter(filePath))
        using (var csvWriter = new CsvWriter(writer, config))
        {
            foreach (var userToUpdate in users)
            {
                if (userToUpdate.Login == loggedInUser)
                {

                    userToUpdate.BirthDay = user.BirthDay;
                    userToUpdate.Password = user.Password;
                }


                csvWriter.WriteRecord(userToUpdate);
            }

        }
    }

    catch (IOException ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}

//сделать редактирование вопросов через админ панель (для админа)
//сделать смешаннаую викторину
//создать функцию для викторин, т.к по факту 3 больших куска кода можно заменить одним
//улучшить, вынести в отдельные функции блоки кода и т.д

