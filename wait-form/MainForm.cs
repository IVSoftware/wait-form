using System.Runtime.CompilerServices;

namespace wait_form
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            buttonLoadData.Click += onClickLoadData;
        }

        private async void onClickLoadData(object? sender, EventArgs e)
        {
            using (var waitForm = new WinFormWaitingPanel(this))
            {
                await Task.Run(() => LoadData());
            }
        }
        private void LoadData()
        {
            // Mock "any" long-running synchronous task
            // (SQL or otherwise)
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
    class WinFormWaitingPanel : Form
    {
        public WinFormWaitingPanel(Form owner)
        {            
            Owner = owner;

            // Size this form to cover the main form's client rectangle.
            Size = Owner.ClientRectangle.Size;
            FormBorderStyle = FormBorderStyle.None;
            UseWaitCursor = true;
            StartPosition = FormStartPosition.Manual;
            Location = PointToClient(
                Owner.PointToScreen(Owner.ClientRectangle.Location));

            // Add a label that says "Loading..."
            var label = new Label
            {
                Text = "Loading...",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
            };
            Controls.Add(label);

            // Set the form color to see-through Blue
            BackColor = Color.LightBlue;
            Opacity = .5;

            var forceHandle = Handle;
            BeginInvoke(() => ShowDialog());
        }
    }
}