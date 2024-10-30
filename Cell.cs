namespace Maze
{
    public enum CellType { HALL, WALL, MEDAL, ENEMY, HERO,HEAL,ENERGY };

    public class Cell
    {
        public static Bitmap[] Images = {
            new Bitmap(Properties.Resources.hall),
            new Bitmap(Properties.Resources.wall),
            new Bitmap(Properties.Resources.medal),
            new Bitmap(Properties.Resources.enemy), 
            new Bitmap(Properties.Resources.player),
            CreateRedBitmap(16, 16, 1),
            CreateRedBitmap(16, 16, 2)
        };

        public CellType Type { get; set; }

        public Image Texture { get; set; }

        public Cell(CellType type)
        {
            Type = type;
            Texture = Images[(int)Type];
        }

        private static Bitmap CreateRedBitmap(int width, int height,int mode)
        {
            Bitmap redBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(redBitmap))
            {
                if (mode == 1)
                {
                    g.Clear(Color.Red);
                }
                else if (mode == 2)
                {
                    g.Clear(Color.Blue);
                }
            }
            return redBitmap;
        }
    }
}