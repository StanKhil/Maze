namespace Maze
{
    public class Maze
    {
        public LevelForm Parent { get; set; }

        public Cell[,] cells;
        public static Random r = new Random();
        public int medals = 0;
        public Maze(LevelForm parent)
        {
            Parent = parent;
            cells = new Cell[Configuration.Rows, Configuration.Columns];
        }

        public void Generate()
        {
            for (ushort row = 0; row < Configuration.Rows-6; row++)
            {
                for (ushort col = 0; col < Configuration.Columns; col++)
                {
                    CellType cell = CellType.HALL;
                    if (r.Next(5) == 0)
                    {
                        cell = CellType.WALL;
                    }
                    if (r.Next(250) <10)
                    {
                        cell = CellType.MEDAL;
                        medals++;
                    }
                    if (r.Next(250) < 10)
                    {
                        cell = CellType.ENEMY;
                    }
                    if (r.Next(250) < 5)
                    {
                        cell = CellType.HEAL;
                    }
                    if (r.Next(250) < 5)
                    {
                        cell = CellType.ENERGY;
                    }

                    if (row == 0 || col == 0 ||
                        row == Configuration.Rows - 1-6 ||
                        col == Configuration.Columns - 1)
                    {
                        cell = CellType.WALL;
                    }

                    if (col == Parent.Hero.PosX &&
                        row == Parent.Hero.PosY)
                    {
                        cell = CellType.HERO;
                    }

                    if (col == Parent.Hero.PosX + 1 &&
                        row == Parent.Hero.PosY ||
                        col == Configuration.Columns - 1 &&
                        row == Configuration.Rows - 3-6)
                    {
                        cell = CellType.HALL;
                    }

                    cells[row, col] = new Cell(cell);
                    var picture = new PictureBox();
                    picture.Name = "pic" + row + "_" + col;
                    picture.Width = Configuration.PictureSide;
                    picture.Height = Configuration.PictureSide;
                    picture.Location = new Point(
                        col * Configuration.PictureSide,
                        row * Configuration.PictureSide);

                    picture.BackgroundImage = cells[row, col].Texture;
                    picture.Visible = false;
                    Parent.Controls.Add(picture);
                }
            }
        }

        public void Show()
        {
            foreach (var control in Parent.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Visible = true;
                }
            }
        }
    }
}
