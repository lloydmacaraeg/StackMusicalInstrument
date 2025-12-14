using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Musical
{
    public partial class PlayerForm : Form
    {
        private int songId;
        private string songName;
        private string connString;
        private Label lblSongTitle, lblArtist, lblAlbum, lblDuration, lblGenre, lblInstruments;
        private Button btnPlay, btnPause, btnStop;
        private System.Media.SoundPlayer player;

        public PlayerForm(int song, string name, string connection)
        {
            songId = song;
            songName = name;
            connString = connection;
            InitializeControls();
            LoadSongInfo();
        }

        private void InitializeControls()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(700, 600);
            this.Name = "PlayerForm";
            this.Text = "Now Playing";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(15, 15, 35);

            Label lblTitle = new Label
            {
                Text = "♪ Now Playing ♪",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 30),
                Size = new Size(600, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            Panel infoPanel = new Panel
            {
                Location = new Point(50, 100),
                Size = new Size(600, 350),
                BackColor = Color.FromArgb(30, 30, 60)
            };

            lblSongTitle = CreateInfoLabel("Song: ", 20);
            lblArtist = CreateInfoLabel("Artist: ", 70);
            lblAlbum = CreateInfoLabel("Album: ", 120);
            lblDuration = CreateInfoLabel("Duration: ", 170);
            lblGenre = CreateInfoLabel("Genre: ", 220);
            lblInstruments = CreateInfoLabel("Instruments: ", 270);

            infoPanel.Controls.Add(lblSongTitle);
            infoPanel.Controls.Add(lblArtist);
            infoPanel.Controls.Add(lblAlbum);
            infoPanel.Controls.Add(lblDuration);
            infoPanel.Controls.Add(lblGenre);
            infoPanel.Controls.Add(lblInstruments);
            this.Controls.Add(infoPanel);

            Panel controlPanel = new Panel
            {
                Location = new Point(150, 480),
                Size = new Size(400, 80),
                BackColor = Color.FromArgb(25, 25, 50)
            };

            btnPlay = CreateControlButton("▶ Play", 20);
            btnPause = CreateControlButton("⏸ Pause", 150);
            btnStop = CreateControlButton("⏹ Stop", 280);

            btnPlay.Click += BtnPlay_Click;
            btnPause.Click += BtnPause_Click;
            btnStop.Click += BtnStop_Click;

            controlPanel.Controls.Add(btnPlay);
            controlPanel.Controls.Add(btnPause);
            controlPanel.Controls.Add(btnStop);
            this.Controls.Add(controlPanel);

            this.ResumeLayout(false);
        }

        private Label CreateInfoLabel(string prefix, int y)
        {
            return new Label
            {
                Text = prefix,
                Font = new Font("Segoe UI", 13),
                ForeColor = Color.White,
                Location = new Point(20, y),
                Size = new Size(560, 40),
                AutoSize = false
            };
        }

        private Button CreateControlButton(string text, int x)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, 20),
                Size = new Size(110, 45),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadSongInfo()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = @"SELECT s.song_name, s.duration, s.genre, s.audio_path, 
                                   a.album_name, a.artist 
                                   FROM songs s 
                                   INNER JOIN albums a ON s.album_id = a.album_id 
                                   WHERE s.song_id = @songId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@songId", songId);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblSongTitle.Text = "Song: " + reader["song_name"].ToString();
                        lblArtist.Text = "Artist: " + reader["artist"].ToString();
                        lblAlbum.Text = "Album: " + reader["album_name"].ToString();
                        lblDuration.Text = "Duration: " + reader["duration"].ToString();
                        lblGenre.Text = "Genre: " + reader["genre"].ToString();

                        string audioPath = reader["audio_path"].ToString();
                        if (!string.IsNullOrEmpty(audioPath) && System.IO.File.Exists(audioPath))
                        {
                            player = new System.Media.SoundPlayer(audioPath);
                        }
                    }
                    reader.Close();

                    string instrumentQuery = @"SELECT i.instrument_name 
                                             FROM instruments i 
                                             INNER JOIN song_instruments si ON i.instrument_id = si.instrument_id 
                                             WHERE si.song_id = @songId";

                    cmd = new MySqlCommand(instrumentQuery, conn);
                    cmd.Parameters.AddWithValue("@songId", songId);
                    reader = cmd.ExecuteReader();

                    string instruments = "";
                    while (reader.Read())
                    {
                        instruments += reader["instrument_name"].ToString() + ", ";
                    }
                    lblInstruments.Text = "Instruments: " + instruments.TrimEnd(',', ' ');
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading song info: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                try
                {
                    player.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error playing audio: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No audio file available for this song.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Stop();
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Stop();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (player != null)
                {
                    player.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
