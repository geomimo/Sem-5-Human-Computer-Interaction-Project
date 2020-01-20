using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;

namespace AAYFinalProj19
{
    public partial class mainForm : Form
    {
        IDictionary<String, String> USERS = new Dictionary<String, String>();
        User curUser = new User();


        public mainForm()
        {
            InitializeComponent();

            USERS.Add("client", "client");
            USERS.Add("employee", "employee");

            
        }


        private void mainForm_Load(object sender, EventArgs e)
        {
            dockAll();
            switchPanels(homePanel);
            accountBtn.Visible = false;
            loadFilms();
        }

        private void dockAll()
        {
            var allPanels = getAll(this, typeof(Panel));
            foreach(var panel in allPanels)
            {
                if (panel.Name != "navBarPanel") panel.Dock = DockStyle.Bottom;
            }
        }

        public IEnumerable<Control> getAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.Where(c => c.GetType() == type);
        }

        private void switchPanels(Panel p)
        {
            var allPanels = getAll(this, typeof(Panel));

            foreach(var panel in allPanels)
            {
                if (panel.Name != "navBarPanel" ) panel.Visible = false;
            }
            

            p.Visible = true;
        }


        private void loginBtn_Click(object sender, EventArgs e)
        {
            String username = usernameTxtBx.Text;
            String password = passwordTxtBx.Text;
            try
            {
                if (USERS[username].Equals(password))
                {
                    curUser.Username = username;
                    curUser.Password = password;

                    switchPanels(homePanel);
                    accountBtn.Visible = true;
                    loginPageBtn.Text = "Exit";
                    MessageBox.Show("You have successfully logged in!");
                }
                else
                {
                    MessageBox.Show("User not found! Please try again.");
                    usernameTxtBx.Text = String.Empty;
                    passwordTxtBx.Text = String.Empty;
                }
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("User not found! Please try again.");
                usernameTxtBx.Text = String.Empty;
                passwordTxtBx.Text = String.Empty;
            }
           
        }

        private void loginPageBtn_Click(object sender, EventArgs e)
        {
            if (loginPageBtn.Text.Equals("Login"))
            {
                switchPanels(loginPanel);
            }
            else
            {
                MessageBox.Show("Have a good day. Bye!");
                curUser = new User();
                loginPageBtn.Text = "Login";
                accountBtn.Visible = false;
                switchPanels(homePanel);
            }
        }

        private void homeBtn_Click(object sender, EventArgs e)
        {
            switchPanels(homePanel);
        }
        private void loadFilms()
        {
            JObject data = JObject.Parse(File.ReadAllText(Application.StartupPath + "\\films.json"));
            IList<Film> films = new List<Film>();
            foreach (JToken t in data.Children())
            {
                Film f = new Film();
                f.Filename = (String)t.First["filename"];
                f.Description = (String)t.First["description"];
                films.Add(f);
            }

            
            FlowLayoutPanel mainPanel = new FlowLayoutPanel();
            mainPanel.Parent = filmsPanel;
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Width = filmsPanel.Width;
            mainPanel.FlowDirection = FlowDirection.LeftToRight;          
            mainPanel.AutoScroll = true;

            foreach (Film f in films)
            {

                FlowLayoutPanel p = new FlowLayoutPanel();
                p.FlowDirection = FlowDirection.TopDown;
                p.Width = mainPanel.Width - 30;
                p.Height = 200;
                p.Parent = mainPanel;
                p.BorderStyle = BorderStyle.Fixed3D;

                PictureBox postPctBx = new PictureBox();
                postPctBx.Width = 135;
                postPctBx.Height = 200;
                postPctBx.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Posters\\" + f.Filename);
                postPctBx.Parent = p;
                postPctBx.BackgroundImageLayout = ImageLayout.Stretch;

                Label titleLabel = new Label();
                titleLabel.Text = f.Filename.Substring(0, f.Filename.Length - 4);
                titleLabel.Parent = p;
                titleLabel.Dock = DockStyle.Top;
                titleLabel.TextAlign = ContentAlignment.MiddleCenter;
                titleLabel.Font = new Font("Calibri", 20.0F, FontStyle.Bold);
                titleLabel.Height = 30;

                RichTextBox descText = new RichTextBox();
                descText.Text = f.Description;
                descText.Font = new Font("Calibri", 12.5F);
                descText.BackColor = Color.FromArgb(255, 214, 191);
                descText.BorderStyle = BorderStyle.None;
                descText.Parent = p;
                descText.Width = p.Width - postPctBx.Width - 100;
                descText.Height = p.Height - titleLabel.Height - 75;
                descText.WordWrap = true;
                
                

                Button buyTicketsButton = new Button();
                buyTicketsButton.Text = "Buy Tickets";
                buyTicketsButton.Font = new Font("Calibri", 12F);
                buyTicketsButton.BackColor = Color.FromArgb(242, 150, 97);
                buyTicketsButton.Parent = p;
                buyTicketsButton.Height = 50;
                buyTicketsButton.Width = 200;
                buyTicketsButton.Click += new EventHandler(this.buyTicketsButtonClick);


                p.Controls.Add(postPctBx);
                p.Controls.Add(titleLabel);
                p.Controls.Add(descText);
                p.Controls.Add(buyTicketsButton);
                mainPanel.Controls.Add(p);
                
            }
            filmsPanel.Controls.Add(mainPanel);
           
        }

        private void buyTicketsButtonClick(object sender, EventArgs e)
        {
            switchPanels(filmInfoPanel);
        }

        private void filmsBtn_Click(object sender, EventArgs e)
        {
            switchPanels(filmsPanel);
        }

        private void button12_Click(object sender, EventArgs e)
        {
          
            ReservationForm resForm = new ReservationForm();
            resForm.ShowDialog();
           
        }

        private void infoBackBtn_Click(object sender, EventArgs e)
        {
            switchPanels(filmsPanel);
        }

       
    }
}
