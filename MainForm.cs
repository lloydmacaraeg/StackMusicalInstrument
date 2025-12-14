using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Musical
{
    public partial class MainForm : Form
    {
        private string connString = "Server=localhost;Database=musicdb;Uid=root;Pwd=12Bernerslee23@;";
        private Panel searchPanel;
        private ComboBox cmbInstruments;
        private Button btnSearch;
        private Label lblTitle;

        public MainForm()
        {
            InitializeControls();
            LoadInstruments();
        }

        private void InitializeControls()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(900, 600);
            this.Name = "MainForm";
            this.Text = "Music Instrument Explorer";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(129, 93, 72);

            // Title Label
            lblTitle = new Label
            {
                Text = "🎵 Music Instrument Explorer",
                Font = new Font("Courier", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 80),
                Size = new Size(800, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            // Search Panel
            searchPanel = new Panel
            {
                Location = new Point(150, 250),
                Size = new Size(600, 120),
                BackColor = Color.White
            };
            searchPanel.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(225, 220, 217), 2),
                    0, 0, searchPanel.Width - 1, searchPanel.Height - 1);
            };

            // Instrument Label
            Label lblInstrument = new Label
            {
                Text = "Select Instrument Type:",
                Font = new Font("Courier", 14),
                ForeColor = Color.Black,
                Location = new Point(30, 25),
                Size = new Size(220, 30)
            };
            searchPanel.Controls.Add(lblInstrument);

            // Instrument ComboBox
            cmbInstruments = new ComboBox
            {
                Location = new Point(260, 25),
                Size = new Size(300, 30),
                Font = new Font("Courier", 12),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(231, 223, 189),
                FlatStyle = FlatStyle.Flat
            };
            searchPanel.Controls.Add(cmbInstruments);

            // Search Button
            btnSearch = new Button
            {
                Text = "Search Albums",
                Location = new Point(200, 70),
                Size = new Size(200, 40),
                Font = new Font("Courier", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(231, 223, 189),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);

            this.Controls.Add(searchPanel);
            this.ResumeLayout(false);
        }

        private void LoadInstruments()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT instrument_type FROM instruments ORDER BY instrument_type";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbInstruments.Items.Add(reader["instrument_type"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading instruments: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (cmbInstruments.SelectedItem == null)
            {
                MessageBox.Show("Please select an instrument type!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string instrumentType = cmbInstruments.SelectedItem.ToString();
            AlbumForm albumForm = new AlbumForm(instrumentType, connString);
            albumForm.ShowDialog();
        }
    }
}