using System.Windows.Forms;
using System.Drawing;
using System.Timers;

namespace Maze
{
    public partial class LevelForm : Form
    {
        public Maze maze;
        public Character Hero;
        private int hpValue = 3;
        private int medals = 0;
        private int totalMedals = 0;
        private int energy = 100;
        private int timerDuration = 300;
        private System.Windows.Forms.Timer gameTimer;

        private StatusStrip statusStrip;
        private ToolStripStatusLabel hpLabel;
        private ToolStripStatusLabel medalsLabel;
        private ToolStripStatusLabel energyLabel;
        private ToolStripStatusLabel timerLabel;

        public LevelForm()
        {
            InitializeComponent();
            FormSettings();
            InitializeStatusStrip();
            Hero = new Character(this);
            maze = new Maze(this);
            maze.Generate();
            totalMedals = maze.medals;
            UpdateMedalsLabel();
            StartGameProcess();
            StartTimer();
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

        private void InitializeStatusStrip()
        {
            
            statusStrip = new StatusStrip();

            hpLabel = new ToolStripStatusLabel($"HP: {hpValue}");
            hpLabel.ForeColor = Color.Red;

            medalsLabel = new ToolStripStatusLabel($"Medals: {medals}/{totalMedals}");
            medalsLabel.ForeColor = Color.Gold;

            energyLabel = new ToolStripStatusLabel($"Energy: {energy}");
            energyLabel.ForeColor = Color.Blue;

            timerLabel = new ToolStripStatusLabel($"Time Left: {timerDuration}s               ");
            timerLabel.ForeColor = Color.DarkGreen;
            statusStrip.Items.Add(hpLabel);
            statusStrip.Items.Add(medalsLabel);
            statusStrip.Items.Add(energyLabel);
            statusStrip.Items.Add(timerLabel);
            Controls.Add(statusStrip);
        }

        private void StartGameProcess()
        {
            maze.Show();
        }

        private void StartTimer()
        {
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += OnTimerTick;
            gameTimer.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            timerDuration--;
            UpdateTimerLabel();

            if (timerDuration <= 0)
            {
                gameTimer.Stop();
                MessageBox.Show("Время вышло! Игра окончена.");
                this.Close();
            }
        }

        private void UpdateTimerLabel()
        {
            timerLabel.Text = $"Time Left: {timerDuration}s               ";
        }

        private void UpdateHPLabel()
        {
            hpLabel.Text = $"HP: {hpValue}";
        }

        private void UpdateMedalsLabel()
        {
            medalsLabel.Text = $"Medals: {medals}/{totalMedals}";
        }

        private void UpdateEnergyLabel()
        {
            energyLabel.Text = $"Energy: {energy}";
        }

        public void DecreaseHP(int amount)
        {
            hpValue -= amount;
            if (hpValue < 0) hpValue = 0;
            UpdateHPLabel();
            if (hpValue == 0)
            {
                MessageBox.Show("0 HP. You lose!");
                this.Close();
            }
        }

        public void IncreaseHP(int amount)
        {
            if (hpValue < 3)
            {
                hpValue += amount;
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
                UpdateMedalsLabel();
                if (medals == totalMedals)
                {
                    MessageBox.Show("Победа - медали собраны!");
                    this.Close();
                }
            }
            else if (t == CellType.ENERGY)
            {
                energy += 10;
                UpdateEnergyLabel();
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            CellType previous = CellType.HERO;
            if (e.KeyCode == Keys.Right && maze.cells[Hero.PosY, Hero.PosX + 1].Type != CellType.WALL)
            {
                previous = maze.cells[Hero.PosY, Hero.PosX + 1].Type;
                
                energy--;
                UpdateEnergyLabel();
                Hero.Clear();
                Hero.MoveRight();
                if (Hero.PosX == 19)
                {
                    MessageBox.Show("Вы нашли выход!");
                    Close();
                }
                Hero.Show();
                CheckEvent(previous);
            }
            else if (e.KeyCode == Keys.Left && Hero.PosX != 0 && maze.cells[Hero.PosY, Hero.PosX - 1].Type != CellType.WALL)
            {
                previous = maze.cells[Hero.PosY, Hero.PosX - 1].Type;
                energy--;
                UpdateEnergyLabel();
                Hero.Clear();
                Hero.MoveLeft();
                Hero.Show();
                CheckEvent(previous);
            }
            else if (e.KeyCode == Keys.Up && maze.cells[Hero.PosY - 1, Hero.PosX].Type != CellType.WALL)
            {
                previous = maze.cells[Hero.PosY - 1, Hero.PosX].Type;
                energy--;
                UpdateEnergyLabel();
                Hero.Clear();
                Hero.MoveUp();
                Hero.Show();
                CheckEvent(previous);
            }
            else if (e.KeyCode == Keys.Down && maze.cells[Hero.PosY + 1, Hero.PosX].Type != CellType.WALL)
            {
                previous = maze.cells[Hero.PosY + 1, Hero.PosX].Type;
                energy--;
                UpdateEnergyLabel();
                Hero.Clear();
                Hero.MoveDown();
                Hero.Show();
                CheckEvent(previous);
            }
        }
    }
}
