namespace Maze
{
    public partial class LevelForm : Form
    {
        public Maze maze;
        public Character Hero;
        private int hpValue = 3;
        public Label HP = new Label();
        public Label Medals = new Label();
        public Label Energy = new Label();
        private int medals = 0;
        private int totalMedals = 0;
        private int energy = 100;
        public LevelForm()
        {
            InitializeComponent();
            FormSettings();
            Hero = new Character(this);
            maze = new Maze(this);
            maze.Generate();
            totalMedals = maze.medals;
            InitializeHPLabel();
            InitializeMedalLabel();
            InitializeEnergyLabel();
            StartGameProcess(); 
        }

        public void FormSettings()
        {
            Text = Configuration.Title;
            BackColor = Configuration.Background;
            ClientSize = new Size(
                Configuration.Columns * Configuration.PictureSide,
                Configuration.Rows * Configuration.PictureSide);

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeHPLabel()
        {
            HP.Text = $"HP: {hpValue}";
            HP.Font = new Font("Arial", 16, FontStyle.Bold);
            HP.ForeColor = Color.Red;
            HP.Location = new Point(10, 330);
            HP.AutoSize = true;
            Controls.Add(HP);
        }

        private void InitializeEnergyLabel()
        {
            Energy.Text = $"Energy: {energy}";
            Energy.Font = new Font("Arial", 16, FontStyle.Bold);
            Energy.ForeColor = Color.Blue;
            Energy.Location = new Point(50, 360);
            Energy.AutoSize = true;
            Controls.Add(Energy);
        }

        private void InitializeMedalLabel()
        {
            Medals.Text = $"Medal: {medals}/{totalMedals}";
            Medals.Font = new Font("Arial", 16, FontStyle.Bold);
            Medals.ForeColor = Color.Gold;
            Medals.Location = new Point(100, 330);
            Medals.AutoSize = true;
            Controls.Add(Medals);
        }

        public void StartGameProcess()
        {
            maze.Show();
        }

        private void UpdateHPLabel()
        {
            HP.Text = $"HP: {hpValue}";
        }

        private void UpdateMedals()
        {
            Medals.Text = $"Medal: {medals}/{totalMedals}";
        }
        private void UpdateEnergy()

        {
            Energy.Text = $"Energy: {energy}";
            if (energy == 0)
            {
                MessageBox.Show("0 Energy. U lose!");
                this.Close();
            }
        }
        public void DecreaseHP(int amount)
        {
            hpValue -= amount;
            if (hpValue < 0) hpValue = 0;
            UpdateHPLabel();
            if (hpValue == 0)
            {
                MessageBox.Show("0HP. U lose!");
                this.Close();
            }
        }

        public void IncreaseHP(int amount)
        {
            if (hpValue < 3)
            {
                hpValue += amount;
                if (hpValue < 0) hpValue = 0;
                UpdateHPLabel();
            } 
        }

        private void CheckEvent(CellType t)
        {
            if (t == CellType.ENEMY)
            {
                DecreaseHP(1);
            }
            else if (t == CellType.HEAL)
            {
                IncreaseHP(1);
            }
            else if (t == CellType.MEDAL)
            {
                medals++;
                UpdateMedals();
                if (medals == totalMedals)
                {
                    MessageBox.Show("Победа - медали собраны!");
                    this.Close();
                }
            }else if (t == CellType.ENERGY)
            {
                energy += 10;
                UpdateEnergy();
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            CellType previous = CellType.HERO;
            if (e.KeyCode == Keys.Right)
            {
                if (maze.cells[Hero.PosY, Hero.PosX + 1].Type != CellType.WALL)
                {
                    previous = maze.cells[Hero.PosY, Hero.PosX + 1].Type;
                    energy--;
                    UpdateEnergy();
                    Hero.Clear();
                    Hero.MoveRight();
                    Hero.Show();
                }
                CheckEvent(previous);

                if (Hero.PosX == 19)
                {
                    MessageBox.Show("Вы нашли выход!!!");
                    this.Close();
                }
            }
            else if (e.KeyCode == Keys.Left && Hero.PosX != 0)
            {
                if (maze.cells[Hero.PosY, Hero.PosX - 1].Type != CellType.WALL)
                {
                    previous = maze.cells[Hero.PosY, Hero.PosX - 1].Type;
                    energy--;
                    UpdateEnergy();
                    Hero.Clear();
                    Hero.MoveLeft();
                    Hero.Show();
                }
                CheckEvent(previous);
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (maze.cells[Hero.PosY - 1, Hero.PosX].Type != CellType.WALL)
                {
                    previous = maze.cells[Hero.PosY - 1, Hero.PosX].Type;
                    energy--;
                    UpdateEnergy();
                    Hero.Clear();
                    Hero.MoveUp();
                    Hero.Show();
                }
                CheckEvent(previous);
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (maze.cells[Hero.PosY + 1, Hero.PosX].Type != CellType.WALL)
                {
                    previous = maze.cells[Hero.PosY + 1, Hero.PosX].Type;
                    energy--;
                    UpdateEnergy();
                    Hero.Clear();
                    Hero.MoveDown();
                    Hero.Show();
                }
                CheckEvent(previous);
            }
        }
    }
}
