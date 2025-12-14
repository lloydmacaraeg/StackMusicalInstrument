using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Musical
{
    public partial class AlbumForm : Form
    {
        private string instrumentType;
        private string connString;
        private FlowLayoutPanel albumPanel;

        public AlbumForm(string instrument, string connection)
        {
            instrumentType = instrument;
            connString = connection;
            InitializeControls();
            LoadAlbums();
        }

        private void InitializeControls()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 700);
            this.Name = "AlbumForm";
            this.Text = $"Albums - {instrumentType}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(129, 93, 72);

            // Title
            Label lblTitle = new Label
            {
                Text = $"Albums featuring {instrumentType}",
                Font = new Font("Courier", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 30),
                Size = new Size(900, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            // Album Panel
            albumPanel = new FlowLayoutPanel
            {
                Location = new Point(50, 100),
                Size = new Size(900, 550),
                AutoScroll = true,
                BackColor = Color.FromArgb(211, 183, 167)
            };
            this.Controls.Add(albumPanel);

            this.ResumeLayout(false);
        }

        private void LoadAlbums()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = @"SELECT DISTINCT a.album_id, a.album_name, a.artist, a.release_year 
                                   FROM albums a 
                                   INNER JOIN songs s ON a.album_id = s.album_id 
                                   INNER JOIN song_instruments si ON s.song_id = si.song_id 
                                   INNER JOIN instruments i ON si.instrument_id = i.instrument_id 
                                   WHERE i.instrument_type = @instrumentType 
                                   ORDER BY a.album_name";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@instrumentType", instrumentType);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Panel albumCard = CreateAlbumCard(
                            Convert.ToInt32(reader["album_id"]),
                            reader["album_name"].ToString(),
                            reader["artist"].ToString(),
                            reader["release_year"].ToString()
                        );
                        albumPanel.Controls.Add(albumCard);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading albums: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateAlbumCard(int albumId, string albumName, string artist, string year)
        {
            Panel card = new Panel
            {
                Size = new Size(250, 200),
                BackColor = Color.FromArgb(40, 40, 70),
                Margin = new Padding(15),
                Cursor = Cursors.Hand
            };

            Label lblAlbum = new Label
            {
                Text = albumName,
                Font = new Font("Courier", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 20),
                Size = new Size(230, 60),
                TextAlign = ContentAlignment.TopCenter
            };

            Label lblArtist = new Label
            {
                Text = artist,
                Font = new Font("Courier", 11),
                ForeColor = Color.LightGray,
                Location = new Point(10, 90),
                Size = new Size(230, 30),
                TextAlign = ContentAlignment.TopCenter
            };

            Label lblYear = new Label
            {
                Text = year,
                Font = new Font("Courier", 10),
                ForeColor = Color.Gray,
                Location = new Point(10, 125),
                Size = new Size(230, 25),
                TextAlign = ContentAlignment.TopCenter
            };

            Button btnOpen = new Button
            {
                Text = "View Songs",
                Location = new Point(50, 155),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Courier", 10, FontStyle.Bold)
            };
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.Click += (s, e) =>
            {
                SongForm songForm = new SongForm(albumId, albumName, connString);
                songForm.ShowDialog();
            };

            card.Controls.Add(lblAlbum);
            card.Controls.Add(lblArtist);
            card.Controls.Add(lblYear);
            card.Controls.Add(btnOpen);

            card.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(100, 100, 200), 2),
                    0, 0, card.Width - 1, card.Height - 1);
            };

            return card;
        }
    }
}