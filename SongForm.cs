using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Musical
{
    public partial class SongForm : Form
    {
        private int albumId;
        private string albumName;
        private string connString;
        private DataGridView dgvSongs;

        public SongForm(int album, string name, string connection)
        {
            albumId = album;
            albumName = name;
            connString = connection;
            InitializeControls();
            LoadSongs();
        }

        private void InitializeControls()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1100, 700);
            this.Name = "SongForm";
            this.Text = $"Songs - {albumName}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(129, 93, 72);

            Label lblTitle = new Label
            {
                Text = $"Songs from: {albumName}",
                Font = new Font("Courier", 22, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 30),
                Size = new Size(1000, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            dgvSongs = new DataGridView
            {
                Location = new Point(50, 100),
                Size = new Size(1000, 550),
                BackgroundColor = Color.FromArgb(211, 183, 167),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(60, 60, 100),
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Courier", 11)
            };
            dgvSongs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 70);
            dgvSongs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSongs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgvSongs.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 60);
            dgvSongs.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 130, 180);
            dgvSongs.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvSongs.CellDoubleClick += DgvSongs_CellDoubleClick;

            this.Controls.Add(dgvSongs);
            this.ResumeLayout(false);
        }

        private void LoadSongs()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = @"SELECT song_id, song_name, duration, genre 
                                   FROM songs WHERE album_id = @albumId ORDER BY song_name";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@albumId", albumId);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSongs.DataSource = dt;
                    dgvSongs.Columns["song_id"].Visible = false;
                    dgvSongs.Columns["song_name"].HeaderText = "Song Title";
                    dgvSongs.Columns["duration"].HeaderText = "Duration";
                    dgvSongs.Columns["genre"].HeaderText = "Genre";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading songs: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSongs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int songId = Convert.ToInt32(dgvSongs.Rows[e.RowIndex].Cells["song_id"].Value);
                string songName = dgvSongs.Rows[e.RowIndex].Cells["song_name"].Value.ToString();

                PlayerForm playerForm = new PlayerForm(songId, songName, connString);
                playerForm.ShowDialog();
            }
        }
    }
}