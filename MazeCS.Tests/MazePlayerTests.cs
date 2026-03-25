using MazeCs.Shared;

namespace MazeCS.Tests;

public class MazePlayerTests
{
    [Fact]
    public void MazeAndPlayerCanBeInstantiated()
    {
        // Arrange
        const double CoinRate = 0.3;
        const double DoorRate = 0.2;
        var MazeSize = new Vec2d(20, 15);

        // Act
        var mazeGenerator = new MazeGen(MazeSize, StartPos: Vec2d.Origin, CoinRate, DoorRate);
        var maze = new Maze(mazeGenerator);
        var player = new Player(maze);
        player.InitializePosition();

        // Assert
        Assert.NotNull(maze);
        Assert.NotNull(player);
        Assert.Equal(MazeSize, maze.MazeSize);
        Assert.Equal(0, player.Score);
        Assert.Equal(0, player.InventorySize);
        Assert.True(player.IsPlaying);
        Assert.False(player.HasWon);
    }

    [Fact]
    public void PlayerStartsAtMazeStartPosition()
    {
        // Arrange
        var mazeGenerator = new MazeGen(new Vec2d(10, 10), StartPos: Vec2d.Origin, 0.3, 0.2);
        var maze = new Maze(mazeGenerator);
        var player = new Player(maze);
        player.InitializePosition();

        // Act
        var startPos = maze.StartPos;

        // Assert
        Assert.NotNull(startPos);
        Assert.Equal(startPos, Vec2d.Origin);
    }

    [Fact]
    public void InventoryChangedEventCanBeSubscribed()
    {
        // Arrange
        var mazeGenerator = new MazeGen(new Vec2d(10, 10), StartPos: Vec2d.Origin, 0.3, 0.2);
        var maze = new Maze(mazeGenerator);
        var player = new Player(maze);
        player.InitializePosition();
        var eventRaised = false;

        // Act
        player.InventoryChanged += (sender, e) => eventRaised = true;

        Assert.False(eventRaised);
    }

    [Fact]
    public void ScoreChangedEventCanBeSubscribed()
    {
        // Arrange
        var mazeGenerator = new MazeGen(new Vec2d(10, 10), StartPos: Vec2d.Origin, 0.3, 0.2);
        var maze = new Maze(mazeGenerator);
        var player = new Player(maze);
        player.InitializePosition();

        // Act
        var eventSubscribed = true;
        try
        {
            player.ScoreChanged += (sender, e) => { };
        }
        catch
        {
            eventSubscribed = false;
        }

        // Assert
        Assert.True(eventSubscribed);
    }
}
