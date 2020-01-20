using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AAYFinalProj19
{
    public partial class ReservationForm : Form
    {
        Film f = new Film();
        int availSeats;
        List<Button> selectedSeats = new List<Button>();
        List<String> productList = new List<String>();
        IDictionary<String, bool> paymentData = new Dictionary<String, bool>()
                                                        {
                                                            {"number", false},
                                                            {"expMonth", true},
                                                            {"expYear", true},
                                                            {"name", false},
                                                            {"cvc", false}
                                                        };


        public ReservationForm()
        {
            InitializeComponent();
            loadFilm();
            
        }
        private void loadFilm()
        {
            JObject data = JObject.Parse(File.ReadAllText(Application.StartupPath + "\\films.json"));
            IList<Film> films = new List<Film>();
            foreach (JToken t in data.Children())
            {
                Film film = new Film();
                film.Filename = (String)t.First["filename"];
                film.Description = (String)t.First["description"];
                var ar = t.First["seats"];
                bool[][] temp1 = new bool[10][];
                
                int i = 0;
                
                foreach(var k in ar)
                {
                    int j = 0;
                    bool[] temp2 = new bool[9];
                    foreach (var r in k)
                    {
                        
                        temp2[j] = (bool)r;
                        j++;
                    }
                    temp1[i] = temp2;
                    i++;
                }
                film.Seats = temp1;
                films.Add(film);
            }
            f = films.First();


        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ReservationForm_Load(object sender, EventArgs e)
        {
            switchPanels(seatsChoicePanel);
            var allButtons = seatsPanel.Controls.Cast<Button>();
            availSeats = 90;
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j<9; j++)
                {
                    if (f.Seats[i][j])
                    {
                        availSeats--;
                        Button button = allButtons.Where(b => b.Name.Equals("button" + ((i * 9) + j + 1))).Single();
                        button.BackColor = Color.Red;
                        button.Enabled = false;
                    }
                }
            }

            availTicketsLbl.Text = "Available Tickets: " + availSeats.ToString();

        }

        private void seatsChoicePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.BackColor == Color.Green)
            {
                selectedSeats.Remove(b);
                b.BackColor = Color.Silver;
            }else if(b.BackColor == Color.Silver)
            {
                selectedSeats.Add(b);
                b.BackColor = Color.Green;
            }
            selectedLbl.Text = "You have selected " + selectedSeats.Count.ToString() + " seats.";
        }

        private void button91_Click(object sender, EventArgs e)
        {

            foreach (Button b in selectedSeats)
            {        
                b.BackColor = Color.Silver;    
            }
            selectedSeats.Clear();
            selectedLbl.Text = "You have selected 0 seats.";

        }

        private void continueBtn_Click(object sender, EventArgs e)
        {
            
            foreach(Button b in selectedSeats)
            {
                String prod = "Seat no." + b.Name.Substring(6);             
                productList.Add(prod);
            }
            updatePanel(showResPanel);
            switchPanels(reservPanel);
        }

        private void switchPanels(Panel p)
        {
            var allPanels = this.Controls.Cast<Control>().Where(c => c.GetType() == typeof(Panel));

            foreach (var panel in allPanels)
            {
                panel.Dock = DockStyle.None;
                panel.Visible = false;
            }

            p.Dock = DockStyle.Fill;
            p.Visible = true;
        }

        private void updatePanel(Panel panel)
        {
            panel.Controls.Clear();
            foreach (String prod in productList)
            {
                         
                Panel p = new Panel();
                p.Parent = panel;
                p.Width = panel.Width - 20;
                p.Height = 50;
                p.BorderStyle = BorderStyle.FixedSingle;

                Label name = new Label();
                name.Parent = p;
                name.Text = prod;
                name.Font = new Font("Calibri", 13F);
                name.Dock = DockStyle.Left;
                name.Margin = new Padding(10);

                Button del = new Button();
                del.Parent = p;
                del.Text = "Delete";
                del.Font = new Font("Calibri", 10F);
                del.Width = 65;
                del.Height = 15;
                del.BackColor = Color.Red;
                del.Dock = DockStyle.Right;
                del.Margin = new Padding(8);
                del.Click += new EventHandler(this.deleteProduct);
                del.MouseHover += new EventHandler(this.deleteButton_MouseHover);

                

                p.Controls.Add(name);
                p.Controls.Add(del);
                panel.Controls.Add(p);
            }

                        
        }

        private void deleteProduct(object sender, EventArgs e)
        {
            
            productList.Remove(productList.First());
            selectedSeats.Remove(selectedSeats.First());
            updatePanel(showResPanel);
            if(selectedSeats.Count == 0)
            {
                switchPanels(seatsChoicePanel);
                foreach(Button b in seatsPanel.Controls.Cast<Button>())
                {
                    if(b.BackColor == Color.Green)
                    {
                        b.BackColor = Color.Silver;
                    }
                }

                selectedLbl.Text = "You have selected 0 seats.";
            }
        }

        private void deleteButton_MouseHover(object sender, EventArgs e)
        {
            deleteToolTip.SetToolTip((Control)sender, "Delete this product.");
        }

        private void buyBtn_Click(object sender, EventArgs e)
        {
            switchPanels(paymentPanel);
            updatePanel(finalResPanel);
        }

        private void addPopBtn_Click(object sender, EventArgs e)
        {
            String prod = "Pop Corn x" + (int)popNum.Value;
            productList.Add(prod);
            updatePanel(showResPanel);
        }

        private void addNachosBtn_Click(object sender, EventArgs e)
        {
            String prod = "Nachos x" + (int)nachosNum.Value;
            productList.Add(prod);
            updatePanel(showResPanel);
        }

        private void addChipsBtn_Click(object sender, EventArgs e)
        {
            String prod = "Chips x" + (int)chipsNum.Value;
            productList.Add(prod);
            updatePanel(showResPanel);
        }

        private void addCokeBtn_Click(object sender, EventArgs e)
        {
            String prod = "Coke x" + (int)cokeNum.Value;
            productList.Add(prod);
            updatePanel(showResPanel);
        }

        private void addWaterBtn_Click(object sender, EventArgs e)
        {
            String prod = "Water x" + (int)waterNum.Value;
            productList.Add(prod);
            updatePanel(showResPanel);
        }

        private void cancelBtn1_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Are you sure?", "Cancel", MessageBoxButtons.YesNo);
            if(res == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void checkFullfillment()
        {
            if (paymentData.Values.All(v => v))
            {
                finishBtn.Enabled = true;
            }
            else
            {
                finishBtn.Enabled = false;
            }
        }
    
        private void finishBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you! Check your email for your tikcets.");
            this.Close();
        }

        private void cancelBtn2_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("You you like to cancel your reservation?", "Cancel", MessageBoxButtons.YesNo);
            if(res == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void cardNumTxtBx_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(cardNumTxtBx.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                cardNumTxtBx.Text = cardNumTxtBx.Text.Remove(cardNumTxtBx.Text.Length - 1);
            }

            if (cardNumTxtBx.TextLength == 16)
            {
                paymentData["number"] = true;
            }
            else
            {
                paymentData["number"] = false;
            }

            checkFullfillment();

        }

        private void monthCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void yearCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ownersNameTxtBx_TextChanged(object sender, EventArgs e)
        {
            if (ownersNameTxtBx.TextLength >= 3)
            {
                paymentData["name"] = true;
            }
            else
            {
                paymentData["name"] = false;
            }

            checkFullfillment();
        }

        private void cvcTxtBx_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(cvcTxtBx.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                cvcTxtBx.Text = cvcTxtBx.Text.Remove(cvcTxtBx.Text.Length - 1);
            }

            if (cvcTxtBx.TextLength == 3)
            {
                paymentData["cvc"] = true;
            }
            else
            {
                paymentData["cvc"] = false;
            }

            checkFullfillment();
        }

        private void finishDiv_MouseHover(object sender, EventArgs e)
        {
            if (!finishBtn.Enabled)
            {
                finishToolTip.SetToolTip(finishDiv, "Please complete the payment form.");
            }
        }

        private void cardNumTxtBx_MouseHover(object sender, EventArgs e)
        {
            cardNumToolTip.SetToolTip(cardNumTxtBx, "Insert the 16-digit card's number.");
        }

        private void monthCmbBx_MouseHover(object sender, EventArgs e)
        {
            expMonthToolTip.SetToolTip(monthCmbBx, "Choose month that card expires.");
        }

        private void yearCmbBx_MouseHover(object sender, EventArgs e)
        {
            expYearToolTip.SetToolTip(yearCmbBx, "Choose year that card expires.");
        }

        private void ownersNameTxtBx_MouseHover(object sender, EventArgs e)
        {
            nameToolTip.SetToolTip(ownersNameTxtBx, "Insert card's owner name.");
        }

        private void cvcTxtBx_MouseHover(object sender, EventArgs e)
        {
            cvcToolTip.SetToolTip(cvcTxtBx, "Insert card's cvc.\nThe cvc is located under the magnetic tape of the card.");
        }

        private void continueBtn_MouseHover(object sender, EventArgs e)
        {
            continueToolTip.SetToolTip(continueBtn, "Continue the booking process.");
        }

        private void button91_MouseHover(object sender, EventArgs e)
        {
            resetSeatsToolTip.SetToolTip(button91, "Reset your seat selection.");
        }

        private void addPopBtn_MouseHover(object sender, EventArgs e)
        {
            addToolTip.SetToolTip((Control)sender, "Add this product to your reservations.");
        }

        
    }
}
