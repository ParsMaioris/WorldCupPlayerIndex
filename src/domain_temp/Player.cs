public class Player
{
    public string Name { get; }
    public string Nationality { get; }
    public int GoalsScored { get; }
    public int Age { get; }

    public Player(string name, string nationality, int goalsScored, int age)
    {
        Name = name;
        Nationality = nationality;
        GoalsScored = goalsScored;
        Age = age;
    }

    public bool IsVeteran() => Age >= 35;
    public bool IsTopScorer(int threshold) => GoalsScored >= threshold;
    public double GetPerformanceScore() => Age > 0 ? (double)GoalsScored / Age : 0;
}