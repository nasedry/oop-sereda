using System;

class SportTeam
{
    private string name;
    private string coach;
    public int PlayersCount { get; set; }

    public SportTeam(string name, string coach, int playersCount)
    {
        this.name = name;
        this.coach = coach;
        PlayersCount = playersCount;
        Console.WriteLine($"[Створено] Команда {name}, тренер: {coach}, гравців: {playersCount}");
    }

    ~SportTeam()
    {
        Console.WriteLine($"[Знищено] Команда {name}");
    }

    public void PlayMatch()
    {
        Console.WriteLine($"Команда '{name}' (тренер: {coach}) зіграла матч з {PlayersCount} гравцями.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        SportTeam team1 = new SportTeam("Леви", "Іван Петров", 11);
        SportTeam team2 = new SportTeam("Тигри", "Олена Коваль", 9);
        SportTeam team3 = new SportTeam("Орли", "Сергій Іваненко", 7);

        team1.PlayMatch();
        team2.PlayMatch();
        team3.PlayMatch();

        Console.WriteLine($"\nКількість гравців у команді team1: {team1.PlayersCount}");
        Console.WriteLine($"Кількість гравців у команді team2: {team2.PlayersCount}");
        Console.WriteLine($"Кількість гравців у команді team3: {team3.PlayersCount}");
    }
}
