using System;
using System.IO;
using System.Windows.Forms;
using Echo.Definitions;

namespace Echo;

internal partial class OptionsForm : Form
{
    public OptionsForm()
    {
        InitializeComponent();
        darkAgesPath.Text = Settings.Instance.DarkAgesPath;
        UseDawndCbox.Checked = Settings.Instance.UseDawnd;
        UseDDrawCompatCbox.Checked = Settings.Instance.UseDDrawCompat;
    }

    private void BrowseDADirectoryButton_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = @"Executable File (*.exe)|*.exe",
            InitialDirectory = Path.GetDirectoryName(Settings.Instance.DarkAgesPath)
        };

        if (openFileDialog.ShowDialog() != DialogResult.OK)
            return;

        darkAgesPath.Text = openFileDialog.FileName;
    }

    public void Save()
    {
        Settings.Instance.DarkAgesPath = darkAgesPath.Text;
        Settings.Instance.UseDawnd = UseDawndCbox.Checked;
        Settings.Instance.UseDDrawCompat = UseDDrawCompatCbox.Checked;
        Settings.Instance.Save();
    }

    private void SaveSettingsButton_Click(object sender, EventArgs e)
    {
        Save();
        Close();
    }
}