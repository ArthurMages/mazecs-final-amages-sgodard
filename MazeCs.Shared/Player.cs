namespace MazeCs.Shared;

public class Player(Maze maze)
{
    public const string PlayerSymbol = "@";

    public bool Move(IController kbd, out Vec2d prevPos)
    {
        var prevSize  = InventorySize;
        var prevScore = Score;
        
        if (kbd.IsCollectPressed)
            _inventory.AddRange(maze[Position].Collect(ref _score));

        prevPos = Position;
        Position = NextWalkablePos(Position + kbd.DirectionPressed);                
        
        HasWon    = maze[Position].IsEndPos;
        IsPlaying = !HasWon && !kbd.IsEscapePressed;

        if (prevSize !=InventorySize) InventoryChanged?.Invoke(this, EventArgs.Empty);
        if (prevScore!=Score        ) ScoreChanged    ?.Invoke(this, EventArgs.Empty);
        return prevPos != Position;
    }

    private Vec2d NextWalkablePos(Vec2d nextPos) =>
        nextPos.IsIn(maze.MazeSize) && maze[nextPos].TryTraverse(_inventory) ? nextPos : Position;
    public void Draw(IGridDisplay gridDisp) =>
        gridDisp.DrawGridCell(Position, PlayerSymbol, ConsoleColor.Yellow);
    public Maze Maze => maze;
    public bool IsPlaying { get; private set; } = true;
    public bool HasWon    { get; private set; } = false;

    public Vec2d Position { get; private set; } = Vec2d.Origin;

    public event EventHandler? InventoryChanged;

    public event EventHandler? ScoreChanged;

    public int Score => _score;

    public int InventorySize => _inventory.Count;

    private readonly List<ICollectable> _inventory = new();
    private int _score = 0;

    public void InitializePosition() => Position = maze.StartPos;
}
