[TestClass]
public class PlayerUnitTests
{
    [TestMethod]
    public void Test_IsVeteran_ReturnsTrue_WhenAgeIs35OrMore()
    {
        var veteranPlayer = new Player("John Doe", "USA", 50, 35);

        var isVeteran = veteranPlayer.IsVeteran();

        Assert.IsTrue(isVeteran, "Expected player to be considered a veteran when age is 35 or above.");
    }

    [TestMethod]
    public void Test_IsVeteran_ReturnsFalse_WhenAgeIsLessThan35()
    {
        var youngPlayer = new Player("Jane Doe", "USA", 20, 30);

        var isVeteran = youngPlayer.IsVeteran();

        Assert.IsFalse(isVeteran, "Expected player not to be considered a veteran when age is below 35.");
    }

    [TestMethod]
    public void Test_IsTopScorer_ReturnsTrue_WhenGoalsExceedThreshold()
    {
        var player = new Player("John Doe", "USA", 50, 30);

        var isTopScorer = player.IsTopScorer(40);

        Assert.IsTrue(isTopScorer, "Expected player to be a top scorer when goals exceed the threshold.");
    }

    [TestMethod]
    public void Test_IsTopScorer_ReturnsFalse_WhenGoalsDoNotExceedThreshold()
    {
        var player = new Player("John Doe", "USA", 50, 30);

        var isTopScorer = player.IsTopScorer(60);

        Assert.IsFalse(isTopScorer, "Expected player not to be a top scorer when goals do not reach the threshold.");
    }

    [TestMethod]
    public void Test_GetPerformanceScore_CalculatesCorrectly()
    {
        var player = new Player("John Doe", "USA", 30, 10);
        double expectedScore = 3.0;

        var score = player.GetPerformanceScore();

        Assert.AreEqual(expectedScore, score, 0.001, "Expected performance score to be calculated as goals divided by age.");
    }
}